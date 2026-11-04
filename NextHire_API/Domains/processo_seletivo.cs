using System;
using System.Collections.Generic;

namespace NextHire_API.Domains;

public partial class processo_seletivo
{
    public int processo_seletivo_id { get; set; }

    public string titulo_processo { get; set; } = null!;

    public DateTime? data_abertura { get; set; }

    public DateTime data_fechamento { get; set; }

    public bool? status_processo_seletivo { get; set; }

    public int vaga_id { get; set; }

    public virtual ICollection<entrevista> entrevista { get; set; } = new List<entrevista>();

    public virtual vaga vaga { get; set; } = null!;

    public virtual ICollection<usuario> candidato { get; set; } = new List<usuario>();
}
