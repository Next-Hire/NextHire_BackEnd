using System;
using System.Collections.Generic;

namespace NextHire_API.Domains;

public partial class vaga
{
    public int vaga_id { get; set; }

    public string titulo { get; set; } = null!;

    public DateTime? data_abertura { get; set; }

    public DateTime data_fechamento { get; set; }

    public string descricao { get; set; } = null!;

    public bool? status_vaga { get; set; }

    public string idiomas_desejados { get; set; } = null!;

    public decimal? tempo_experiencia_desejado { get; set; }

    public int empresa_id { get; set; }

    public int analista_rh { get; set; }

    public virtual usuario analista_rhNavigation { get; set; } = null!;

    public virtual ICollection<candidato_vaga> candidato_vaga { get; set; } = new List<candidato_vaga>();

    public virtual empresa empresa { get; set; } = null!;

    public virtual ICollection<processo_seletivo> processo_seletivo { get; set; } = new List<processo_seletivo>();

    public virtual ICollection<questao> questao { get; set; } = new List<questao>();

    public virtual ICollection<usuario> gestor { get; set; } = new List<usuario>();

    public virtual ICollection<habilidade> habilidade { get; set; } = new List<habilidade>();
}
