namespace NextHire_API.Domains;

// Propriedades de navegação adicionadas em arquivo parcial para preservar
// a classe AvaliacaoHabilidade gerada pelo scaffold do EF Core.
public partial class AvaliacaoHabilidade
{
    public virtual Usuario Candidato { get; set; } = null!;

    public virtual Entrevista Entrevista { get; set; } = null!;

    public virtual Habilidade Habilidade { get; set; } = null!;
}
