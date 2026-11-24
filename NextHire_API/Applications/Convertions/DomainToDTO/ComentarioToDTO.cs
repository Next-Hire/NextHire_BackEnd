using NextHire_API.Domains;
using NextHire_API.DTOs.ComentarioDTO;

namespace NextHire_API.Applications.Convertions.DomainToDTO
{
    public static class ComentarioToDTO
    {
        public static ListarComentarioDto ListarComentarioDto(Comentario comentario)
        {
            return new ListarComentarioDto
            {
                ComentarioId = comentario.ComentarioId,
                EntrevistaId = comentario.EntrevistaId,
                NomeGestor = comentario.Gestor.Nome,
                GestorId = comentario.GestorId,
                Conteudo = comentario.Texto,
                DataComentario = (DateTime)comentario.DataComentario == null ? DateTime.Now : (DateTime)comentario.DataComentario
            };
        }

        public static Comentario AtualizarComentarioToDomain(AtualizarComentarioDto comentario)
        {
            return new Comentario
            {
                Texto = comentario.Conteudo
            };
        }

        public static Comentario AdicionarComentarioToDomain(AdicionarComentarioDto comentario)
        {
            return new Comentario
            {
                EntrevistaId = comentario.EntrevistaId,
                GestorId = comentario.GestorId,
                Texto = comentario.Conteudo
            };
        } 
    }
}   