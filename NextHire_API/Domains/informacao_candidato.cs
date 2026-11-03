using System;
using System.Collections.Generic;

namespace NextHire_API.Domains;

public partial class informacao_candidato
{
    public int info_candidato_id { get; set; }

    public string telefone { get; set; } = null!;

    public DateTime data_nascimento { get; set; }

    public string resumo { get; set; } = null!;

    public int candidato_id { get; set; }

    public string formacao_academica { get; set; } = null!;

    public string idioma { get; set; } = null!;

    public virtual usuario candidato { get; set; } = null!;
}
