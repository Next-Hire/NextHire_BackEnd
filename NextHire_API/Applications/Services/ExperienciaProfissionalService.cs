using NextHire_API.Domains;
using NextHire_API.DTOs.ExperienciaProfissionalDTO;
using NextHire_API.Exceptions;
using NextHire_API.Interfaces;
using NextHireAPI.Applications.Convertions.DomainToDTO;

namespace NextHire_API.Applications.Services
{
    public class ExperienciaProfissionalService
    {
        private readonly IExperienciaProfissionalRepository _repository;
        public ExperienciaProfissionalService(IExperienciaProfissionalRepository repository) => _repository = repository;

        public async Task<List<ListarExperienciaProfissionalDto>> ListarExperienciaProfissionalPorUsuario(int usuarioId, bool? ativo)
        {
            if (usuarioId <= 0)
                throw new DomainException("O Id do usuário deve ser maior que zero.");
            List<ExperienciaProfissional> experienciasDto = await _repository.ListarExperienciasProfissionaisPorUsuario(usuarioId, ativo);

            return experienciasDto.Select(x => ExperienciaProfissionalToDTO.ConvertarParaListar(x)).ToList();
        }

        public async Task<ListarExperienciaProfissionalDto> BuscarExperienciaProfissional(int experienciaProfissionalId)
        {
            if (experienciaProfissionalId <= 0)
                throw new DomainException("O Id da experiência profissional deve ser maior que zero.");

            var experiencia = await _repository.BuscarExperienciaProfissionalPorId(experienciaProfissionalId);
            if (experiencia == null)
                throw new DomainException("Experiência profissional não encontrada.");

            return ExperienciaProfissionalToDTO.ConvertarParaListar(experiencia);
        }

        public async Task CadastrarExperienciaProfissional(AdicionarExperienciaProfissionalDto experienciaProfissionalDto)
        {
            if (experienciaProfissionalDto == null || experienciaProfissionalDto.UsuarioId <= 0 || experienciaProfissionalDto.DataEntrada > DateOnly.FromDateTime(DateTime.Now) || String.IsNullOrWhiteSpace(experienciaProfissionalDto.Cargo) || String.IsNullOrWhiteSpace(experienciaProfissionalDto.NomeEmpresa))
                throw new DomainException("O objeto de experiência profissional não pode ser nulo.");

            var experiencia = ExperienciaProfissionalToDTO.ConvertarParaAdicionar(experienciaProfissionalDto);
            await _repository.CadastrarExperienciaProfissional(experiencia);
        }

        public async Task AtualizarExperienciaProfissional(int experienciaProfissionalId, AtualizarExperienciaProfissionalDto experienciaProfissionalAtualizada)
        {
            if (experienciaProfissionalId <= 0)
                throw new DomainException("O Id da experiência profissional deve ser maior que zero.");

            ExperienciaProfissional experienciaBuscada = await _repository.BuscarExperienciaProfissionalPorId(experienciaProfissionalId);
            if (experienciaBuscada == null)
                throw new DomainException("O objeto de experiência profissional não pode ser nulo.");

            var experiencia = ExperienciaProfissionalToDTO.ConverderParaAtualizar(experienciaProfissionalAtualizada, experienciaProfissionalId, experienciaBuscada.UsuarioId);
            await _repository.AtualizarExperienciaProfissional(experienciaProfissionalId, experiencia);
        }

        public async Task RemoverExperienciaProfissional(int experienciaProfissionalId)
        {
            if (experienciaProfissionalId <= 0)
                throw new DomainException("O Id da experiencia deve ser maior que zero");

            await _repository.RemoverExperienciaProfissional(experienciaProfissionalId);
        }
    }
}
