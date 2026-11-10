using NextHire_API.Domains;
using NextHire_API.DTOs.AnotacaoDto;
using NextHire_API.Exceptions;
using NextHire_API.Interfaces;

namespace NextHire_API.Applications.Services
{
    public class AnotacaoService
    {
        private readonly IAnotacaoRepository _repository;

        public AnotacaoService(IAnotacaoRepository anotacaoRepository)
        {
            _repository = anotacaoRepository;
        }

        private static ListarAnotacaoDto lerDto(Anotacao anotacao)
        {
            return new ListarAnotacaoDto
            {
                AnotacaoId = anotacao.AnotacaoId,
                CandidatoId = anotacao.CandidatoId,
                Comentario = anotacao.Comentario,
                GestorId = anotacao.GestorId
            };
        }  

        public List<ListarAnotacaoDto> Listar()
        {
            return _repository.Listar().Select(lerDto).ToList();
        }

        public ListarAnotacaoDto BuscarPorId(int id)
        {
            Anotacao anotacao = _repository.BuscarPorId(id);
            if (anotacao == null)
            {
                throw new Exception("Anotação não encontrada.");
            }
            return lerDto(anotacao);
        }

        public ListarAnotacaoDto Adicionar(AdicionarAnotacaoDto anotacaoDto)
        {
            Anotacao anotacao = new Anotacao
            {
                CandidatoId = anotacaoDto.CandidatoId,
                Comentario = anotacaoDto.Comentario,
                GestorId = anotacaoDto.GestorId
            };
            _repository.Adicionar(anotacao);
            return lerDto(anotacao);
        }

        public ListarAnotacaoDto Atualizar(AdicionarAnotacaoDto anotacaoDto, int id)
        {
            var anotacaoExiste = _repository.BuscarPorId(id);

            if (anotacaoExiste == null)
            {
                throw new DomainException("Anotação não encontrada.");
            }

            anotacaoExiste.CandidatoId = anotacaoDto.CandidatoId;
            anotacaoExiste.Comentario = anotacaoDto.Comentario;
            anotacaoExiste.GestorId = anotacaoDto.GestorId;

            _repository.Atualizar(anotacaoExiste);
            return lerDto(anotacaoExiste);
        }

        public void Deletar(int id)
        {
            var anotacaoExiste = _repository.BuscarPorId(id);
            if (anotacaoExiste == null)
            {
                throw new DomainException("Anotação não encontrada.");
            }
            _repository.Deletar(id);
        }
    }
}
