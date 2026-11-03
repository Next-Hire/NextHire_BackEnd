using System;
using System.Collections.Generic;

namespace NextHire_API.Domains;

public partial class candidato_vaga
{
    public int candidato_id { get; set; }

    public int vaga_id { get; set; }

    public DateTime? data_hora_inscricao { get; set; }

    public virtual usuario candidato { get; set; } = null!;

    public virtual vaga vaga { get; set; } = null!;
}
