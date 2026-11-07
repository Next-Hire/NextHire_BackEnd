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
        private readonly ISetor_OcupacaoRepository _repSetorOcupacao;
        private readonly IAreaOcupacaoRepository _repArea;
        private readonly IEmpresaRepository _repEmpresa;
        public Setor_OcupacaoService(ISetor_OcupacaoRepository rep, IAreaOcupacaoRepository repArea, IEmpresaRepository repEmpresa)
        {
            _repSetorOcupacao = rep;
            _repArea = repArea;
            _repEmpresa = repEmpresa;
        }


        public async Task<IEnumerable<LerSetor_OcupacaoDTO2>> Listar()
        {
            IEnumerable<SetorOcupacao?> setores = await _repSetorOcupacao.ListagemTotal();
            if(setores == null || !setores.Any())
                throw new DomainException("Nenhum setor de ocupação encontrado");

            return setores.Select(varAux => Setor_OcupacaoConvertion.lerSetoresOcupacoes(varAux)).ToList();
        }


        public async Task<LerSetor_OcupacaoDTO2> ObterSetorPorId(int setorIrd)
        {
            SetorOcupacao? setoresQuery = await _repSetorOcupacao.ObterSetorPorId(setorIrd);
            if(setoresQuery == null)
                throw new DomainException("Nenhum setor de ocupação encontrado");

            return Setor_OcupacaoConvertion.lerSetoresOcupacoes(setoresQuery);
        }

        
        public async Task<LerSetor_OcupacaoDTO2> ObterSetorNome(string setorNome)
        {
            SetorOcupacao? setoresQuery = await _repSetorOcupacao.ObterSetorPorNome(setorNome);
            if (setoresQuery == null)
                throw new DomainException("Nenhum setor de ocupação encontrado");

            return Setor_OcupacaoConvertion.lerSetoresOcupacoes(setoresQuery);
        }


        public async Task<LerSetor_OcupacaoDTO2> ObterSetorPorArea(int areaId, string? nomeArea)
        {
            AreaOcupacao? areaQuery = _repArea.BuscarPorId(areaId) ?? _repArea.BuscarPorNome(nomeArea);
            if (areaQuery == null)
                throw new DomainException("Área não encontrada.");

            SetorOcupacao setorQuery = await _repSetorOcupacao.ObterSetorPorArea(areaQuery.AreaOcupacaoId, areaQuery.Nome);
                if(setorQuery == null)
                    throw new DomainException("Nenhum setor de ocupação encontrado para a área especificada.");

            return Setor_OcupacaoConvertion.lerSetoresOcupacoes(setorQuery);
        }

        public async Task<IEnumerable<LerSetor_OcupacaoDTO2?>> ObterListagemSetoresPorEmpresaEPorArea(int empresaId, string? empresaNome, string? nomeArea, int areaId)
        {
            AreaOcupacao? areaQuery = _repArea.BuscarPorId(areaId) ?? _repArea.BuscarPorNome(nomeArea);
            Empresa? empresaQuery = await _repEmpresa.BuscarPorId(empresaId) ?? await _repEmpresa.BuscarPorNome(empresaNome);
            if (areaQuery == null || empresaQuery == null)
                throw new DomainException("Empresa e/ou Área não encontrada.");

            List<SetorOcupacao?> setorQuery = await _repSetorOcupacao.ObterListagemSetoresPorEmpresaEPorArea(empresaQuery.EmpresaId, empresaQuery.Nome, areaQuery.Nome, areaQuery.AreaOcupacaoId);
            if (setorQuery == null)
                throw new DomainException("Setor não encontrado");

            return setorQuery.Select(varAux => Setor_OcupacaoConvertion.lerSetoresOcupacoes(varAux)).ToList();
        }

        public async Task<IEnumerable<LerSetor_OcupacaoDTO2>> ObterSetoresPorEmpresa(int? empresaId, string? nomeEmpresa)
        {
            Empresa? empresaQuery = await _repEmpresa.BuscarPorId(empresaId) ?? await _repEmpresa.BuscarPorNome(nomeEmpresa);
            if(empresaQuery == null || empresaQuery.EmpresaId == 0)
                throw new DomainException("Empresa não encontrada.");

            IEnumerable<SetorOcupacao?> setorOcupacaoQuery = await _repSetorOcupacao.ObterSetoresPorEmpresa(empresaQuery.EmpresaId, empresaQuery.Nome);
            return setorOcupacaoQuery.Select(varAux => Setor_OcupacaoConvertion.lerSetoresOcupacoes(varAux)).ToList();
        }
        

        public async Task<LerSetor_OcupacaoDTO2> RemoverSetor(int id)


    }
}
