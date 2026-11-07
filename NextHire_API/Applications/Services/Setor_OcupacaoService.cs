using NextHire_API.Applications.Convertions.DomainToConversion;
using NextHire_API.Domains;
using NextHire_API.DTOs.EmpresaDTOs;
using NextHire_API.DTOs.Setor_OcupacaoDTOs;
using NextHire_API.Exceptions;
using NextHire_API.Interfaces;

namespace NextHire_API.Applications.Services
{
    public class Setor_OcupacaoService
    {
        private readonly ISetor_OcupacaoRepository _rep;

        public Setor_OcupacaoService(ISetor_OcupacaoRepository rep)
        {
            _rep = rep;
        }


        public async Task<IEnumerable<LerSetor_OcupacaoDTO2>> Listar()
        {
            IEnumerable<SetorOcupacao?> setores = await _rep.ListagemTotal();
            if(setores == null || !setores.Any())
                throw new DomainException("Nenhum setor de ocupação encontrado");

            var setoresJSON = setores.Select(varAux => Setor_OcupacaoConvertion.lerSetoresOcupacoes(varAux)).ToList();
            return setoresJSON;
        }


        public async Task<LerSetor_OcupacaoDTO2> ObterSetorPorId(int setorIrd)
        {
            SetorOcupacao? setoresQuery = await _rep.ObterSetorPorId(setorIrd);
            if(setoresQuery == null)
                throw new DomainException("Nenhum setor de ocupação encontrado");

            return Setor_OcupacaoConvertion.lerSetoresOcupacoes(setoresQuery);
        }

        
        public async Task<LerSetor_OcupacaoDTO2> ObterSetorNome(string setorNome)
        {
            SetorOcupacao? setoresQuery = await _rep.ObterSetorPorNome(setorNome);
            if (setoresQuery == null)
                throw new DomainException("Nenhum setor de ocupação encontrado");

            return Setor_OcupacaoConvertion.lerSetoresOcupacoes(setoresQuery);
        }


        public async Task<LerSetor_OcupacaoDTO2> ObterSetorPorArea(int? areaId, string? area)
        {

        }



    }
}
