using NextHire_API.Domains;
using NextHire_API.DTOs.ExperienciaProfissionalDTO;

namespace NextHireAPI.Applications.Convertions.DomainToDTO
{
    public static class ExperienciaProfissionalToDTO
    {
        public static ListarExperienciaProfissionalDto ConvertarParaListar(ExperienciaProfissional experienciaProfissional)
        {
            return new ListarExperienciaProfissionalDto
            {
                ExperienciaProfissionalId = experienciaProfissional.ExperienciaProfissionalId,
                DataEntrada = experienciaProfissional.DataEntrada,
                DataSaida = experienciaProfissional.DataSaida,
                StatusExperiencia = experienciaProfissional.AtualmenteTrabalho ?? true ? "Trabalha atualmente aqui" : "Experiência anterior",
                UsuarioId = experienciaProfissional.UsuarioId
            };
        }

        public static AtualizarExperienciaProfissionalDTO ConverderParaAtualizar(ExperienciaProfissional experienciaProfissional)
        {
            return new AtualizarExperienciaProfissionalDTO
            {
                DataEntrada = experienciaProfissional.DataEntrada,
                DataSaida = experienciaProfissional.DataSaida,
                AtualmenteTrabalha = experienciaProfissional.AtualmenteTrabalho,
            };
        }
    }
}
