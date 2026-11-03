using NpgsqlTypes;

namespace NextHire_API.Domains;

public enum SituacaoProfissionalEnum
{
    [PgName("empregado")]
    Empregado = 0,

    [PgName("desempregado")]
    Desempregado = 1,

}
