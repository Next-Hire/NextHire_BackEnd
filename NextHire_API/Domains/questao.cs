using System;
using System.Collections.Generic;

namespace NextHire_API.Domains;

public partial class questao
{
    public int questao_id { get; set; }

    public string texto_questao { get; set; } = null!;

    public int vaga_id { get; set; }

    public virtual ICollection<alternativa> alternativa { get; set; } = new List<alternativa>();

    public virtual ICollection<candidato_questao> candidato_questao { get; set; } = new List<candidato_questao>();

    public virtual vaga vaga { get; set; } = null!;
}
