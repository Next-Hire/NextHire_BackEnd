using NextHire_API.Domains;
using NextHire_API.DTOs.ExperienciaProfissionalDTO;

namespace NextHire_API.Applications.Convertions.DomainToDTO
{
    public static class ExperienciaProfissionalToDTO
    {
        public static ListarExperienciaProfissionalDto ConvertarParaListar(experiencia_profissional experienciaProfissional)
        {
            return new ListarExperienciaProfissionalDto
            {
                experiencia_profissional_id = experienciaProfissional.experiencia_profissional_id,
                data_entrada = experienciaProfissional.data_entrada,
                data_saida = experienciaProfissional.data_saida,
                status_experiencia = experienciaProfissional.atualmente_trabalho ?? true ? "Trabalha atualmente aqui" : "Experiência anterior",
                usuario_id = experienciaProfissional.usuario_id
            };
        }

        public static AtualizarExperienciaProfissionalDTO ConverderParaAtualizar(experiencia_profissional experienciaProfissional)
        {
            return new AtualizarExperienciaProfissionalDTO
            {
                data_entrada = experienciaProfissional.data_entrada,
                data_saida = experienciaProfissional.data_saida,
                atualmente_trabalho = experienciaProfissional.atualmente_trabalho,
            };
        }
    }
}
