using System;
using System.Collections.Generic;

namespace NextHire_API.Domains;

public partial class usuario
{
    public int usuario_id { get; set; }

    public string nome { get; set; } = null!;

    public string email { get; set; } = null!;

    public string senha { get; set; } = null!;

    public bool? primeiro_acesso { get; set; }

    public bool? status_usuario { get; set; }

    public virtual ICollection<anotacao> anotacaocandidato { get; set; } = new List<anotacao>();

    public virtual ICollection<anotacao> anotacaogestor { get; set; } = new List<anotacao>();

    public virtual ICollection<candidato_questao> candidato_questao { get; set; } = new List<candidato_questao>();

    public virtual ICollection<candidato_vaga> candidato_vaga { get; set; } = new List<candidato_vaga>();

    public virtual ICollection<comentario> comentario { get; set; } = new List<comentario>();

    public virtual ICollection<curriculo_candidato> curriculo_candidato { get; set; } = new List<curriculo_candidato>();

    public virtual ICollection<experiencia_profissional> experiencia_profissional { get; set; } = new List<experiencia_profissional>();

    public virtual ICollection<habilidade_candidato> habilidade_candidato { get; set; } = new List<habilidade_candidato>();

    public virtual ICollection<informacao_candidato> informacao_candidato { get; set; } = new List<informacao_candidato>();

    public virtual ICollection<vaga> vagaNavigation { get; set; } = new List<vaga>();

    public virtual ICollection<area_ocupacao> area_ocupacao { get; set; } = new List<area_ocupacao>();

    public virtual ICollection<empresa> empresa { get; set; } = new List<empresa>();

    public virtual ICollection<entrevista> entrevista { get; set; } = new List<entrevista>();

    public virtual ICollection<entrevista> entrevistaNavigation { get; set; } = new List<entrevista>();

    public virtual ICollection<processo_seletivo> processo_seletivo { get; set; } = new List<processo_seletivo>();

    public virtual ICollection<setor_ocupacao> setor_ocupacao { get; set; } = new List<setor_ocupacao>();

    public virtual ICollection<vaga> vaga { get; set; } = new List<vaga>();
}
