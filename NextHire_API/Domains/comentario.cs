using System;
using System.Collections.Generic;

namespace NextHire_API.Domains;

public partial class comentario
{
    public int comentario_id { get; set; }

    public string texto { get; set; } = null!;

    public DateTime? data_comentario { get; set; }

    public int gestor_id { get; set; }

    public int entrevista_id { get; set; }

    public virtual entrevista entrevista { get; set; } = null!;

    public virtual usuario gestor { get; set; } = null!;
}
