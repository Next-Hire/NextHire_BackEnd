using System;
using System.Collections.Generic;

namespace NextHire_API.Domains;

public partial class avaliacao_habilidade
{
    public int habilidade_id { get; set; }

    public int candidato_id { get; set; }

    public int entrevista_id { get; set; }

    public int nivel_avaliado { get; set; }

    public virtual usuario candidato { get; set; } = null!;

    public virtual entrevista entrevista { get; set; } = null!;

    public virtual habilidade habilidade { get; set; } = null!;
}
