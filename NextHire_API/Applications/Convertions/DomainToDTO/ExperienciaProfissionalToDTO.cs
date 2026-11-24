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
                Cargo = experienciaProfissional.Cargo,
                NomeEmpresa = experienciaProfissional.NomeEmpresa,
                DataEntrada = experienciaProfissional.DataEntrada,
                DataSaida = experienciaProfissional.DataSaida,
                StatusExperiencia = experienciaProfissional.AtualmenteTrabalho ?? true ? "Trabalha atualmente aqui" : "Experiência anterior",
                UsuarioId = experienciaProfissional.UsuarioId
            };
        }

        public static ExperienciaProfissional ConvertarParaAdicionar(AdicionarExperienciaProfissionalDto experienciaProfissionalDto)
        {
            return new ExperienciaProfissional
            {
                DataEntrada = experienciaProfissionalDto.DataEntrada,
                UsuarioId = experienciaProfissionalDto.UsuarioId,
                Cargo = experienciaProfissionalDto.Cargo,
                NomeEmpresa = experienciaProfissionalDto.NomeEmpresa,
                DataSaida = experienciaProfissionalDto.DataSaida,
                AtualmenteTrabalho = experienciaProfissionalDto.StatusExperiencia ?? false,
            };
        }

        public static ExperienciaProfissional ConverderParaAtualizar(AtualizarExperienciaProfissionalDto experienciaProfissional, int experienciaId, int usuarioId)
        {
            return new ExperienciaProfissional
            {
                ExperienciaProfissionalId = experienciaId,
                UsuarioId = usuarioId,
                Cargo = experienciaProfissional.Cargo,
                NomeEmpresa = experienciaProfissional.NomeEmpresa,
                DataEntrada = experienciaProfissional.DataEntrada,
                DataSaida = experienciaProfissional.DataSaida,
                AtualmenteTrabalho = experienciaProfissional.AtualmenteTrabalha
            };
        }
    }
}
