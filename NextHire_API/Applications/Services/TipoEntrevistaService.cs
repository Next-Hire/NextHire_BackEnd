using NextHire_API.Domains;
using NextHire_API.DTOs.TipoEntrevistaDTO;
using NextHire_API.Interfaces;
using NextHire_API.Exceptions;

namespace NextHire_API.Applications.Services
{


    public class TipoEntrevistaService
    {
        private static ListarTipoEntrevista LerDTO(TipoEntrevista tipoEntr)
    => new ListarTipoEntrevista
    {
        tipoEntrevistaId = tipoEntr.TipoEntrevistaId,
        nome = tipoEntr.Nome_Tipo,
        descricao = tipoEntr.Descricao
    };


        private readonly ITipoEntrevistaRepository _rep;

        public TipoEntrevistaService(ITipoEntrevistaRepository rep)
         =>   _rep = rep;
    
        
        public async Task<List<ListarTipoEntrevista>> Listar()
        {
            List < TipoEntrevista > listagemQuery = await _rep.Listar();
            if (listagemQuery == null || listagemQuery.Count == 0)
                throw new DomainException("TipoEntrevista não encontrado");

            return listagemQuery.Select(varAUx => LerDTO(varAUx)).ToList();

        }

        public async Task<ListarTipoEntrevista?> BuscarPorTipo(string? nome)
            => LerDTO(await _rep.BuscarPorTipo(nome)) 
                ?? throw new DomainException("Tipo não encontrado");

        public async Task<ListarTipoEntrevista?> BuscarPorTipo(int id)
    => LerDTO(await _rep.BuscarPorId(id))
        ?? throw new DomainException("Tipo não encontrado");





    }
}
