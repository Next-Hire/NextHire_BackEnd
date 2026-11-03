using System;
using System.Collections.Generic;

namespace NextHire_API.Domains;

public partial class area_ocupacao
{
    public int area_ocupacao_id { get; set; }

    public string nome { get; set; } = null!;

    public virtual ICollection<setor_ocupacao> setor_ocupacao { get; set; } = new List<setor_ocupacao>();

    public virtual ICollection<usuario> usuario { get; set; } = new List<usuario>();
}
