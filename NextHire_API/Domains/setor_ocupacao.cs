using System;
using System.Collections.Generic;

namespace NextHire_API.Domains;

public partial class setor_ocupacao
{
    public int setor_ocupacao_id { get; set; }

    public string nome { get; set; } = null!;

    public int empresa_id { get; set; }

    public int area_ocupacao_id { get; set; }

    public virtual area_ocupacao area_ocupacao { get; set; } = null!;

    public virtual empresa empresa { get; set; } = null!;

    public virtual ICollection<usuario> gestor { get; set; } = new List<usuario>();
}
