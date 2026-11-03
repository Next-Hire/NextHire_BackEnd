using System;
using System.Collections.Generic;

namespace NextHire_API.Domains;

public partial class experiencia_profissional
{
    public int experiencia_profissional_id { get; set; }

    public DateTime data_entrada { get; set; }

    public DateTime? data_saida { get; set; }

    public bool? atualmente_trabalho { get; set; }

    public int usuario_id { get; set; }

    public virtual usuario usuario { get; set; } = null!;
}
