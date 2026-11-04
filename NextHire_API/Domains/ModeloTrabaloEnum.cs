using NpgsqlTypes;

namespace NextHire_API.Domains;

public enum ModeloTrabaloEnum
{
    [PgName("hibrido")]
    Hibrido = 0,

    [PgName("presencial")]
    Presencial = 1,

    [PgName("remoto")]
    Remoto = 2,

}
