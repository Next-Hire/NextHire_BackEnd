using NpgsqlTypes;

namespace NextHire_API.Domains;

public enum EscalaTrabalhoEnum
{
    [PgName("12x36")]
    _12x36 = 0,

    [PgName("5x2")]
    _5x2 = 1,

    [PgName("4x3")]
    _4x3 = 2,

    [PgName("15x15")]
    _15x15 = 3,

    [PgName("30x30")]
    _30x30 = 4,

}
