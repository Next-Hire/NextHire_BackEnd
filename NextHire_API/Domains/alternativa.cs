using System;
using System.Collections.Generic;

namespace NextHire_API.Domains;

public partial class alternativa
{
    public int alternativa_id { get; set; }

    public string texto_alternativa { get; set; } = null!;

    public bool alternativa_correta { get; set; }

    public int questao_id { get; set; }

    public virtual questao questao { get; set; } = null!;
}
