using NextHire_API.Domains;
using NextHire_API.DTOs.HabilidadeDto;
using NextHire_API.Interfaces;

namespace NextHire_API.Applications.Services
{
    public class HabilidadesService 
    {
        private readonly IHabilidadesRepository _repository; 

        public HabilidadesService(IHabilidadesRepository repository)
        {
            _repository = repository;
        }

        private static ListarHabilidadeDto lerDto(Habilidade habilidade)
        {
            return new ListarHabilidadeDto
            {
                HabilidadeId = habilidade.HabilidadeId,
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
                Nome = habilidadeDto.Nome
            };
            if(habilidade.Nome == null || habilidade.Nome.Trim() == "")
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
            if (habilidade.Nome == null || habilidade.Nome.Trim() == "")
            {
                throw new Exception("O nome da habilidade não pode ser nulo ou vazio.");
            }
            if (id < 0)
            {
                throw new Exception("O ID da habilidade deve ser um valor positivo.");
            }

            Habilidade habilidadeAtualizada = new Habilidade
            {
                HabilidadeId = id,
                Nome = habilidadeDto.Nome
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
