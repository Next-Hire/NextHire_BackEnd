using Microsoft.EntityFrameworkCore;
using NextHire_API.Contexts;
using NextHire_API.Domains;
using NextHire_API.Exceptions;
using NextHire_API.Interfaces;

namespace NextHire_API.Repository
{
    public class QuestaoRepository : IQuestaoRepository
    {
        private readonly NextHireContext _context;

        public QuestaoRepository(NextHireContext context)
            => _context = context;

        public async Task<List<Questao>> ListarPorVaga(int vagaId)
            => await _context.Questao
                .AsNoTracking()
                .Include(q => q.Alternativa)
                .Include(q => q.Vaga)
                .OrderBy(q => q.QuestaoId)
                .Where(q => q.VagaId == vagaId)
                .ToListAsync() ?? throw new DomainException("Nenhuma questao ou encontrada!");

        public async Task<Questao> BuscarPorId(int questaoId)
            => await _context.Questao
                .AsNoTracking()
                .Include(q => q.Alternativa)
                .Include(q => q.Vaga)
                .FirstOrDefaultAsync(q => q.QuestaoId == questaoId)
                    ?? throw new DomainException("Questão não encontrada.");

        public async Task<Vaga> BuscarVagaParaAgente(int vagaId)
            => await _context.Vaga
                .AsNoTracking()
                .Include(v => v.Setor)
                    .ThenInclude(s => s.AreaOcupacao)
                .Include(v => v.Habilidade)
                .FirstOrDefaultAsync(v => v.VagaId == vagaId)
                    ?? throw new DomainException("Vaga não encontrada.");

        public async Task<List<Questao>> ListarQuestoesExistentesPorVaga(int vagaId)
            => await _context.Questao
                .AsNoTracking()
                .OrderBy(q => q.QuestaoId)
                .Where(q => q.VagaId == vagaId)
                .ToListAsync();

        public async Task Adicionar(Questao questao)
        {
            await _context.Questao.AddAsync(questao);
            await _context.SaveChangesAsync();
        }

        public async Task AdicionarVarias(List<Questao> questoes)
        {
            await using var transacao = await _context.Database.BeginTransactionAsync();

            await _context.Questao.AddRangeAsync(questoes);
            await _context.SaveChangesAsync();

            await transacao.CommitAsync();
        }

        public async Task Atualizar(int questaoId, Questao dados, List<Alternativa>? alternativas)
        {
            var questao = await _context.Questao
                .Include(q => q.Alternativa)
                .FirstOrDefaultAsync(q => q.QuestaoId == questaoId)
                    ?? throw new DomainException("Questão não encontrada.");

            questao.TextoQuestao = dados.TextoQuestao;
            questao.ClasssificacaoQuestao = dados.ClasssificacaoQuestao;
            questao.TipoQuestao = dados.TipoQuestao;

            if (alternativas != null)
            {
                _context.Alternativa.RemoveRange(questao.Alternativa);

                questao.Alternativa = alternativas
                    .Select(a => new Alternativa
                    {
                        TextoAlternativa = a.TextoAlternativa,
                        AlternativaCorreta = a.AlternativaCorreta,
                        QuestaoId = questao.QuestaoId
                    })
                    .ToList();
            }

            await _context.SaveChangesAsync();
        }

        public async Task Remover(int questaoId)
        {
            var questao = await _context.Questao
                .Include(q => q.Alternativa)
                .Include(q => q.CandidatoQuestao)
                .FirstOrDefaultAsync(q => q.QuestaoId == questaoId)
                    ?? throw new DomainException("Questão não encontrada.");

            if (questao.CandidatoQuestao.Count > 0)
                throw new DomainException("Não é possível remover: já existem respostas de candidatos para esta questão.");

            _context.Alternativa.RemoveRange(questao.Alternativa);
            _context.Questao.Remove(questao);
            await _context.SaveChangesAsync();
        }
    }
}
