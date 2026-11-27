using Microsoft.Extensions.Diagnostics.HealthChecks;
using Npgsql;

namespace NextHire_API.Health
{
    public class DatabaseHealthCheck : IHealthCheck
    {
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                string connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING")
                    ?? throw new InvalidOperationException("CONNECTION_STRING não configurada.");

                await using var conexao = new NpgsqlConnection(connectionString);
                await conexao.OpenAsync(cancellationToken);
                await using var cmd = new NpgsqlCommand("SELECT 1", conexao);
                await cmd.ExecuteScalarAsync(cancellationToken);

                return HealthCheckResult.Healthy("Banco de dados acessível.");
            }
            catch (Exception ex)
            {
                return HealthCheckResult.Unhealthy("Banco de dados inacessível.", ex);
            }
        }
    }
}
