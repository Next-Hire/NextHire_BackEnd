using Konscious.Security.Cryptography;
using System.Security.Cryptography;
using System.Text;

namespace NextHireAPI.Applications.Conversions
{
    /// <summary>
    /// Utilitário estático para hash e verificação de senhas com Argon2id.
    /// </summary>
    public static class PassToArgon2
    {
        // ─── Parâmetros do Argon2id ───────────────────────────────────────────
        // m = memória (KiB), t = iterações, p = paralelismo
        // Valores recomendados pelo OWASP para senhas (2023+):
        // m=19456 (19 MiB), t=2, p=1
        private const int MemorySize = 19456;
        private const int Iterations = 2;
        private const int Parallelism = 1;
        private const int HashLength = 32;   // 256 bits de output
        private const int SaltLength = 16;   // 128 bits de salt (recomendado)

        /// <summary>
        /// Gera um salt aleatório criptograficamente seguro.
        /// O salt DEVE ser único por usuário e salvo junto com o hash.
        /// </summary>
        public static byte[] GenerateSalt()
        {
            // RandomNumberGenerator usa CSPRNG (criptograficamente seguro)
            // Nunca use Random() para salt — não é seguro
            byte[] salt = new byte[SaltLength];
            RandomNumberGenerator.Fill(salt);
            return salt;
        }

        /// <summary>
        /// Gera o hash Argon2id da senha e retorna a string PHC formatada.
        /// Formato: $argon2id$v=19$m=19456,t=2,p=1$SALT_BASE64$HASH_BASE64
        ///
        /// Uso no registro:
        ///   byte[] salt = GenerateSalt();
        ///   string hash = HashPassword("senhaDoUsuario", salt);
        ///   // Salve 'hash' no banco (uma única coluna)
        /// </summary>
        public static string HashPassword(string password, byte[] salt)
        {
            byte[] hash = Argon2idHash(password, salt);

            // Base64 sem padding (padrão do formato PHC)
            string b64(byte[] b) => Convert.ToBase64String(b).TrimEnd('=');

            // Monta a string PHC:
            // $argon2id$v=19$m=19456,t=2,p=1$<salt>$<hash>
            return $"$argon2id$v=19$m={MemorySize},t={Iterations},p={Parallelism}${b64(salt)}${b64(hash)}";
        }

        /// <summary>
        /// Verifica se a senha digitada corresponde ao hash armazenado.
        ///
        /// Uso no login:
        ///   string storedHash = /* buscar do banco */;
        ///   bool ok = VerifyPassword("senhaDigitada", storedHash);
        /// </summary>
        public static bool VerifyPassword(string password, string storedHash)
        {
            // ─── 1. Parseia a string PHC ──────────────────────────────────────
            // $argon2id$v=19$m=19456,t=2,p=1$SALT$HASH
            //  [0]    [1]    [2]   [3]             [4]  [5]
            string[] parts = storedHash.Split('$');

            // parts[4] = salt em base64 (sem padding)
            // parts[5] = hash em base64 (sem padding)
            byte[] salt = Base64DecodeNoPadding(parts[4]);
            byte[] storedHashBytes = Base64DecodeNoPadding(parts[5]);

            // ─── 2. Recalcula o hash com a senha fornecida + salt extraído ───
            byte[] newHash = Argon2idHash(password, salt);

            // ─── 3. Compara em tempo fixo (evita timing attack) ──────────────
            // FixedTimeEquals compara byte a byte sem early-exit
            return CryptographicOperations.FixedTimeEquals(newHash, storedHashBytes);
        }

        /// <summary>
        /// Calcula o hash Argon2id bruto (retorna byte[], não string).
        /// </summary>
        private static byte[] Argon2idHash(string password, byte[] salt)
        {
            // Encoding.UTF8.GetBytes converte a string para byte[]
            // (a API do Argon2id exige byte[], não string)
            using var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password))
            {
                Salt = salt,
                DegreeOfParallelism = Parallelism,
                MemorySize = MemorySize,
                Iterations = Iterations
            };

            // GetBytes(32) executa o hash e retorna 32 bytes (256 bits)
            return argon2.GetBytes(HashLength);
        }

        /// <summary>
        /// Decodifica Base64 sem padding (padrão PHC).
        /// O formato PHC remove os '=' do final, então precisamos re-adicionar.
        /// </summary>
        private static byte[] Base64DecodeNoPadding(string s)
        {
            // Base64 requer tamanho múltiplo de 4 — re-adiciona o padding
            int padding = (4 - s.Length % 4) % 4;
            s = s.PadRight(s.Length + padding, '=');
            return Convert.FromBase64String(s);
        }
    }
}