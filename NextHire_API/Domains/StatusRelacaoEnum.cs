using NpgsqlTypes;

namespace NextHire_API.Domains;

public enum StatusRelacaoEnum
{
    [PgName("avanca")]
    Avanca = 0,

    [PgName("nao_avanca")]
    NaoAvanca = 1,

    [PgName("considera")]
    Considera = 2,

}
