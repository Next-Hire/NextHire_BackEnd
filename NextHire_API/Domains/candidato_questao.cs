using System;
using System.Collections.Generic;

namespace NextHire_API.Domains;

public partial class candidato_questao
{
    public int questao_id { get; set; }

    public int candidato_id { get; set; }

    public string reposta_questao { get; set; } = null!;

    public virtual usuario candidato { get; set; } = null!;

    public virtual questao questao { get; set; } = null!;
}
