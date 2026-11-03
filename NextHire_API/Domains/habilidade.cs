using System;
using System.Collections.Generic;

namespace NextHire_API.Domains;

public partial class habilidade
{
    public int habilidade_id { get; set; }

    public string nome { get; set; } = null!;

    public virtual ICollection<habilidade_candidato> habilidade_candidato { get; set; } = new List<habilidade_candidato>();

    public virtual ICollection<vaga> vaga { get; set; } = new List<vaga>();
}
