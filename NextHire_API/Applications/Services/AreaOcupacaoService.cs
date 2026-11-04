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

        private static ListarAreaOcupacaoDto lerDto(area_ocupacao areaOcupacao)
        {
            return new ListarAreaOcupacaoDto
            {
                area_ocupacao_id = areaOcupacao.area_ocupacao_id,
                nome = areaOcupacao.nome,
                setor_ocupacao = areaOcupacao.setor_ocupacao,
                usuario = areaOcupacao.usuario
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
            var area = new area_ocupacao
            {
                nome = areaOcupacao.nome,
                setor_ocupacao = areaOcupacao.setor_ocupacao,
                usuario = areaOcupacao.usuario
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

            areaExistente.nome = areaOcupacao.nome;
            areaExistente.setor_ocupacao = areaOcupacao.setor_ocupacao;
            areaExistente.usuario = areaOcupacao.usuario;

            _repository.Atualizar(areaExistente);
            return lerDto(areaExistente);
        }

        public void Deletar(int id)
        {
            area_ocupacao area = _repository.BuscarPorId(id);

            if (area == null)
            {
                throw new DomainException("Area de Ocupação não encontrado.");
            }

            _repository.Deletar(id);
        }
    }
}
