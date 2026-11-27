using System.Text.Json;
using Npgsql;

namespace NextHire_API.Applications.Email
{
    public class EmailOutboxItem
    {
        public int Id { get; set; }
        public string Destinatario { get; set; } = null!;
        public string? NomeDestinatario { get; set; }
        public string Assunto { get; set; } = null!;
        public string Template { get; set; } = null!;
        public string DadosJson { get; set; } = "{}";
        public string DedupeKey { get; set; } = null!;
        public int Tentativas { get; set; }

        public Dictionary<string, string> Dados() =>
            JsonSerializer.Deserialize<Dictionary<string, string>>(DadosJson) ?? new();
    }

    public class EmailOutboxStore
    {
        private readonly string _connectionString;

        public EmailOutboxStore()
        {
            _connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING")
                ?? throw new InvalidOperationException("CONNECTION_STRING não configurada.");
        }

        private async Task<NpgsqlConnection> AbrirAsync(CancellationToken ct)
        {
            var conexao = new NpgsqlConnection(_connectionString);
            await conexao.OpenAsync(ct);
            return conexao;
        }

        public async Task<int?> EnfileirarAsync(EmailMessage mensagem, string dedupeKey, CancellationToken ct = default)
        {
            const string sql = """
                INSERT INTO email_outbox
                    (destinatario, nome_destinatario, assunto, template, dados_json, dedupe_key, status, tentativas)
                VALUES
                    (@destinatario, @nome, @assunto, @template, @dados::jsonb, @dedupe, 'pendente', 0)
                ON CONFLICT (dedupe_key) DO NOTHING
                RETURNING email_outbox_id;
                """;

            await using var conexao = await AbrirAsync(ct);
            await using var cmd = new NpgsqlCommand(sql, conexao);
            cmd.Parameters.AddWithValue("destinatario", mensagem.Destinatario);
            cmd.Parameters.AddWithValue("nome", (object?)mensagem.NomeDestinatario ?? DBNull.Value);
            cmd.Parameters.AddWithValue("assunto", mensagem.Assunto);
            cmd.Parameters.AddWithValue("template", mensagem.Template);
            cmd.Parameters.AddWithValue("dados", JsonSerializer.Serialize(mensagem.Dados));
            cmd.Parameters.AddWithValue("dedupe", dedupeKey);

            object? resultado = await cmd.ExecuteScalarAsync(ct);
            return resultado is int id ? id : null;
        }

        public async Task<List<EmailOutboxItem>> PendentesAsync(int limite, CancellationToken ct = default)
        {
            const string sql = """
                SELECT email_outbox_id, destinatario, nome_destinatario, assunto, template,
                       dados_json, dedupe_key, tentativas
                FROM email_outbox
                WHERE status = 'pendente'
                  AND tentativas < 5
                  AND (proxima_tentativa_em IS NULL OR proxima_tentativa_em <= CURRENT_TIMESTAMP)
                ORDER BY email_outbox_id
                LIMIT @limite;
                """;

            var itens = new List<EmailOutboxItem>();
            await using var conexao = await AbrirAsync(ct);
            await using var cmd = new NpgsqlCommand(sql, conexao);
            cmd.Parameters.AddWithValue("limite", limite);

            await using var reader = await cmd.ExecuteReaderAsync(ct);
            while (await reader.ReadAsync(ct))
            {
                itens.Add(new EmailOutboxItem
                {
                    Id = reader.GetInt32(0),
                    Destinatario = reader.GetString(1),
                    NomeDestinatario = reader.IsDBNull(2) ? null : reader.GetString(2),
                    Assunto = reader.GetString(3),
                    Template = reader.GetString(4),
                    DadosJson = reader.GetString(5),
                    DedupeKey = reader.GetString(6),
                    Tentativas = reader.GetInt32(7)
                });
            }

            return itens;
        }

        public async Task MarcarEnviadoAsync(int id, CancellationToken ct = default)
        {
            const string sql = """
                UPDATE email_outbox
                SET status = 'enviado', enviado_em = CURRENT_TIMESTAMP, erro = NULL
                WHERE email_outbox_id = @id;
                """;

            await using var conexao = await AbrirAsync(ct);
            await using var cmd = new NpgsqlCommand(sql, conexao);
            cmd.Parameters.AddWithValue("id", id);
            await cmd.ExecuteNonQueryAsync(ct);
        }

        public async Task MarcarFalhaAsync(int id, int tentativas, string erro, int intervaloMinutos, CancellationToken ct = default)
        {
            const string sql = """
                UPDATE email_outbox
                SET tentativas = @tentativas,
                    erro = @erro,
                    status = CASE WHEN @tentativas >= 5 THEN 'falhou' ELSE 'pendente' END,
                    proxima_tentativa_em = CURRENT_TIMESTAMP + (@intervalo || ' minutes')::interval
                WHERE email_outbox_id = @id;
                """;

            await using var conexao = await AbrirAsync(ct);
            await using var cmd = new NpgsqlCommand(sql, conexao);
            cmd.Parameters.AddWithValue("id", id);
            cmd.Parameters.AddWithValue("tentativas", tentativas);
            cmd.Parameters.AddWithValue("erro", erro);
            cmd.Parameters.AddWithValue("intervalo", intervaloMinutos);
            await cmd.ExecuteNonQueryAsync(ct);
        }

        public async Task<bool> PreferenciaHabilitadaAsync(int usuarioId, string tipo, CancellationToken ct = default)
        {
            const string sql = """
                SELECT habilitado FROM preferencia_notificacao
                WHERE usuario_id = @usuario AND tipo = @tipo
                LIMIT 1;
                """;

            await using var conexao = await AbrirAsync(ct);
            await using var cmd = new NpgsqlCommand(sql, conexao);
            cmd.Parameters.AddWithValue("usuario", usuarioId);
            cmd.Parameters.AddWithValue("tipo", tipo);

            object? valor = await cmd.ExecuteScalarAsync(ct);
            return valor is null || valor is DBNull || (bool)valor;
        }
    }
}
