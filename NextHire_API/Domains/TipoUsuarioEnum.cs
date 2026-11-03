using NpgsqlTypes;

namespace NextHire_API.Domains;

public enum TipoUsuarioEnum
{
    [PgName("candidato")]
    Candidato = 0,

    [PgName("gestor")]
    Gestor = 1,

    [PgName("analista_rh")]
    AnalistaRh = 2,

    [PgName("adm")]
    Adm = 3,

}
