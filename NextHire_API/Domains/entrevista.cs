using System;
using System.Collections.Generic;

namespace NextHire_API.Domains;

public partial class entrevista
{
    public int entrevista_id { get; set; }

    public string nome_entrevista { get; set; } = null!;

    public DateTime? horario_inicio { get; set; }

    public DateTime horario_fim { get; set; }

    public bool? status_entrevista { get; set; }

    public int processo_seletivo_id { get; set; }

    public virtual ICollection<comentario> comentario { get; set; } = new List<comentario>();

    public virtual processo_seletivo processo_seletivo { get; set; } = null!;

    public virtual ICollection<usuario> candidato { get; set; } = new List<usuario>();

    public virtual ICollection<usuario> gestor { get; set; } = new List<usuario>();
}
