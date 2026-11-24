using NextHire_API.Domains;
using NextHire_API.DTOs.HabilidadeDto;
using NextHire_API.Interfaces;

namespace NextHire_API.Applications.Services
{
    public class HabilidadesService 
    {
        private readonly IHabilidadesRepository _repository; 
        private readonly IRbosfHabilidadeClient _rbosf;

        public HabilidadesService(IHabilidadesRepository repository, IRbosfHabilidadeClient rbosf)
        {
            _repository = repository;
            _rbosf = rbosf;
        }

        public async Task<List<RbosfHabilidadeDto>> BuscarNoRbosfPorCodigo(string codigo, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(codigo))
                throw new Exception("Insira um código de habilidade.");

            codigo = codigo.Trim().ToUpperInvariant();

            if (codigo.Length < 3 || codigo.Length > 8)
                throw new Exception("Insira um código com 3 a 8 caracteres.");

            return await _rbosf.BuscarPorCodigoAsync(codigo, cancellationToken);
        }

        public async Task<List<RbosfHabilidadeDto>> BuscarNoRbosfPorTermo(string termo, string? classificacao = null, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(termo))
                throw new Exception("Insira um termo para buscar habilidades.");

            termo = termo.Trim();

            if (termo.Length > 80)
                throw new Exception("O termo deve ter no máximo 80 caracteres.");

            string? classificacaoNormalizada = null;

            if (!string.IsNullOrWhiteSpace(classificacao))
            {
                classificacaoNormalizada = classificacao.Trim().ToLowerInvariant() switch
                {
                    "hard" or "hardskill" or "hard-skill" or "hard_skill" => "hard",
                    "soft" or "softskill" or "soft-skill" or "soft_skill" => "soft",
                    _ => throw new Exception("Classificação inválida. Use 'hard' ou 'soft'.")
                };
            }

            return await _rbosf.BuscarPorTermoAsync(termo, classificacaoNormalizada, cancellationToken);
        }

        public async Task<ResultadoImportacaoRbosfDto> ImportarDoRbosf(ImportarHabilidadesRbosfDto dto, CancellationToken cancellationToken = default)
        {
            if (dto == null)
                throw new Exception("Informe os códigos das habilidades a importar.");

            List<string> codigos = (dto.Codigos ?? new List<string>())
                .Where(c => !string.IsNullOrWhiteSpace(c))
                .Select(c => c.Trim().ToUpperInvariant())
                .Distinct(StringComparer.Ordinal)
                .ToList();

            if (codigos.Count == 0)
                throw new Exception("Informe ao menos um código de habilidade.");

            if (codigos.Count > 50)
                throw new Exception("Importe no máximo 50 habilidades por requisição.");

            ResultadoImportacaoRbosfDto resultado = new()
            {
                Solicitados = codigos.Count
            };

            foreach (string codigo in codigos)
            {
                cancellationToken.ThrowIfCancellationRequested();

                List<RbosfHabilidadeDto> encontradas = await _rbosf.BuscarPorCodigoAsync(codigo, cancellationToken);
                RbosfHabilidadeDto? externa = encontradas.FirstOrDefault(h =>
                    string.Equals(h.Codigo?.Trim(), codigo, StringComparison.OrdinalIgnoreCase));

                if (externa == null || string.IsNullOrWhiteSpace(externa.Nome))
                {
                    resultado.NaoEncontrados.Add(codigo);
                    continue;
                }

                string nome = externa.Nome.Trim();
                Habilidade? existente = _repository.BuscarPorCodigo(codigo);

                if (existente == null)
                {
                    Habilidade nova = new()
                    {
                        Codigo = codigo,
                        Nome = nome
                    };

                    _repository.Adicionar(nova);
                    resultado.Importados++;
                    resultado.Habilidades.Add(lerDto(nova));
                    continue;
                }

                if (!string.Equals(existente.Nome, nome, StringComparison.Ordinal))
                {
                    existente.Nome = nome;
                    _repository.Atualizar(existente.HabilidadeId, existente);
                    resultado.Atualizados++;
                }
                else
                {
                    resultado.SemAlteracao++;
                }

                resultado.Habilidades.Add(lerDto(existente));
            }

            return resultado;
        }

        private static ListarHabilidadeDto lerDto(Habilidade habilidade)
        {
            return new ListarHabilidadeDto
            {
                HabilidadeId = habilidade.HabilidadeId,
                Codigo = habilidade.Codigo,
                Nome = habilidade.Nome,
            };
        }

        public List<ListarHabilidadeDto> Listar()
        {
            var habilidades = _repository.Listar();
            return habilidades.Select(lerDto).ToList();
        }

        public ListarHabilidadeDto ListarPorID(int id)
        {
            var habilidade = _repository.BuscarPorId(id);
            if (habilidade == null)
            {
                throw new Exception("Habilidade não encontrada.");
            }
            if(id < 0)
            {
                throw new Exception("O ID da habilidade deve ser um valor positivo.");
            }
            return lerDto(habilidade);
        }

        public ListarHabilidadeDto Adicionar(AdicionarHabilidadeDto habilidadeDto)
        {
            Habilidade habilidade = new Habilidade
            {
                Codigo = habilidadeDto.Codigo?.Trim().ToUpperInvariant() ?? string.Empty,
                Nome = habilidadeDto.Nome?.Trim() ?? string.Empty
            };
            if (habilidade.Codigo == "")
            {
                throw new Exception("O código da habilidade não pode ser nulo ou vazio.");
            }
            if(habilidade.Nome == "")
            {
                throw new Exception("O nome da habilidade não pode ser nulo ou vazio.");
            }

            _repository.Adicionar(habilidade);
            return lerDto(habilidade);
        }

        public ListarHabilidadeDto Atualizar(int id, AdicionarHabilidadeDto habilidadeDto)
        {
            var habilidade = _repository.BuscarPorId(id);
            if (habilidade == null)
            {
                throw new Exception("Habilidade não encontrada.");
            }
            if (habilidadeDto.Nome == null || habilidadeDto.Nome.Trim() == "")
            {
                throw new Exception("O nome da habilidade não pode ser nulo ou vazio.");
            }
            if (habilidadeDto.Codigo == null || habilidadeDto.Codigo.Trim() == "")
            {
                throw new Exception("O código da habilidade não pode ser nulo ou vazio.");
            }
            if (id < 0)
            {
                throw new Exception("O ID da habilidade deve ser um valor positivo.");
            }

            Habilidade habilidadeAtualizada = new Habilidade
            {
                HabilidadeId = id,
                Codigo = habilidadeDto.Codigo.Trim().ToUpperInvariant(),
                Nome = habilidadeDto.Nome.Trim()
            };

            _repository.Atualizar(id, habilidadeAtualizada);
            return lerDto(habilidadeAtualizada);
        }

        public void Deletar(int id)
        {
            var habilidade = _repository.BuscarPorId(id);
            if (habilidade == null)
            {
                throw new Exception("Habilidade não encontrada.");
            }
            _repository.Deletar(id);
        }
    }
}
