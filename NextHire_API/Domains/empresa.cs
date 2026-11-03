using System;
using System.Collections.Generic;

namespace NextHire_API.Domains;

public partial class empresa
{
    public int empresa_id { get; set; }

    public string nome { get; set; } = null!;

    public bool? status_empresa { get; set; }

    public virtual ICollection<setor_ocupacao> setor_ocupacao { get; set; } = new List<setor_ocupacao>();

    public virtual ICollection<vaga> vaga { get; set; } = new List<vaga>();

    public virtual ICollection<usuario> funcionario { get; set; } = new List<usuario>();
}
