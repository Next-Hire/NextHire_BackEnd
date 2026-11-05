using NextHire_API.Domains;
using NextHire_API.DTOs.AreaOcupacaoDto;
using NextHire_API.Exceptions;
using NextHire_API.Interfaces;

namespace NextHire_API.Applications.Services
{
    public class AreaOcupacaoService
    {
        private readonly IAreaOcupacaoRepository _repository;

        public AreaOcupacaoService(IAreaOcupacaoRepository areaOcupacaoRepository)
        {
            _repository = areaOcupacaoRepository;
        }

        private static ListarAreaOcupacaoDto lerDto(AreaOcupacao areaOcupacao)
        {
            return new ListarAreaOcupacaoDto
            {
                AreaOcupacaoId = areaOcupacao.AreaOcupacaoId,
                Nome = areaOcupacao.Nome,
                SetorOcupacao = areaOcupacao.SetorOcupacao,
                Usuario = areaOcupacao.Usuario
            };
        }

        public List<ListarAreaOcupacaoDto> Listar()
        {
            return _repository.Listar().Select(lerDto).ToList();
        }

        public ListarAreaOcupacaoDto ListarPorID(int id)
        {
            var area = _repository.BuscarPorId(id);

            if (area == null)
            {
                throw new DomainException("Area de Ocupação não encontrado.");
            }

            return lerDto(area);
        }

        public ListarAreaOcupacaoDto ListarPorNome(string nome)
        {
            var area = _repository.BuscarPorNome(nome);
            if (area == null)
            {
                throw new DomainException("Area de Ocupação não encontrado.");
            }
            return lerDto(area);
        }

        public ListarAreaOcupacaoDto Adicionar(AdicionarAreaOcupacao areaOcupacao)
        {
            var setor = _repository.BuscarSetorPorId(areaOcupacao.SetorId);
            var usuario = _repository.BuscarUsuarioPorId(areaOcupacao.UsuarioId);

            if (setor == null || usuario == null)
            {
                throw new DomainException("Setor ou usuário não encontrado.");
            }

            var area = new AreaOcupacao
            {
                Nome = areaOcupacao.Nome,
                SetorOcupacao = new List<SetorOcupacao> { setor },
                Usuario = new List<Usuario> { usuario }
            };

            _repository.Adicionar(area);
            return lerDto(area);
        }

        public ListarAreaOcupacaoDto Atualizar(int id, AdicionarAreaOcupacao areaOcupacao)
        {
            var areaExistente = _repository.BuscarPorId(id);

            if (areaExistente == null)
            {
                throw new DomainException("Area de Ocupação não encontrado.");
            }

            areaExistente.Nome = areaOcupacao.Nome;

            var setor = _repository.BuscarSetorPorId(areaOcupacao.SetorId);
            var usuario = _repository.BuscarUsuarioPorId(areaOcupacao.UsuarioId);

            if (setor == null || usuario == null)
            {
                throw new DomainException("Setor ou usuário não encontrado.");
            }

            areaExistente.SetorOcupacao = new List<SetorOcupacao> { setor };
            areaExistente.Usuario = new List<Usuario> { usuario };

            _repository.Atualizar(areaExistente);
            return lerDto(areaExistente);
        }

        public void Deletar(int id)
        {
            AreaOcupacao area = _repository.BuscarPorId(id);

            if (area == null)
            {
                throw new DomainException("Area de Ocupação não encontrado.");
            }

            _repository.Deletar(id);
        }
    }
}
