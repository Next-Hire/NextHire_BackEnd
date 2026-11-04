using System;
using System.Collections.Generic;

namespace NextHire_API.Domains;

public partial class curriculo_candidato
{
    public int curriculo_candidato_id { get; set; }

    public string curriculo_convertido { get; set; } = null!;

    public int candidato_id { get; set; }

    public virtual usuario candidato { get; set; } = null!;
}
