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
            var item = _repository.BuscarPorId(id);

            if (item == null)
            {
                throw new DomainException("Item não encontrado.");
            }

            return lerDto(item);
        }

        public ListarAreaOcupacaoDto ListarPorNome(string nome)
        {
            var item = _repository.BuscarPorNome(nome);
            if (item == null)
            {
                throw new DomainException("Item não encontrado.");
            }
            return lerDto(item);
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
                throw new DomainException("Item não encontrado.");
            }

            areaExistente.nome = areaOcupacao.nome;
            areaExistente.setor_ocupacao = areaOcupacao.setor_ocupacao;
            areaExistente.usuario = areaOcupacao.usuario;

            _repository.Atualizar(areaExistente);
            return lerDto(areaExistente);
        }
    }
}
