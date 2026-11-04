using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using NextHire_API.Domains;

namespace NextHire_API.Contexts;

public partial class NextHireContext : DbContext
{
    public NextHireContext()
    {
    }

    public NextHireContext(DbContextOptions<NextHireContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Alternativa> Alternativa { get; set; }

    public virtual DbSet<Anotacao> Anotacao { get; set; }

    public virtual DbSet<AreaOcupacao> AreaOcupacao { get; set; }

    public virtual DbSet<AvaliacaoHabilidade> AvaliacaoHabilidade { get; set; }

    public virtual DbSet<CandidatoQuestao> CandidatoQuestao { get; set; }

    public virtual DbSet<CandidatoVaga> CandidatoVaga { get; set; }

    public virtual DbSet<Comentario> Comentario { get; set; }

    public virtual DbSet<CurriculoCandidato> CurriculoCandidato { get; set; }

    public virtual DbSet<Empresa> Empresa { get; set; }

    public virtual DbSet<Entrevista> Entrevista { get; set; }

    public virtual DbSet<ExperienciaProfissional> ExperienciaProfissional { get; set; }

    public virtual DbSet<Habilidade> Habilidade { get; set; }

    public virtual DbSet<HabilidadeCandidato> HabilidadeCandidato { get; set; }

    public virtual DbSet<InformacaoCandidato> InformacaoCandidato { get; set; }

    public virtual DbSet<ProcessoSeletivo> ProcessoSeletivo { get; set; }

    public virtual DbSet<Questao> Questao { get; set; }

    public virtual DbSet<SetorOcupacao> SetorOcupacao { get; set; }

    public virtual DbSet<Usuario> Usuario { get; set; }

    public virtual DbSet<Vaga> Vaga { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Server=localhost;Port=5432;Database=nexthire_db;User Id=nexthire;Password=nexthire;Trust Server Certificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasPostgresEnum("classificacao_questao_enum", new[] { "comportamental", "tecnica", "situacional" })
            .HasPostgresEnum("escala_trabalho_enum", new[] { "12x36", "5x2", "4x3", "15x15", "30x30" })
            .HasPostgresEnum("modelo_trabalo_enum", new[] { "hibrido", "presencial", "remoto" })
            .HasPostgresEnum("nivel_prioridade_enum", new[] { "alta", "media", "baixa" })
            .HasPostgresEnum("situacao_profissional_enum", new[] { "empregado", "desempregado" })
            .HasPostgresEnum("status_relacao_enum", new[] { "avanca", "nao_avanca", "considera" })
            .HasPostgresEnum("tipo_questao_enum", new[] { "verdadeiro_falso", "unica_escolha", "multipla_escolha", "dissertativa" })
            .HasPostgresEnum("tipo_usuario_enum", new[] { "candidato", "gestor", "analista_rh", "adm" });

        modelBuilder.Entity<Alternativa>(entity =>
        {
            entity.HasKey(e => e.AlternativaId).HasName("alternativa_pkey");

            entity.ToTable("alternativa");

            entity.Property(e => e.AlternativaId).HasColumnName("alternativa_id");
            entity.Property(e => e.AlternativaCorreta).HasColumnName("alternativa_correta");
            entity.Property(e => e.QuestaoId).HasColumnName("questao_id");
            entity.Property(e => e.TextoAlternativa).HasColumnName("texto_alternativa");

            entity.HasOne(d => d.Questao).WithMany(p => p.Alternativa)
                .HasForeignKey(d => d.QuestaoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("alternativa_questao_id_fkey");
        });

        modelBuilder.Entity<Anotacao>(entity =>
        {
            entity.HasKey(e => e.AnotacaoId).HasName("anotacao_pkey");

            entity.ToTable("anotacao");

            entity.Property(e => e.AnotacaoId).HasColumnName("anotacao_id");
            entity.Property(e => e.CandidatoId).HasColumnName("candidato_id");
            entity.Property(e => e.Comentario).HasColumnName("comentario");
            entity.Property(e => e.GestorId).HasColumnName("gestor_id");

            entity.HasOne(d => d.Candidato).WithMany(p => p.AnotacaoCandidato)
                .HasForeignKey(d => d.CandidatoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("anotacao_candidato_id_fkey");

            entity.HasOne(d => d.Gestor).WithMany(p => p.AnotacaoGestor)
                .HasForeignKey(d => d.GestorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("anotacao_gestor_id_fkey");
        });

        modelBuilder.Entity<AreaOcupacao>(entity =>
        {
            entity.HasKey(e => e.AreaOcupacaoId).HasName("area_ocupacao_pkey");

            entity.ToTable("area_ocupacao");

            entity.Property(e => e.AreaOcupacaoId).HasColumnName("area_ocupacao_id");
            entity.Property(e => e.Nome).HasColumnName("nome");
        });

        modelBuilder.Entity<AvaliacaoHabilidade>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("avaliacao_habilidade");

            entity.Property(e => e.CandidatoId).HasColumnName("candidato_id");
            entity.Property(e => e.EntrevistaId).HasColumnName("entrevista_id");
            entity.Property(e => e.HabilidadeId).HasColumnName("habilidade_id");
            entity.Property(e => e.NivelAvaliado).HasColumnName("nivel_avaliado");

            entity.HasOne(d => d.Candidato).WithMany()
                .HasForeignKey(d => d.CandidatoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("avaliacao_habilidade_candidato_id_fkey");

            entity.HasOne(d => d.Entrevista).WithMany()
                .HasForeignKey(d => d.EntrevistaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("avaliacao_habilidade_entrevista_id_fkey");

            entity.HasOne(d => d.Habilidade).WithMany()
                .HasForeignKey(d => d.HabilidadeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("avaliacao_habilidade_habilidade_id_fkey");
        });

        modelBuilder.Entity<CandidatoQuestao>(entity =>
        {
            entity.HasKey(e => new { e.QuestaoId, e.CandidatoId }).HasName("pk_candidato_questao");

            entity.ToTable("candidato_questao");

            entity.Property(e => e.QuestaoId).HasColumnName("questao_id");
            entity.Property(e => e.CandidatoId).HasColumnName("candidato_id");
            entity.Property(e => e.RepostaQuestao)
                .HasColumnType("jsonb")
                .HasColumnName("reposta_questao");

            entity.HasOne(d => d.Candidato).WithMany(p => p.CandidatoQuestao)
                .HasForeignKey(d => d.CandidatoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("candidato_questao_candidato_id_fkey");

            entity.HasOne(d => d.Questao).WithMany(p => p.CandidatoQuestao)
                .HasForeignKey(d => d.QuestaoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("candidato_questao_questao_id_fkey");
        });

        modelBuilder.Entity<CandidatoVaga>(entity =>
        {
            entity.HasKey(e => new { e.CandidatoId, e.VagaId }).HasName("pk_candidato_vaga");

            entity.ToTable("candidato_vaga");

            entity.Property(e => e.CandidatoId).HasColumnName("candidato_id");
            entity.Property(e => e.VagaId).HasColumnName("vaga_id");
            entity.Property(e => e.DataHoraInscricao)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("data_hora_inscricao");

            entity.HasOne(d => d.Candidato).WithMany(p => p.CandidatoVaga)
                .HasForeignKey(d => d.CandidatoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("candidato_vaga_candidato_id_fkey");

            entity.HasOne(d => d.Vaga).WithMany(p => p.CandidatoVaga)
                .HasForeignKey(d => d.VagaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("candidato_vaga_vaga_id_fkey");
        });

        modelBuilder.Entity<Comentario>(entity =>
        {
            entity.HasKey(e => e.ComentarioId).HasName("comentario_pkey");

            entity.ToTable("comentario");

            entity.Property(e => e.ComentarioId).HasColumnName("comentario_id");
            entity.Property(e => e.DataComentario)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("data_comentario");
            entity.Property(e => e.EntrevistaId).HasColumnName("entrevista_id");
            entity.Property(e => e.GestorId).HasColumnName("gestor_id");
            entity.Property(e => e.Texto).HasColumnName("texto");

            entity.HasOne(d => d.Entrevista).WithMany(p => p.Comentario)
                .HasForeignKey(d => d.EntrevistaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("comentario_entrevista_id_fkey");

            entity.HasOne(d => d.Gestor).WithMany(p => p.Comentario)
                .HasForeignKey(d => d.GestorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("comentario_gestor_id_fkey");
        });

        modelBuilder.Entity<CurriculoCandidato>(entity =>
        {
            entity.HasKey(e => e.CurriculoCandidatoId).HasName("curriculo_candidato_pkey");

            entity.ToTable("curriculo_candidato");

            entity.Property(e => e.CurriculoCandidatoId).HasColumnName("curriculo_candidato_id");
            entity.Property(e => e.CandidatoId).HasColumnName("candidato_id");
            entity.Property(e => e.CurriculoConvertido)
                .HasColumnType("jsonb")
                .HasColumnName("curriculo_convertido");

            entity.HasOne(d => d.Candidato).WithMany(p => p.CurriculoCandidato)
                .HasForeignKey(d => d.CandidatoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("curriculo_candidato_candidato_id_fkey");
        });

        modelBuilder.Entity<Empresa>(entity =>
        {
            entity.HasKey(e => e.EmpresaId).HasName("empresa_pkey");

            entity.ToTable("empresa");

            entity.Property(e => e.EmpresaId).HasColumnName("empresa_id");
            entity.Property(e => e.Nome).HasColumnName("nome");
            entity.Property(e => e.StatusEmpresa)
                .HasDefaultValue(true)
                .HasColumnName("status_empresa");

            entity.HasMany(d => d.Funcionario).WithMany(p => p.Empresa)
                .UsingEntity<Dictionary<string, object>>(
                    "EmpresaFuncionario",
                    r => r.HasOne<Usuario>().WithMany()
                        .HasForeignKey("FuncionarioId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("empresa_funcionario_funcionario_id_fkey"),
                    l => l.HasOne<Empresa>().WithMany()
                        .HasForeignKey("EmpresaId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("empresa_funcionario_empresa_id_fkey"),
                    j =>
                    {
                        j.HasKey("EmpresaId", "FuncionarioId").HasName("pk_empresa_funcionario");
                        j.ToTable("empresa_funcionario");
                        j.IndexerProperty<int>("EmpresaId").HasColumnName("empresa_id");
                        j.IndexerProperty<int>("FuncionarioId").HasColumnName("funcionario_id");
                    });
        });

        modelBuilder.Entity<Entrevista>(entity =>
        {
            entity.HasKey(e => e.EntrevistaId).HasName("entrevista_pkey");

            entity.ToTable("entrevista");

            entity.Property(e => e.EntrevistaId).HasColumnName("entrevista_id");
            entity.Property(e => e.HorarioFim)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("horario_fim");
            entity.Property(e => e.HorarioInicio)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("horario_inicio");
            entity.Property(e => e.NomeEntrevista).HasColumnName("nome_entrevista");
            entity.Property(e => e.ProcessoSeletivoId).HasColumnName("processo_seletivo_id");
            entity.Property(e => e.StatusEntrevista)
                .HasDefaultValue(true)
                .HasColumnName("status_entrevista");

            entity.HasOne(d => d.ProcessoSeletivo).WithMany(p => p.Entrevista)
                .HasForeignKey(d => d.ProcessoSeletivoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("entrevista_processo_seletivo_id_fkey");
        });

        modelBuilder.Entity<ExperienciaProfissional>(entity =>
        {
            entity.HasKey(e => e.ExperienciaProfissionalId).HasName("experiencia_profissional_pkey");

            entity.ToTable("experiencia_profissional");

            entity.Property(e => e.ExperienciaProfissionalId).HasColumnName("experiencia_profissional_id");
            entity.Property(e => e.AtualmenteTrabalho)
                .HasDefaultValue(true)
                .HasColumnName("atualmente_trabalho");
            entity.Property(e => e.DataEntrada)
                .HasColumnType("timestamp(0) without time zone")
                .HasColumnName("data_entrada");
            entity.Property(e => e.DataSaida)
                .HasColumnType("timestamp(0) without time zone")
                .HasColumnName("data_saida");
            entity.Property(e => e.UsuarioId).HasColumnName("usuario_id");

            entity.HasOne(d => d.Usuario).WithMany(p => p.ExperienciaProfissional)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("experiencia_profissional_usuario_id_fkey");
        });

        modelBuilder.Entity<Habilidade>(entity =>
        {
            entity.HasKey(e => e.HabilidadeId).HasName("habilidade_pkey");

            entity.ToTable("habilidade");

            entity.Property(e => e.HabilidadeId).HasColumnName("habilidade_id");
            entity.Property(e => e.Nome).HasColumnName("nome");

            entity.HasMany(d => d.Vaga).WithMany(p => p.Habilidade)
                .UsingEntity<Dictionary<string, object>>(
                    "HabilidadeVaga",
                    r => r.HasOne<Vaga>().WithMany()
                        .HasForeignKey("VagaId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("habilidade_vaga_vaga_id_fkey"),
                    l => l.HasOne<Habilidade>().WithMany()
                        .HasForeignKey("HabilidadeId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("habilidade_vaga_habilidade_id_fkey"),
                    j =>
                    {
                        j.HasKey("HabilidadeId", "VagaId").HasName("pk_habilidade_vaga");
                        j.ToTable("habilidade_vaga");
                        j.IndexerProperty<int>("HabilidadeId").HasColumnName("habilidade_id");
                        j.IndexerProperty<int>("VagaId").HasColumnName("vaga_id");
                    });
        });

        modelBuilder.Entity<HabilidadeCandidato>(entity =>
        {
            entity.HasKey(e => new { e.HabilidadeId, e.CandidatoId }).HasName("pk_habilidade_candidato");

            entity.ToTable("habilidade_candidato");

            entity.Property(e => e.HabilidadeId).HasColumnName("habilidade_id");
            entity.Property(e => e.CandidatoId).HasColumnName("candidato_id");
            entity.Property(e => e.Nivel).HasColumnName("nivel");

            entity.HasOne(d => d.Candidato).WithMany(p => p.HabilidadeCandidato)
                .HasForeignKey(d => d.CandidatoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("habilidade_candidato_candidato_id_fkey");

            entity.HasOne(d => d.Habilidade).WithMany(p => p.HabilidadeCandidato)
                .HasForeignKey(d => d.HabilidadeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("habilidade_candidato_habilidade_id_fkey");
        });

        modelBuilder.Entity<InformacaoCandidato>(entity =>
        {
            entity.HasKey(e => e.InfoCandidatoId).HasName("informacao_candidato_pkey");

            entity.ToTable("informacao_candidato");

            entity.Property(e => e.InfoCandidatoId).HasColumnName("info_candidato_id");
            entity.Property(e => e.CandidatoId).HasColumnName("candidato_id");
            entity.Property(e => e.DataNascimento)
                .HasColumnType("timestamp(0) without time zone")
                .HasColumnName("data_nascimento");
            entity.Property(e => e.FormacaoAcademica)
                .HasColumnType("jsonb")
                .HasColumnName("formacao_academica");
            entity.Property(e => e.Idioma)
                .HasColumnType("jsonb")
                .HasColumnName("idioma");
            entity.Property(e => e.Resumo).HasColumnName("resumo");
            entity.Property(e => e.Telefone)
                .HasColumnType("character varying")
                .HasColumnName("telefone");

            entity.HasOne(d => d.Candidato).WithMany(p => p.InformacaoCandidato)
                .HasForeignKey(d => d.CandidatoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("informacao_candidato_candidato_id_fkey");
        });

        modelBuilder.Entity<ProcessoSeletivo>(entity =>
        {
            entity.HasKey(e => e.ProcessoSeletivoId).HasName("processo_seletivo_pkey");

            entity.ToTable("processo_seletivo");

            entity.Property(e => e.ProcessoSeletivoId).HasColumnName("processo_seletivo_id");
            entity.Property(e => e.DataAbertura)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("data_abertura");
            entity.Property(e => e.DataFechamento)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("data_fechamento");
            entity.Property(e => e.StatusProcessoSeletivo)
                .HasDefaultValue(true)
                .HasColumnName("status_processo_seletivo");
            entity.Property(e => e.TituloProcesso).HasColumnName("titulo_processo");
            entity.Property(e => e.VagaId).HasColumnName("vaga_id");

            entity.HasOne(d => d.Vaga).WithMany(p => p.ProcessoSeletivo)
                .HasForeignKey(d => d.VagaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("processo_seletivo_vaga_id_fkey");

            entity.HasMany(d => d.Candidato).WithMany(p => p.ProcessoSeletivo)
                .UsingEntity<Dictionary<string, object>>(
                    "CandidatoProcessoSeletivo",
                    r => r.HasOne<Usuario>().WithMany()
                        .HasForeignKey("CandidatoId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("candidato_processo_seletivo_candidato_id_fkey"),
                    l => l.HasOne<ProcessoSeletivo>().WithMany()
                        .HasForeignKey("ProcessoSeletivoId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("candidato_processo_seletivo_processo_seletivo_id_fkey"),
                    j =>
                    {
                        j.HasKey("ProcessoSeletivoId", "CandidatoId").HasName("pk_candidato_processo_seletivo");
                        j.ToTable("candidato_processo_seletivo");
                        j.IndexerProperty<int>("ProcessoSeletivoId").HasColumnName("processo_seletivo_id");
                        j.IndexerProperty<int>("CandidatoId").HasColumnName("candidato_id");
                    });
        });

        modelBuilder.Entity<Questao>(entity =>
        {
            entity.HasKey(e => e.QuestaoId).HasName("questao_pkey");

            entity.ToTable("questao");

            entity.Property(e => e.QuestaoId).HasColumnName("questao_id");
            entity.Property(e => e.TextoQuestao).HasColumnName("texto_questao");
            entity.Property(e => e.VagaId).HasColumnName("vaga_id");

            entity.HasOne(d => d.Vaga).WithMany(p => p.Questao)
                .HasForeignKey(d => d.VagaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("questao_vaga_id_fkey");
        });

        modelBuilder.Entity<SetorOcupacao>(entity =>
        {
            entity.HasKey(e => e.SetorOcupacaoId).HasName("setor_ocupacao_pkey");

            entity.ToTable("setor_ocupacao");

            entity.Property(e => e.SetorOcupacaoId).HasColumnName("setor_ocupacao_id");
            entity.Property(e => e.AreaOcupacaoId).HasColumnName("area_ocupacao_id");
            entity.Property(e => e.EmpresaId).HasColumnName("empresa_id");
            entity.Property(e => e.Nome).HasColumnName("nome");

            entity.HasOne(d => d.AreaOcupacao).WithMany(p => p.SetorOcupacao)
                .HasForeignKey(d => d.AreaOcupacaoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("setor_ocupacao_area_ocupacao_id_fkey");

            entity.HasOne(d => d.Empresa).WithMany(p => p.SetorOcupacao)
                .HasForeignKey(d => d.EmpresaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("setor_ocupacao_empresa_id_fkey");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.UsuarioId).HasName("usuario_pkey");

            entity.ToTable("usuario");

            entity.Property(e => e.UsuarioId).HasColumnName("usuario_id");
            entity.Property(e => e.Email)
                .HasColumnType("character varying")
                .HasColumnName("email");
            entity.Property(e => e.Nome).HasColumnName("nome");
            entity.Property(e => e.PrimeiroAcesso)
                .HasDefaultValue(true)
                .HasColumnName("primeiro_acesso");
            entity.Property(e => e.Senha)
                .HasColumnType("character varying")
                .HasColumnName("senha");
            entity.Property(e => e.StatusUsuario)
                .HasDefaultValue(true)
                .HasColumnName("status_usuario");

            entity.HasMany(d => d.AreaOcupacao).WithMany(p => p.Usuario)
                .UsingEntity<Dictionary<string, object>>(
                    "UsuarioArea",
                    r => r.HasOne<AreaOcupacao>().WithMany()
                        .HasForeignKey("AreaOcupacaoId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("usuario_area_area_ocupacao_id_fkey"),
                    l => l.HasOne<Usuario>().WithMany()
                        .HasForeignKey("UsuarioId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("usuario_area_usuario_id_fkey"),
                    j =>
                    {
                        j.HasKey("UsuarioId", "AreaOcupacaoId").HasName("pk_usuario_area");
                        j.ToTable("usuario_area");
                        j.IndexerProperty<int>("UsuarioId").HasColumnName("usuario_id");
                        j.IndexerProperty<int>("AreaOcupacaoId").HasColumnName("area_ocupacao_id");
                    });

            entity.HasMany(d => d.Entrevista).WithMany(p => p.Candidato)
                .UsingEntity<Dictionary<string, object>>(
                    "CandidatoEntrevista",
                    r => r.HasOne<Entrevista>().WithMany()
                        .HasForeignKey("EntrevistaId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("candidato_entrevista_entrevista_id_fkey"),
                    l => l.HasOne<Usuario>().WithMany()
                        .HasForeignKey("CandidatoId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("candidato_entrevista_candidato_id_fkey"),
                    j =>
                    {
                        j.HasKey("CandidatoId", "EntrevistaId").HasName("pk_candidato_entrevista");
                        j.ToTable("candidato_entrevista");
                        j.IndexerProperty<int>("CandidatoId").HasColumnName("candidato_id");
                        j.IndexerProperty<int>("EntrevistaId").HasColumnName("entrevista_id");
                    });

            entity.HasMany(d => d.EntrevistaNavigation).WithMany(p => p.Gestor)
                .UsingEntity<Dictionary<string, object>>(
                    "EntrevistadorEntrevista",
                    r => r.HasOne<Entrevista>().WithMany()
                        .HasForeignKey("EntrevistaId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("entrevistador_entrevista_entrevista_id_fkey"),
                    l => l.HasOne<Usuario>().WithMany()
                        .HasForeignKey("GestorId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("entrevistador_entrevista_gestor_id_fkey"),
                    j =>
                    {
                        j.HasKey("GestorId", "EntrevistaId").HasName("pk_entrevistador_entrevista");
                        j.ToTable("entrevistador_entrevista");
                        j.IndexerProperty<int>("GestorId").HasColumnName("gestor_id");
                        j.IndexerProperty<int>("EntrevistaId").HasColumnName("entrevista_id");
                    });

            entity.HasMany(d => d.SetorOcupacao).WithMany(p => p.Gestor)
                .UsingEntity<Dictionary<string, object>>(
                    "GestorSetor",
                    r => r.HasOne<SetorOcupacao>().WithMany()
                        .HasForeignKey("SetorOcupacaoId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("gestor_setor_setor_ocupacao_id_fkey"),
                    l => l.HasOne<Usuario>().WithMany()
                        .HasForeignKey("GestorId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("gestor_setor_gestor_id_fkey"),
                    j =>
                    {
                        j.HasKey("GestorId", "SetorOcupacaoId").HasName("pk_gestor_setor");
                        j.ToTable("gestor_setor");
                        j.IndexerProperty<int>("GestorId").HasColumnName("gestor_id");
                        j.IndexerProperty<int>("SetorOcupacaoId").HasColumnName("setor_ocupacao_id");
                    });

            entity.HasMany(d => d.Vaga).WithMany(p => p.Gestor)
                .UsingEntity<Dictionary<string, object>>(
                    "GestorVaga",
                    r => r.HasOne<Vaga>().WithMany()
                        .HasForeignKey("VagaId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("gestor_vaga_vaga_id_fkey"),
                    l => l.HasOne<Usuario>().WithMany()
                        .HasForeignKey("GestorId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("gestor_vaga_gestor_id_fkey"),
                    j =>
                    {
                        j.HasKey("GestorId", "VagaId").HasName("pk_gestor_vaga");
                        j.ToTable("gestor_vaga");
                        j.IndexerProperty<int>("GestorId").HasColumnName("gestor_id");
                        j.IndexerProperty<int>("VagaId").HasColumnName("vaga_id");
                    });
        });

        modelBuilder.Entity<Vaga>(entity =>
        {
            entity.HasKey(e => e.VagaId).HasName("vaga_pkey");

            entity.ToTable("vaga");

            entity.Property(e => e.VagaId).HasColumnName("vaga_id");
            entity.Property(e => e.AnalistaRh).HasColumnName("analista_rh");
            entity.Property(e => e.DataAbertura)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("data_abertura");
            entity.Property(e => e.DataFechamento)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("data_fechamento");
            entity.Property(e => e.Descricao).HasColumnName("descricao");
            entity.Property(e => e.EmpresaId).HasColumnName("empresa_id");
            entity.Property(e => e.IdiomasDesejados)
                .HasColumnType("jsonb")
                .HasColumnName("idiomas_desejados");
            entity.Property(e => e.StatusVaga)
                .HasDefaultValue(true)
                .HasColumnName("status_vaga");
            entity.Property(e => e.TempoExperienciaDesejado)
                .HasPrecision(10, 2)
                .HasColumnName("tempo_experiencia_desejado");
            entity.Property(e => e.Titulo).HasColumnName("titulo");

            entity.HasOne(d => d.AnalistaRhNavigation).WithMany(p => p.VagaNavigation)
                .HasForeignKey(d => d.AnalistaRh)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("vaga_analista_rh_fkey");

            entity.HasOne(d => d.Empresa).WithMany(p => p.Vaga)
                .HasForeignKey(d => d.EmpresaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("vaga_empresa_id_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
