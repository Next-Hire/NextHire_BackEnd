using NextHire_API.Domains;

namespace NextHire_API.Interfaces;

public interface ICurriculoCandidatoRepository
{
    public Task <List<CurriculoCandidato?>> ListarCurriculos();

    public Task<CurriculoCandidato?> BuscarCurriculoPorId(int curriculoCandidatoId);
    public Task<CurriculoCandidato?> BuscarCurriculoPorCandidatoId(int candidatoId);
    
    public Task<CurriculoCandidato> AdicionarCurriculo(CurriculoCandidato curriculoCandidato);
    public Task<CurriculoCandidato> RemoverCurriculo();
}