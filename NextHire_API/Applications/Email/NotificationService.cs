using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NextHire_API.Contexts;
using NextHire_API.Domains;

namespace NextHire_API.Applications.Email
{
    public class NotificationService
    {
        private const string TipoCandidaturaRecebida = "candidatura_recebida";
        private const string TipoCandidaturaCancelada = "candidatura_cancelada";
        private const string TipoStatus = "status";
        private const string TipoResultado = "resultado";
        private const string TipoEntrevista = "entrevista";
        private const string TipoNovaCandidatura = "nova_candidatura";
        private const string TipoMarco = "marco";
        private const string TipoCandidatoLiberado = "candidato_liberado";
        private const string TipoFeedback = "feedback";

        private readonly NextHireContext _context;
        private readonly IEmailService _email;
        private readonly EmailOutboxStore _outbox;
        private readonly ILogger<NotificationService> _logger;

        public NotificationService(
            NextHireContext context,
            IEmailService email,
            EmailOutboxStore outbox,
            ILogger<NotificationService> logger)
        {
            _context = context;
            _email = email;
            _outbox = outbox;
            _logger = logger;
        }

        private async Task EnviarComSeguranca(EmailMessage mensagem, int usuarioId, string tipo, CancellationToken ct)
        {
            try
            {
                if (!await _outbox.PreferenciaHabilitadaAsync(usuarioId, tipo, ct))
                {
                    _logger.LogInformation("Notificação {Tipo} desabilitada para o usuário {UsuarioId}", tipo, usuarioId);
                    return;
                }

                await _email.EnviarAsync(mensagem, ct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Falha ao disparar notificação {Tipo} para {Destinatario}", tipo, mensagem.Destinatario);
            }
        }

        public async Task CandidaturaRecebida(int curriculoId, int vagaId, CancellationToken ct = default)
        {
            CurriculoVaga? candidatura = await _context.CurriculoVaga
                .Include(c => c.Curriculo).ThenInclude(c => c.Candidato)
                .Include(c => c.Vaga).ThenInclude(v => v.Empresa)
                .Include(c => c.Vaga).ThenInclude(v => v.AnalistaRhNavigation)
                .FirstOrDefaultAsync(c => c.CurriculoId == curriculoId && c.VagaId == vagaId, ct);

            if (candidatura is null) return;

            Usuario candidato = candidatura.Curriculo.Candidato;
            Vaga vaga = candidatura.Vaga;
            string data = (candidatura.DataHoraInscricao ?? DateTime.Now).ToString("dd/MM/yyyy HH:mm");

            var dadosCandidato = new Dictionary<string, string>
            {
                ["nome"] = candidato.Nome,
                ["vaga"] = vaga.Titulo,
                ["empresa"] = vaga.Empresa.Nome,
                ["data"] = data
            };

            await EnviarComSeguranca(new EmailMessage
            {
                Destinatario = candidato.Email,
                NomeDestinatario = candidato.Nome,
                Assunto = $"Candidatura recebida: {vaga.Titulo}",
                Template = "candidatura-recebida",
                Dados = dadosCandidato,
                DedupeKey = $"candidatura-recebida:{curriculoId}:{vagaId}"
            }, candidato.UsuarioId, TipoCandidaturaRecebida, ct);

            Usuario? analista = vaga.AnalistaRhNavigation;
            if (analista is not null)
            {
                var dadosRh = new Dictionary<string, string>(dadosCandidato)
                {
                    ["candidato"] = candidato.Nome,
                    ["vaga"] = vaga.Titulo,
                    ["empresa"] = vaga.Empresa.Nome
                };

                await EnviarComSeguranca(new EmailMessage
                {
                    Destinatario = analista.Email,
                    NomeDestinatario = analista.Nome,
                    Assunto = $"Nova candidatura para {vaga.Titulo}",
                    Template = "nova-candidatura",
                    Dados = dadosRh,
                    DedupeKey = $"nova-candidatura:{curriculoId}:{vagaId}"
                }, analista.UsuarioId, TipoNovaCandidatura, ct);

                await VerificarMarcosAsync(vaga, analista, ct);
            }
        }

        private async Task VerificarMarcosAsync(Vaga vaga, Usuario analista, CancellationToken ct)
        {
            int total = await _context.CurriculoVaga.CountAsync(c => c.VagaId == vaga.VagaId, ct);
            int[] marcos = (Environment.GetEnvironmentVariable("EMAIL_MILESTONES") ?? "10,25,50,100")
                .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Select(m => int.TryParse(m, out int valor) ? valor : 0)
                .Where(m => m > 0)
                .ToArray();

            if (!marcos.Contains(total)) return;

            await EnviarComSeguranca(new EmailMessage
            {
                Destinatario = analista.Email,
                NomeDestinatario = analista.Nome,
                Assunto = $"Marco de {total} candidaturas em {vaga.Titulo}",
                Template = "marco-candidaturas",
                Dados = new Dictionary<string, string>
                {
                    ["nome"] = analista.Nome,
                    ["vaga"] = vaga.Titulo,
                    ["total"] = total.ToString(),
                    ["empresa"] = vaga.Empresa?.Nome ?? string.Empty
                },
                DedupeKey = $"marco:{vaga.VagaId}:{total}"
            }, analista.UsuarioId, TipoMarco, ct);
        }

        public async Task CandidaturaCancelada(int curriculoId, int vagaId, CancellationToken ct = default)
        {
            CurriculoVaga? candidatura = await _context.CurriculoVaga
                .Include(c => c.Curriculo).ThenInclude(c => c.Candidato)
                .Include(c => c.Vaga).ThenInclude(v => v.Empresa)
                .FirstOrDefaultAsync(c => c.CurriculoId == curriculoId && c.VagaId == vagaId, ct);

            if (candidatura is null) return;

            Usuario candidato = candidatura.Curriculo.Candidato;
            await EnviarComSeguranca(new EmailMessage
            {
                Destinatario = candidato.Email,
                NomeDestinatario = candidato.Nome,
                Assunto = $"Candidatura cancelada: {candidatura.Vaga.Titulo}",
                Template = "candidatura-cancelada",
                Dados = new Dictionary<string, string>
                {
                    ["nome"] = candidato.Nome,
                    ["vaga"] = candidatura.Vaga.Titulo,
                    ["empresa"] = candidatura.Vaga.Empresa.Nome
                },
                DedupeKey = $"candidatura-cancelada:{curriculoId}:{vagaId}"
            }, candidato.UsuarioId, TipoCandidaturaCancelada, ct);
        }

        public async Task StatusAtualizado(int candidatoId, int processoSeletivoId, StatusRelacaoEnum status, CancellationToken ct = default)
        {
            CandidatoProcessoSeletivo? relacao = await _context.CandidatoProcessoSeletivo
                .Include(r => r.Candidato)
                .Include(r => r.ProcessoSeletivo).ThenInclude(p => p.Vaga).ThenInclude(v => v.Empresa)
                .Include(r => r.ProcessoSeletivo).ThenInclude(p => p.Vaga).ThenInclude(v => v.AnalistaRhNavigation)
                .FirstOrDefaultAsync(r => r.CandidatoId == candidatoId && r.ProcessoSeletivoId == processoSeletivoId, ct);

            if (relacao is null) return;

            Usuario candidato = relacao.Candidato;
            Vaga vaga = relacao.ProcessoSeletivo.Vaga;
            (string titulo, string descricao) = status switch
            {
                StatusRelacaoEnum.Avanca => ("Você avançou de etapa!", "Seu perfil avançou para a próxima etapa do processo seletivo. Em breve entraremos em contato."),
                StatusRelacaoEnum.NaoAvanca => ("Atualização do processo seletivo", "Agradecemos sua participação. Neste momento seu perfil não seguirá para a próxima etapa."),
                _ => ("Seu perfil está em análise", "Seu perfil está sendo avaliado pela equipe responsável. Avisaremos assim que houver novidades.")
            };

            var dados = new Dictionary<string, string>
            {
                ["nome"] = candidato.Nome,
                ["vaga"] = vaga.Titulo,
                ["empresa"] = vaga.Empresa.Nome,
                ["status"] = titulo,
                ["descricao"] = descricao
            };

            await EnviarComSeguranca(new EmailMessage
            {
                Destinatario = candidato.Email,
                NomeDestinatario = candidato.Nome,
                Assunto = $"{titulo} - {vaga.Titulo}",
                Template = status == StatusRelacaoEnum.NaoAvanca ? "resultado-candidato" : "status-candidatura",
                Dados = dados,
                DedupeKey = $"status:{candidatoId}:{processoSeletivoId}:{status}"
            }, candidato.UsuarioId, status == StatusRelacaoEnum.NaoAvanca ? TipoResultado : TipoStatus, ct);

            if (status != StatusRelacaoEnum.Avanca) return;

            Usuario? gestor = await _context.Empresa
                .Where(e => e.EmpresaId == vaga.EmpresaId)
                .SelectMany(e => e.Funcionario)
                .FirstOrDefaultAsync(u => u.TipoUsuario == TipoUsuarioEnum.Gestor, ct);

            if (gestor is null) return;

            await EnviarComSeguranca(new EmailMessage
            {
                Destinatario = gestor.Email,
                NomeDestinatario = gestor.Nome,
                Assunto = $"Candidato liberado para avaliação: {candidato.Nome}",
                Template = "candidato-liberado",
                Dados = new Dictionary<string, string>
                {
                    ["nome"] = gestor.Nome,
                    ["candidato"] = candidato.Nome,
                    ["vaga"] = vaga.Titulo,
                    ["empresa"] = vaga.Empresa.Nome
                },
                DedupeKey = $"candidato-liberado:{candidatoId}:{processoSeletivoId}"
            }, gestor.UsuarioId, TipoCandidatoLiberado, ct);
        }

        public async Task EntrevistaConfirmada(int entrevistaId, CancellationToken ct = default)
        {
            Entrevista? entrevista = await _context.Entrevista
                .Include(e => e.Candidato)
                .Include(e => e.ProcessoSeletivo).ThenInclude(p => p.Vaga).ThenInclude(v => v.Empresa)
                .FirstOrDefaultAsync(e => e.EntrevistaId == entrevistaId, ct);

            if (entrevista is null) return;

            string link = Environment.GetEnvironmentVariable("INTERVIEW_LINK") ?? string.Empty;
            string inicio = entrevista.HorarioInicio.ToString("dd/MM/yyyy HH:mm");
            string fim = entrevista.HorarioFim.ToString("HH:mm");

            foreach (Usuario candidato in entrevista.Candidato)
            {
                await EnviarComSeguranca(new EmailMessage
                {
                    Destinatario = candidato.Email,
                    NomeDestinatario = candidato.Nome,
                    Assunto = $"Entrevista confirmada: {entrevista.ProcessoSeletivo.Vaga.Titulo}",
                    Template = "confirmacao-entrevista",
                    Dados = new Dictionary<string, string>
                    {
                        ["nome"] = candidato.Nome,
                        ["vaga"] = entrevista.ProcessoSeletivo.Vaga.Titulo,
                        ["empresa"] = entrevista.ProcessoSeletivo.Vaga.Empresa.Nome,
                        ["dataHora"] = inicio,
                        ["horarioFim"] = fim,
                        ["link"] = link
                    },
                    DedupeKey = $"entrevista:{entrevistaId}:{candidato.UsuarioId}:{entrevista.HorarioInicio:O}"
                }, candidato.UsuarioId, TipoEntrevista, ct);
            }

            Vaga vaga = entrevista.ProcessoSeletivo.Vaga;
            Usuario? gestor = await _context.Empresa
                .Where(e => e.EmpresaId == vaga.EmpresaId)
                .SelectMany(e => e.Funcionario)
                .FirstOrDefaultAsync(u => u.TipoUsuario == TipoUsuarioEnum.Gestor, ct);

            if (gestor is null) return;

            await EnviarComSeguranca(new EmailMessage
            {
                Destinatario = gestor.Email,
                NomeDestinatario = gestor.Nome,
                Assunto = $"Feedback pendente: entrevista em {inicio}",
                Template = "feedback-pendente",
                Dados = new Dictionary<string, string>
                {
                    ["nome"] = gestor.Nome,
                    ["vaga"] = vaga.Titulo,
                    ["candidato"] = string.Join(", ", entrevista.Candidato.Select(c => c.Nome)),
                    ["dataHora"] = inicio
                },
                DedupeKey = $"feedback-pendente:{entrevistaId}:{entrevista.HorarioInicio:O}"
            }, gestor.UsuarioId, TipoFeedback, ct);
        }
    }
}
