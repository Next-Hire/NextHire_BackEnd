using NextHire_API.Applications.Convertions.DomainToConversion;
using NextHire_API.Domains;
using NextHire_API.DTOs.EmpresaDTOs;
using NextHire_API.Exceptions;
using NextHire_API.Interfaces;

namespace NextHire_API.Applications.Services
{
    public class EmpresaService
    {
        private readonly IEmpresaRepository _rep;

        public EmpresaService(IEmpresaRepository rep) => _rep = rep;

        public async Task<LerEmpresaDTO> ObterPorId(int EmpresaId)
        {
            Empresa EmpresaQuery = await _rep.BuscarPorId(EmpresaId);
                    if(EmpresaQuery == null || EmpresaQuery.EmpresaId == 0)
                     throw new DomainException("Nenhuma Empresa cadastrada!");
            return EmpresaConvertion.listarEmpresas(EmpresaQuery);
        }

        public async Task<IEnumerable<LerEmpresaDTO>> ListagemTotal()
        {
            IEnumerable<Empresa?> EmpresasQuery = await _rep.Listar();
            if(EmpresasQuery == null || !EmpresasQuery.Any())
                throw new DomainException("Nenhuma Empresa cadastrada!");
            return EmpresasQuery.Select(EmpresaConvertion.listarEmpresas);
        }

        public async Task<LerEmpresaDTO> ObterPorEmpresa(string nome)
        {
            Empresa? EmpresaQuery = await _rep.BuscarPorNome(nome);
            if (EmpresaQuery == null || EmpresaQuery.EmpresaId == 0)
                throw new DomainException("Nenhuma Empresa cadastrada!");
            return EmpresaConvertion.listarEmpresas(EmpresaQuery);
        }

        public async Task<CriarEmpresaDTO> Adicionar(CriarEmpresaDTO EmpresaCons)
        {
            if (await _rep.EmpresaExiste(EmpresaCons.nome))
                throw new DomainException("Empresa já cadastrada!");

            Empresa Empresa = new Empresa
            {
                Nome = EmpresaCons.nome,
                //StatusEmpresa = true
            };

            await _rep.Adicionar(Empresa);
            return EmpresaConvertion.criarEmpresaDTO(Empresa);
        }

        public async Task<AtualizarEmpresaDTO> Atualizar(int EmpresaId, string? nome, bool? status)
        {
            Empresa? EmpresaQuery = await _rep.BuscarPorId(EmpresaId)
                ?? throw new DomainException("Empresa não encontrada!");

            EmpresaQuery.Nome = nome;
            EmpresaQuery.StatusEmpresa = status;


            await _rep.Atualizar(EmpresaQuery.EmpresaId, EmpresaQuery);
            return EmpresaConvertion.atualizarEmpresaDTO(EmpresaQuery);
        }

        public async Task<LerEmpresaDTO> Remover(int EmpresaId)
        {
            Empresa? EmpresaQuery = await _rep.BuscarPorId(EmpresaId)
                ?? throw new DomainException("Empresa não encontrada!");

            EmpresaQuery.StatusEmpresa = false;


            await _rep.Remover(EmpresaQuery.EmpresaId);
            return EmpresaConvertion.listarEmpresas(EmpresaQuery);
        }

    }
}
