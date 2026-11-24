using NextHire_API.Applications.Convertions.DomainToDTO;
using NextHire_API.Domains;
using NextHire_API.DTOs.ComentarioDTO;
using NextHire_API.Exceptions;

namespace NextHire_API.Applications.Services
{
    public class ComentarioService
    {
        private readonly IComentarioRepository _repository;

        public ComentarioService(IComentarioRepository comentarioRepository)
        {
            _repository = comentarioRepository;
        }

        public async Task<List<ListarComentarioDto>> Listar()
        {
            List<Comentario> comentarios = await _repository.Listar() 
                ?? throw new DomainException("Nenhum comentário encontrado.");
            return comentarios.Select(c => ComentarioToDTO.ListarComentarioDto(c)).ToList();
        }

        public async Task<List<ListarComentarioDto>> ListarPorEntrevistaId(int entrevistaId)
        {
            List<Comentario> comentarios = await _repository.ListarPorEntrevistaId(entrevistaId);
            return comentarios.Select(c => ComentarioToDTO.ListarComentarioDto(c)).ToList();
        }

        public async Task<List<ListarComentarioDto>> ListarPorGestorId(int gestorId)
        {
            List<Comentario> comentarios = await _repository.ListarPorGestorId(gestorId)
                ?? throw new DomainException("Nenhum comentário encontrado para o gestor informado.");
            return comentarios.Select(c => ComentarioToDTO.ListarComentarioDto(c)).ToList();
        }
        
        public async Task<List<ListarComentarioDto>> ListarPorEntrevista(int entrevistaId)
        {
            List<Comentario> comentarios = await _repository.ListarPorEntrevistaId(entrevistaId)
                ?? throw new DomainException("Nenhum comentário encontrado para a entrevista informada.");

            return comentarios.Select(c => ComentarioToDTO.ListarComentarioDto(c)).ToList();
        }

        public async Task<List<ListarComentarioDto>> ListarPorGestor(int gestorId)
        {
            List<Comentario> comentarios = await _repository.ListarPorGestorId(gestorId)
                ?? throw new DomainException("Nenhum comentário encontrado para o gestor informado.");

            return comentarios.Select(c => ComentarioToDTO.ListarComentarioDto(c)).ToList();
        }

        public async Task<ListarComentarioDto> BuscarPorId(int id)
            => ComentarioToDTO.ListarComentarioDto(await _repository.BuscarPorId(id)
                ?? throw new DomainException("Comentário não encontrado."));

        public async Task Adicionar(AdicionarComentarioDto comentarioDto)
        {
            if(comentarioDto.EntrevistaId <= 0 || comentarioDto.GestorId <= 0 || string.IsNullOrEmpty(comentarioDto.Conteudo))
                throw new DomainException("Todos os campos são obrigatórios.");

            await _repository.Adicionar(ComentarioToDTO.AdicionarComentarioToDomain(comentarioDto));
        }

        public async Task Atualizar(AtualizarComentarioDto comentarioDto, int comentarioId)
        {
            Comentario comentarioAtualizado = await _repository.BuscarPorId(comentarioId)
                ?? throw new DomainException("Comentário não encontrado.");

            await _repository.Atualizar(comentarioId, ComentarioToDTO.AtualizarComentarioToDomain(comentarioDto));
        }

        public async Task Remover(int comentarioId)
        {
            Comentario comentarioAtualizado = await _repository.BuscarPorId(comentarioId)
                ?? throw new DomainException("Comentário não encontrado.");

            await _repository.Remover(comentarioId);
        }
    }
}