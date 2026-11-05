using NextHire_API.Applications.Convertions.DomainToConversion;
using NextHire_API.Applications.Convertions.DomainToDTO;
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

        public async Task<LerEmpresaDTO> ObterPorId(int empresaId)
        {
            empresa empresaQuery = await _rep.BuscarPorId(empresaId)
                    ?? throw new DomainException("Empresa não encontrada!");
            return EmpresaConvertion.listarEmpresas(empresaQuery);
        }

        public async Task<IEnumerable<LerEmpresaDTO>> ListagemTotal()
        {
            IEnumerable<empresa?> empresasQuery = await _rep.Listar();
            if(empresasQuery == null || !empresasQuery.Any())
                throw new DomainException("Nenhuma empresa cadastrada!");
            return empresasQuery.Select(EmpresaConvertion.listarEmpresas);
        }

        public async Task<LerEmpresaDTO> ObterPorEmpresa(string nome)
        {
            empresa? empresaQuery = await _rep.BuscarPorNome(nome)
                    ?? throw new DomainException("Empresa Não encontrada!");
            return EmpresaConvertion.listarEmpresas(empresaQuery);
        }

        public async Task<CriarEmpresaDTO> Adicionar(CriarEmpresaDTO empresaCons)
        {
            if (await _rep.empresaExiste(empresaCons.nome))
                throw new DomainException("Empresa já cadastrada!");

            empresa empresa = new empresa
            {
                nome = empresaCons.nome,

            };

            await _rep.Adicionar(empresa);
            return EmpresaConvertion.criarEmpresaDTO(empresa);
        }

        public async Task<LerEmpresaDTO> Atualizar(int empresaId, LerEmpresaDTO empresaCons)
        {
            empresa? empresaQuery = await _rep.BuscarPorId(empresaId)
                ?? throw new DomainException("Empresa não encontrada!");

            empresaQuery.nome = empresaCons.nome;


            await _rep.Atualizar(empresaQuery.empresa_id, empresaQuery);
            return EmpresaConvertion.listarEmpresas(empresaQuery);
        }

        public async Task<LerEmpresaDTO> Remover(int empresaId)
        {
            empresa? empresaQuery = await _rep.BuscarPorId(empresaId)
                ?? throw new DomainException("Empresa não encontrada!");

            empresaQuery.status_empresa = false;


            await _rep.Remover(empresaQuery.empresa_id);
            return EmpresaConvertion.listarEmpresas(empresaQuery);
        }

    }
}
