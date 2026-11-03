using System;
using System.Collections.Generic;

namespace NextHire_API.Domains;

public partial class habilidade_candidato
{
    public int habilidade_id { get; set; }

    public int candidato_id { get; set; }

    public int nivel { get; set; }

    public virtual usuario candidato { get; set; } = null!;

    public virtual habilidade habilidade { get; set; } = null!;
}
