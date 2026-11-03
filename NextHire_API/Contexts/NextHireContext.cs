
                            
            
                                                
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

        public NextHireContext(
            DbContextOptions<NextHireContext> options
        ) : base(options)
        {
        }

        // =========================================================
        // 6. DBSETS
        // =========================================================

            public virtual DbSet<alternativa> alternativa { get; set; }

            public virtual DbSet<anotacao> anotacao { get; set; }

            public virtual DbSet<area_ocupacao> area_ocupacao { get; set; }

            public virtual DbSet<avaliacao_habilidade> avaliacao_habilidade { get; set; }

            public virtual DbSet<candidato_questao> candidato_questao { get; set; }

            public virtual DbSet<candidato_vaga> candidato_vaga { get; set; }

            public virtual DbSet<comentario> comentario { get; set; }

            public virtual DbSet<curriculo_candidato> curriculo_candidato { get; set; }

            public virtual DbSet<empresa> empresa { get; set; }

            public virtual DbSet<entrevista> entrevista { get; set; }

            public virtual DbSet<experiencia_profissional> experiencia_profissional { get; set; }

            public virtual DbSet<habilidade> habilidade { get; set; }

            public virtual DbSet<habilidade_candidato> habilidade_candidato { get; set; }

            public virtual DbSet<informacao_candidato> informacao_candidato { get; set; }

            public virtual DbSet<processo_seletivo> processo_seletivo { get; set; }

            public virtual DbSet<questao> questao { get; set; }

            public virtual DbSet<setor_ocupacao> setor_ocupacao { get; set; }

            public virtual DbSet<usuario> usuario { get; set; }

            public virtual DbSet<vaga> vaga { get; set; }

            protected override void OnConfiguring(
            DbContextOptionsBuilder optionsBuilder
        )
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql(
                    "Server=localhost;Port=5432;Database=nexthire_db;User Id=nexthire;Password=nexthire;Trust Server Certificate=true",
                    npgsqlOptions =>
                    {
                            npgsqlOptions.MapEnum<
                            global::NextHire_API.Domains.ClassificacaoQuestaoEnum
                        >(
                            "classificacao_questao_enum",
                            "public"
                        );
                            npgsqlOptions.MapEnum<
                            global::NextHire_API.Domains.EscalaTrabalhoEnum
                        >(
                            "escala_trabalho_enum",
                            "public"
                        );
                            npgsqlOptions.MapEnum<
                            global::NextHire_API.Domains.ModeloTrabaloEnum
                        >(
                            "modelo_trabalo_enum",
                            "public"
                        );
                            npgsqlOptions.MapEnum<
                            global::NextHire_API.Domains.NivelPrioridadeEnum
                        >(
                            "nivel_prioridade_enum",
                            "public"
                        );
                            npgsqlOptions.MapEnum<
                            global::NextHire_API.Domains.SituacaoProfissionalEnum
                        >(
                            "situacao_profissional_enum",
                            "public"
                        );
                            npgsqlOptions.MapEnum<
                            global::NextHire_API.Domains.StatusRelacaoEnum
                        >(
                            "status_relacao_enum",
                            "public"
                        );
                            npgsqlOptions.MapEnum<
                            global::NextHire_API.Domains.TipoQuestaoEnum
                        >(
                            "tipo_questao_enum",
                            "public"
                        );
                            npgsqlOptions.MapEnum<
                            global::NextHire_API.Domains.TipoUsuarioEnum
                        >(
                            "tipo_usuario_enum",
                            "public"
                        );
                        }
                );
            }
        }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
                modelBuilder.HasPostgresEnum<
                global::NextHire_API.Domains.ClassificacaoQuestaoEnum
            >(
                "public",
                "classificacao_questao_enum"
            );

                modelBuilder.HasPostgresEnum<
                global::NextHire_API.Domains.EscalaTrabalhoEnum
            >(
                "public",
                "escala_trabalho_enum"
            );

                modelBuilder.HasPostgresEnum<
                global::NextHire_API.Domains.ModeloTrabaloEnum
            >(
                "public",
                "modelo_trabalo_enum"
            );

                modelBuilder.HasPostgresEnum<
                global::NextHire_API.Domains.NivelPrioridadeEnum
            >(
                "public",
                "nivel_prioridade_enum"
            );

                modelBuilder.HasPostgresEnum<
                global::NextHire_API.Domains.SituacaoProfissionalEnum
            >(
                "public",
                "situacao_profissional_enum"
            );

                modelBuilder.HasPostgresEnum<
                global::NextHire_API.Domains.StatusRelacaoEnum
            >(
                "public",
                "status_relacao_enum"
            );

                modelBuilder.HasPostgresEnum<
                global::NextHire_API.Domains.TipoQuestaoEnum
            >(
                "public",
                "tipo_questao_enum"
            );

                modelBuilder.HasPostgresEnum<
                global::NextHire_API.Domains.TipoUsuarioEnum
            >(
                "public",
                "tipo_usuario_enum"
            );

                modelBuilder.Entity<alternativa>(entity =>
            {
                    entity.HasKey(
                    e => e.alternativa_id
                );

                    entity.Property(e => e.texto_alternativa).IsRequired();

                    entity.HasOne(d => d.questao)
                    .WithMany(
                        p => p.alternativa
                    )
                    .HasForeignKey(d => d.questao_id)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                });

                modelBuilder.Entity<anotacao>(entity =>
            {
                    entity.HasKey(
                    e => e.anotacao_id
                );

                    entity.Property(e => e.comentario).IsRequired();

                    entity.HasOne(d => d.candidato)
                    .WithMany(
                        p => p.anotacaocandidato
                    )
                    .HasForeignKey(d => d.candidato_id)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                    entity.HasOne(d => d.gestor)
                    .WithMany(
                        p => p.anotacaogestor
                    )
                    .HasForeignKey(d => d.gestor_id)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                });

                modelBuilder.Entity<area_ocupacao>(entity =>
            {
                    entity.HasKey(
                    e => e.area_ocupacao_id
                );

                    entity.Property(e => e.nome).IsRequired();

                });

                modelBuilder.Entity<avaliacao_habilidade>(entity =>
            {
                    entity.HasOne(d => d.candidato)
                    .WithMany(
                        
                    )
                    .HasForeignKey(d => d.candidato_id)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                    entity.HasOne(d => d.entrevista)
                    .WithMany(
                        
                    )
                    .HasForeignKey(d => d.entrevista_id)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                    entity.HasOne(d => d.habilidade)
                    .WithMany(
                        
                    )
                    .HasForeignKey(d => d.habilidade_id)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                });

                modelBuilder.Entity<candidato_questao>(entity =>
            {
                    entity.HasKey(
                    e => new { e.questao_id, e.candidato_id }
                );

                    entity.Property(e => e.reposta_questao)
                .IsRequired()
                .HasColumnType("jsonb");

                    entity.HasOne(d => d.candidato)
                    .WithMany(
                        p => p.candidato_questao
                    )
                    .HasForeignKey(d => d.candidato_id)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                    entity.HasOne(d => d.questao)
                    .WithMany(
                        p => p.candidato_questao
                    )
                    .HasForeignKey(d => d.questao_id)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                });

                modelBuilder.Entity<candidato_vaga>(entity =>
            {
                    entity.HasKey(
                    e => new { e.candidato_id, e.vaga_id }
                );

                    entity.Property(e => e.data_hora_inscricao)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone");

                    entity.HasOne(d => d.candidato)
                    .WithMany(
                        p => p.candidato_vaga
                    )
                    .HasForeignKey(d => d.candidato_id)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                    entity.HasOne(d => d.vaga)
                    .WithMany(
                        p => p.candidato_vaga
                    )
                    .HasForeignKey(d => d.vaga_id)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                });

                modelBuilder.Entity<comentario>(entity =>
            {
                    entity.HasKey(
                    e => e.comentario_id
                );

                    entity.Property(e => e.data_comentario)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone");

                    entity.Property(e => e.texto).IsRequired();

                    entity.HasOne(d => d.entrevista)
                    .WithMany(
                        p => p.comentario
                    )
                    .HasForeignKey(d => d.entrevista_id)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                    entity.HasOne(d => d.gestor)
                    .WithMany(
                        p => p.comentario
                    )
                    .HasForeignKey(d => d.gestor_id)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                });

                modelBuilder.Entity<curriculo_candidato>(entity =>
            {
                    entity.HasKey(
                    e => e.curriculo_candidato_id
                );

                    entity.Property(e => e.curriculo_convertido)
                .IsRequired()
                .HasColumnType("jsonb");

                    entity.HasOne(d => d.candidato)
                    .WithMany(
                        p => p.curriculo_candidato
                    )
                    .HasForeignKey(d => d.candidato_id)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                });

                modelBuilder.Entity<empresa>(entity =>
            {
                    entity.HasKey(
                    e => e.empresa_id
                );

                    entity.Property(e => e.nome).IsRequired();

                    entity.Property(e => e.status_empresa).HasDefaultValue(true);

                });

                modelBuilder.Entity<entrevista>(entity =>
            {
                    entity.HasKey(
                    e => e.entrevista_id
                );

                    entity.Property(e => e.horario_fim).HasColumnType("timestamp without time zone");

                    entity.Property(e => e.horario_inicio)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone");

                    entity.Property(e => e.nome_entrevista).IsRequired();

                    entity.Property(e => e.status_entrevista).HasDefaultValue(true);

                    entity.HasOne(d => d.processo_seletivo)
                    .WithMany(
                        p => p.entrevista
                    )
                    .HasForeignKey(d => d.processo_seletivo_id)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                });

                modelBuilder.Entity<experiencia_profissional>(entity =>
            {
                    entity.HasKey(
                    e => e.experiencia_profissional_id
                );

                    entity.Property(e => e.atualmente_trabalho).HasDefaultValue(true);

                    entity.Property(e => e.data_entrada).HasColumnType("timestamp(0) without time zone");

                    entity.Property(e => e.data_saida).HasColumnType("timestamp(0) without time zone");

                    entity.HasOne(d => d.usuario)
                    .WithMany(
                        p => p.experiencia_profissional
                    )
                    .HasForeignKey(d => d.usuario_id)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                });

                modelBuilder.Entity<habilidade>(entity =>
            {
                    entity.HasKey(
                    e => e.habilidade_id
                );

                    entity.Property(e => e.nome).IsRequired();

                });

                modelBuilder.Entity<habilidade_candidato>(entity =>
            {
                    entity.HasKey(
                    e => new { e.habilidade_id, e.candidato_id }
                );

                    entity.HasOne(d => d.candidato)
                    .WithMany(
                        p => p.habilidade_candidato
                    )
                    .HasForeignKey(d => d.candidato_id)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                    entity.HasOne(d => d.habilidade)
                    .WithMany(
                        p => p.habilidade_candidato
                    )
                    .HasForeignKey(d => d.habilidade_id)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                });

                modelBuilder.Entity<informacao_candidato>(entity =>
            {
                    entity.HasKey(
                    e => e.info_candidato_id
                );

                    entity.Property(e => e.data_nascimento).HasColumnType("timestamp(0) without time zone");

                    entity.Property(e => e.formacao_academica)
                .IsRequired()
                .HasColumnType("jsonb");

                    entity.Property(e => e.idioma)
                .IsRequired()
                .HasColumnType("jsonb");

                    entity.Property(e => e.resumo).IsRequired();

                    entity.Property(e => e.telefone)
                .IsRequired()
                .HasColumnType("character varying");

                    entity.HasOne(d => d.candidato)
                    .WithMany(
                        p => p.informacao_candidato
                    )
                    .HasForeignKey(d => d.candidato_id)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                });

                modelBuilder.Entity<processo_seletivo>(entity =>
            {
                    entity.HasKey(
                    e => e.processo_seletivo_id
                );

                    entity.Property(e => e.data_abertura)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone");

                    entity.Property(e => e.data_fechamento).HasColumnType("timestamp without time zone");

                    entity.Property(e => e.status_processo_seletivo).HasDefaultValue(true);

                    entity.Property(e => e.titulo_processo).IsRequired();

                    entity.HasOne(d => d.vaga)
                    .WithMany(
                        p => p.processo_seletivo
                    )
                    .HasForeignKey(d => d.vaga_id)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                });

                modelBuilder.Entity<questao>(entity =>
            {
                    entity.HasKey(
                    e => e.questao_id
                );

                    entity.Property(e => e.texto_questao).IsRequired();

                    entity.HasOne(d => d.vaga)
                    .WithMany(
                        p => p.questao
                    )
                    .HasForeignKey(d => d.vaga_id)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                });

                modelBuilder.Entity<setor_ocupacao>(entity =>
            {
                    entity.HasKey(
                    e => e.setor_ocupacao_id
                );

                    entity.Property(e => e.nome).IsRequired();

                    entity.HasOne(d => d.area_ocupacao)
                    .WithMany(
                        p => p.setor_ocupacao
                    )
                    .HasForeignKey(d => d.area_ocupacao_id)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                    entity.HasOne(d => d.empresa)
                    .WithMany(
                        p => p.setor_ocupacao
                    )
                    .HasForeignKey(d => d.empresa_id)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                });

                modelBuilder.Entity<usuario>(entity =>
            {
                    entity.HasKey(
                    e => e.usuario_id
                );

                    entity.Property(e => e.email)
                .IsRequired()
                .HasColumnType("character varying");

                    entity.Property(e => e.nome).IsRequired();

                    entity.Property(e => e.primeiro_acesso).HasDefaultValue(true);

                    entity.Property(e => e.senha)
                .IsRequired()
                .HasColumnType("character varying");

                    entity.Property(e => e.status_usuario).HasDefaultValue(true);

                });

                modelBuilder.Entity<vaga>(entity =>
            {
                    entity.HasKey(
                    e => e.vaga_id
                );

                    entity.Property(e => e.data_abertura)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone");

                    entity.Property(e => e.data_fechamento).HasColumnType("timestamp without time zone");

                    entity.Property(e => e.descricao).IsRequired();

                    entity.Property(e => e.idiomas_desejados)
                .IsRequired()
                .HasColumnType("jsonb");

                    entity.Property(e => e.status_vaga).HasDefaultValue(true);

                    entity.Property(e => e.tempo_experiencia_desejado).HasPrecision(10, 2);

                    entity.Property(e => e.titulo).IsRequired();

                    entity.HasOne(d => d.analista_rhNavigation)
                    .WithMany(
                        p => p.vagaNavigation
                    )
                    .HasForeignKey(d => d.analista_rh)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                    entity.HasOne(d => d.empresa)
                    .WithMany(
                        p => p.vaga
                    )
                    .HasForeignKey(d => d.empresa_id)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                });

                OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }

    