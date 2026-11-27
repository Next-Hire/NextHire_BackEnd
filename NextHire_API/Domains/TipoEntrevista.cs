namespace NextHire_API.Domains
{
    public partial class TipoEntrevista
    {
        public int TipoEntrevistaId { get; set; }

        public string Nome_Tipo { get; set; } = null!;

        public string Descricao { get; set; } = null!;

        public virtual ICollection<Entrevista> Entrevista { get; set; } = new List<Entrevista>();

    }
}
