using System;
using System.Collections.Generic;

namespace NextHire_API.Domains;

public partial class anotacao
{
    public int anotacao_id { get; set; }

    public string comentario { get; set; } = null!;

    public int candidato_id { get; set; }

    public int gestor_id { get; set; }

    public virtual usuario candidato { get; set; } = null!;

    public virtual usuario gestor { get; set; } = null!;
}
