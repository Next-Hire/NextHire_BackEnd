using NpgsqlTypes;

namespace NextHire_API.Domains
{
    public enum CandidatoEnum
    {
        [PgName("candidatoOAuth")]
        _candidatoOAuth = 0,

        [PgName("candidatoPadrao")]
        _candidatoPadrao = 1
    }
}
