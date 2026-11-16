-- ═══════════════════════════════════════════════════════════════════════════
-- NextHire — Script de seed (INSERT)
-- ═══════════════════════════════════════════════════════════════════════════
-- ⚠️  AMBIENTE DE DESENVOLVIMENTO — NÃO USAR EM PRODUÇÃO
--
-- SENHAS: todos os usuários usam a senha "123", armazenada como hash
--         Argon2id no formato PHC, 100% compatível com
--         NextHire_API/Applications/Convertions/Argon2/PassToArgon2.cs
--
-- Parâmetros (idênticos ao PassToArgon2.cs):
--   $argon2id$v=19$m=19456,t=2,p=1$<salt b64 sem padding>$<hash b64 sem padding>
--   m = 19456 KiB (19 MiB) | t = 2 | p = 1 | hash 32 bytes | salt 16 bytes
--
-- Cada usuário tem um SALT ÚNICO (gerado com CSPRNG).
-- Gerados com a mesma biblioteca do projeto (Konscious.Security.Cryptography
-- Argon2 1.3.1, .NET 10) e auto-verificados com VerifyPassword().
--
-- LOGIN para teste: senha "123" para qualquer usuário seed.
--
-- MELHORIAS em relação à versão anterior:
--   1. Senhas em Argon2id (antes: texto puro 'NextHire@123');
--   2. TRUNCATE idempotente no início (script pode ser re-executado sem
--      erro de duplicate key; tolera schemas com ou sem candidato_vaga);
--   3. vaga.setor_id preenchido (NOT NULL no schema atual — antes causava
--      "null value in column setor_id violates not-null constraint");
--   4. candidato_vaga → curriculo_vaga (a tabela foi removida do schema
--      atual e substituída por curriculo_vaga);
--   5. Transação + setval de sequências corrigido.
-- ═══════════════════════════════════════════════════════════════════════════

BEGIN;

-- ─── LIMPEZA IDEMPOTENTE ───────────────────────────────────────────────────
-- Trunca apenas as tabelas que existirem (tolera CREATE antigo/novo).
-- RESTART IDENTITY reinicia as sequências; CASCADE cobre as FKs.
DO $$
DECLARE t text;
BEGIN
    FOREACH t IN ARRAY ARRAY[
        'curriculo_vaga', 'candidato_vaga',
        'curriculo_candidato', 'avaliacao_habilidade', 'comentario',
        'candidato_entrevista', 'entrevistador_entrevista', 'entrevista',
        'candidato_processo_seletivo', 'processo_seletivo',
        'candidato_questao', 'alternativa', 'questao',
        'habilidade_candidato', 'habilidade_vaga', 'habilidade',
        'gestor_vaga', 'vaga',
        'gestor_setor', 'setor_ocupacao',
        'usuario_area', 'area_ocupacao',
        'empresa_funcionario', 'anotacao', 'informacao_candidato',
        'usuario', 'empresa'
    ]
    LOOP
        IF to_regclass(format('public.%I', t)) IS NOT NULL THEN
            EXECUTE format('TRUNCATE TABLE public.%I RESTART IDENTITY CASCADE', t);
        END IF;
    END LOOP;
END $$;

-- ─── EMPRESAS ──────────────────────────────────────────────────────────────
INSERT INTO empresa (empresa_id, nome, status_empresa) VALUES
(1, 'TechNova Solutions', TRUE),
(2, 'Inova Saúde Digital', TRUE);

-- ─── USUÁRIOS ──────────────────────────────────────────────────────────────
-- senha de TODOS: "123" (hash Argon2id, salt único por usuário)
INSERT INTO usuario
(usuario_id, nome, email, senha, primeiro_acesso, status_usuario, tipo_usuario)
VALUES
(1, 'Administrador NextHire', 'admin@nexthire.com',
 '$argon2id$v=19$m=19456,t=2,p=1$40o4/uZ6PxatwMEcx1Y7/g$WBTmXs/9qbpz24VGOzd0Wr2eoiNWjTKv+wFi5XprgUQ',
 FALSE, TRUE, 'adm'),
(2, 'Mariana Costa', 'mariana@technova.com',
 '$argon2id$v=19$m=19456,t=2,p=1$h8HC16pxUPC+jo+ecCpK1Q$kEaeTDVIZOgVuQHf077PbZ+mLkN9wVdnyl9FHbpYkoY',
 FALSE, TRUE, 'analista_rh'),
(3, 'Rafael Almeida', 'rafael@technova.com',
 '$argon2id$v=19$m=19456,t=2,p=1$oKdo6GPvhwNDSHJ21LriOA$JqZfJWJwOjjxkUM5XrIZUS6LXOQBAKlbHkBrU8z277c',
 FALSE, TRUE, 'gestor'),
(4, 'Fernanda Lima', 'fernanda@inovasaude.com',
 '$argon2id$v=19$m=19456,t=2,p=1$u9HsUohUreJgcyaWIGwmkA$nMkquLqY6LAfIXbGpHlHv4FYHGOg6u53yOU91/1c7ZA',
 FALSE, TRUE, 'gestor'),
(5, 'Ana Beatriz Souza', 'ana@email.com',
 '$argon2id$v=19$m=19456,t=2,p=1$l+UMPh+QFnyl7rCKyM0KLQ$M6beWKZIv54zjI3vG72RC5w8O4BYRzMDoLry8zHtsRs',
 FALSE, TRUE, 'candidato'),
(6, 'Bruno Martins', 'bruno@email.com',
 '$argon2id$v=19$m=19456,t=2,p=1$vcUJASVyIBizc/76N0TITw$FONTwH7i29ZfmlteckAeBX7dUoLJmjm+hlipAwCRCHE',
 FALSE, TRUE, 'candidato'),
(7, 'Carla Oliveira', 'carla@email.com',
 '$argon2id$v=19$m=19456,t=2,p=1$wwMY4QvEnk5t+hDBmsvq/w$RcDdpk9kKhOTyW/ub0q4WqnG2lvj69m8Rha3tm3LxLQ',
 FALSE, TRUE, 'candidato'),
(8, 'Diego Santos', 'diego@email.com',
 '$argon2id$v=19$m=19456,t=2,p=1$jxCi9z3syfaqOkrKw8FqNQ$b7qrT6N3U1Kyn07PtHkEZXa6fsXuhclllr8ltf/ZZ7A',
 FALSE, TRUE, 'candidato'),
(9, 'Juliana Ribeiro', 'juliana@inovasaude.com',
 '$argon2id$v=19$m=19456,t=2,p=1$L3duIDOMxlmU2qTQFZMGbg$Zyoa4GyPCob8IL4swievG9Lx4NkQaNrvv3dXL9QOIZ4',
 FALSE, TRUE, 'analista_rh');

-- ─── EXPERIÊNCIAS PROFISSIONAIS ────────────────────────────────────────────
INSERT INTO experiencia_profissional
(experiencia_profissional_id, nome_empresa, cargo, data_entrada, data_saida, atualmente_trabalho, usuario_id)
VALUES
(1, 'DevSolutions', 'Desenvolvedora Full Stack', '2021-03-01', '2023-06-30', FALSE, 5),
(2, 'CodeLab', 'Desenvolvedora Backend', '2023-07-01', NULL, TRUE, 5),
(3, 'DataCorp', 'Analista de Dados Júnior', '2020-02-01', '2022-12-31', FALSE, 6),
(4, 'Analytics Pro', 'Analista de Dados Pleno', '2023-01-01', NULL, TRUE, 6),
(5, 'Sistemas Brasil', 'Analista de Sistemas', '2019-05-01', '2022-08-31', FALSE, 7),
(6, 'HealthTech', 'Desenvolvedora de Sistemas', '2022-09-01', NULL, TRUE, 7),
(7, 'WebStart', 'Desenvolvedor Frontend', '2022-01-01', NULL, TRUE, 8);

-- ─── ANOTAÇÕES ─────────────────────────────────────────────────────────────
INSERT INTO anotacao
(anotacao_id, conteudo, candidato_id, gestor_id)
VALUES
(1, 'Candidato apresenta boa experiência com desenvolvimento backend.', 5, 3),
(2, 'Possui conhecimentos sólidos em análise de dados.', 6, 3),
(3, 'Perfil interessante para vagas de tecnologia na área da saúde.', 7, 4);

-- ─── ÁREAS DE OCUPAÇÃO ─────────────────────────────────────────────────────
INSERT INTO area_ocupacao (area_ocupacao_id, nome) VALUES
(1, 'Tecnologia da Informação'),
(2, 'Recursos Humanos'),
(3, 'Saúde e Tecnologia'),
(4, 'Dados e Inteligência Artificial');

-- ─── USUÁRIOS E ÁREAS ──────────────────────────────────────────────────────
INSERT INTO usuario_area (usuario_id, area_ocupacao_id) VALUES
(5, 1),
(5, 4),
(6, 4),
(7, 1),
(7, 3),
(8, 1);

-- ─── FUNCIONÁRIOS DAS EMPRESAS ─────────────────────────────────────────────
INSERT INTO empresa_funcionario (empresa_id, funcionario_id) VALUES
(1, 2),
(1, 3),
(2, 4),
(2, 9);

-- ─── SETORES DE OCUPAÇÃO ───────────────────────────────────────────────────
INSERT INTO setor_ocupacao
(setor_ocupacao_id, nome, empresa_id, area_ocupacao_id)
VALUES
(1, 'Desenvolvimento de Software', 1, 1),
(2, 'Engenharia de Dados', 1, 4),
(3, 'Recursos Humanos', 2, 2),
(4, 'Sistemas Hospitalares', 2, 3);

-- ─── GESTORES DOS SETORES ──────────────────────────────────────────────────
INSERT INTO gestor_setor (gestor_id, setor_ocupacao_id) VALUES
(3, 1),
(3, 2),
(4, 3),
(4, 4);

-- ─── VAGAS ─────────────────────────────────────────────────────────────────
-- setor_id agora é NOT NULL no schema (vaga 1 → setor 1, vaga 2 → setor 2,
-- vaga 3 → setor 4; todos pertencem à empresa da respectiva vaga)
INSERT INTO vaga
(vaga_id, titulo, data_abertura, data_fechamento, descricao, status_vaga,
 idiomas_desejados, tempo_experiencia_desejado, escala_vaga,
 modelo_trabalho_vaga, nivel_prioridade_vaga, empresa_id, analista_rh, setor_id)
VALUES
(
    1,
    'Desenvolvedor Backend Pleno',
    '2026-10-01',
    '2026-12-31',
    'Desenvolvimento e manutenção de APIs REST utilizando C#, .NET, PostgreSQL e Entity Framework Core.',
    TRUE,
    '["Português", "Inglês"]'::JSONB,
    2.00,
    '5x2',
    'hibrido',
    'alta',
    1,
    2,
    1
),
(
    2,
    'Analista de Dados Pleno',
    '2026-10-05',
    '2026-12-20',
    'Análise de dados, construção de dashboards, consultas SQL e desenvolvimento de indicadores de negócio.',
    TRUE,
    '["Português", "Inglês"]'::JSONB,
    2.00,
    '5x2',
    'remoto',
    'media',
    1,
    2,
    2
),
(
    3,
    'Desenvolvedor de Sistemas Hospitalares',
    '2026-10-10',
    '2027-01-15',
    'Desenvolvimento de sistemas para gestão hospitalar, integração de APIs e manutenção de aplicações.',
    TRUE,
    '["Português"]'::JSONB,
    2.00,
    '5x2',
    'presencial',
    'alta',
    2,
    9,
    4
);

-- ─── GESTORES RESPONSÁVEIS PELAS VAGAS ─────────────────────────────────────
INSERT INTO gestor_vaga (gestor_id, vaga_id) VALUES
(3, 1),
(3, 2),
(4, 3);

-- As candidaturas (curriculo_vaga) são inseridas APÓS o insert de
-- curriculo_candidato, no final deste script (dependência de FK).

-- ─── HABILIDADES ───────────────────────────────────────────────────────────
INSERT INTO habilidade (habilidade_id, nome) VALUES
(1, 'C#'),
(2, '.NET'),
(3, 'PostgreSQL'),
(4, 'Entity Framework Core'),
(5, 'Python'),
(6, 'SQL'),
(7, 'Power BI'),
(8, 'JavaScript'),
(9, 'React'),
(10, 'Docker'),
(11, 'Git'),
(12, 'Comunicação');

-- ─── HABILIDADES EXIGIDAS PELAS VAGAS ──────────────────────────────────────
INSERT INTO habilidade_vaga (habilidade_id, vaga_id) VALUES
(1, 1),
(2, 1),
(3, 1),
(4, 1),
(10, 1),
(11, 1),
(5, 2),
(6, 2),
(7, 2),
(3, 2),
(1, 3),
(2, 3),
(3, 3),
(8, 3),
(11, 3);

-- ─── HABILIDADES DOS CANDIDATOS ────────────────────────────────────────────
INSERT INTO habilidade_candidato (habilidade_id, candidato_id, nivel) VALUES
(1, 5, 5),
(2, 5, 5),
(3, 5, 4),
(4, 5, 5),
(10, 5, 3),
(11, 5, 4),
(5, 6, 5),
(6, 6, 5),
(7, 6, 4),
(3, 6, 4),
(1, 7, 4),
(2, 7, 4),
(3, 7, 3),
(8, 7, 4),
(9, 7, 4),
(1, 8, 3),
(8, 8, 5),
(9, 8, 5),
(11, 8, 4);

-- ─── QUESTÕES ──────────────────────────────────────────────────────────────
INSERT INTO questao
(questao_id, texto_questao, classsificacao_questao, tipo_questao, vaga_id)
VALUES
(1, 'Você possui experiência profissional com C# e .NET?', 'tecnica', 'verdadeiro_falso', 1),
(2, 'Qual tecnologia é utilizada para mapear objetos em bancos relacionais no .NET?', 'tecnica', 'unica_escolha', 1),
(3, 'Quais práticas são importantes para desenvolver uma API REST?', 'tecnica', 'multipla_escolha', 1),
(4, 'Descreva como você resolveria um problema de lentidão em uma consulta SQL.', 'tecnica', 'dissertativa', 2),
(5, 'Como você lidaria com um prazo curto e várias demandas urgentes?', 'comportamental', 'dissertativa', 1),
(6, 'Um cliente relata um erro crítico no sistema hospitalar. Qual seria sua primeira atitude?', 'situacional', 'unica_escolha', 3);

-- ─── ALTERNATIVAS ──────────────────────────────────────────────────────────
INSERT INTO alternativa
(alternativa_id, texto_alternativa, alternativa_correta, questao_id)
VALUES
(1, 'Sim', TRUE, 1),
(2, 'Não', FALSE, 1),
(3, 'Entity Framework Core', TRUE, 2),
(4, 'Photoshop', FALSE, 2),
(5, 'Visual Studio Code', FALSE, 2),
(6, 'Utilizar códigos de status HTTP adequados', TRUE, 3),
(7, 'Validar os dados recebidos', TRUE, 3),
(8, 'Ignorar autenticação e autorização', FALSE, 3),
(9, 'Documentar os endpoints', TRUE, 3),
(10, 'Ignorar o cliente e continuar trabalhando', FALSE, 6),
(11, 'Investigar a falha, avaliar o impacto e comunicar os envolvidos', TRUE, 6),
(12, 'Excluir os dados do sistema', FALSE, 6);

-- ─── RESPOSTAS DOS CANDIDATOS ──────────────────────────────────────────────
INSERT INTO candidato_questao
(questao_id, candidato_id, reposta_questao)
VALUES
(1, 5, '{"resposta": true}'::JSONB),
(2, 5, '{"resposta": "Entity Framework Core"}'::JSONB),
(3, 5, '{"respostas": ["Utilizar códigos de status HTTP adequados", "Validar os dados recebidos", "Documentar os endpoints"]}'::JSONB),
(4, 6, '{"resposta": "Analisaria o plano de execução, verificaria índices e avaliaria a necessidade de otimizar a consulta."}'::JSONB),
(5, 5, '{"resposta": "Priorizaria as demandas por impacto e urgência, alinhando expectativas com a equipe."}'::JSONB),
(6, 7, '{"resposta": "Investigar a falha, avaliar o impacto e comunicar os envolvidos"}'::JSONB);

-- ─── PROCESSOS SELETIVOS ───────────────────────────────────────────────────
INSERT INTO processo_seletivo
(processo_seletivo_id, titulo_processo, data_abertura, data_fechamento, status_processo_seletivo, vaga_id)
VALUES
(1, 'Processo Seletivo - Backend Pleno', '2026-10-01', '2026-12-31', TRUE, 1),
(2, 'Processo Seletivo - Analista de Dados', '2026-10-05', '2026-12-20', TRUE, 2),
(3, 'Processo Seletivo - Sistemas Hospitalares', '2026-10-10', '2027-01-15', TRUE, 3);

-- ─── CANDIDATOS NOS PROCESSOS SELETIVOS ────────────────────────────────────
INSERT INTO candidato_processo_seletivo
(processo_seletivo_id, candidato_id, status_relacao)
VALUES
(1, 5, 'avanca'),
(1, 7, 'considera'),
(1, 8, 'nao_avanca'),
(2, 6, 'avanca'),
(3, 7, 'avanca'),
(3, 8, 'considera');

-- ─── ENTREVISTAS ───────────────────────────────────────────────────────────
INSERT INTO entrevista
(entrevista_id, nome_entrevista, horario_inicio, horario_fim, status_entrevista, processo_seletivo_id)
VALUES
(1, 'Entrevista Técnica - Backend', '2026-11-10 10:00:00', '2026-11-10 11:00:00', TRUE, 1),
(2, 'Entrevista com RH - Dados', '2026-11-11 14:00:00', '2026-11-11 14:45:00', TRUE, 2),
(3, 'Entrevista Técnica - Sistemas Hospitalares', '2026-11-12 09:00:00', '2026-11-12 10:00:00', TRUE, 3);

-- ─── ENTREVISTADORES ───────────────────────────────────────────────────────
INSERT INTO entrevistador_entrevista (gestor_id, entrevista_id) VALUES
(3, 1),
(3, 2),
(4, 3);

-- ─── CANDIDATOS NAS ENTREVISTAS ────────────────────────────────────────────
INSERT INTO candidato_entrevista (candidato_id, entrevista_id) VALUES
(5, 1),
(6, 2),
(7, 3);

-- ─── COMENTÁRIOS DAS ENTREVISTAS ───────────────────────────────────────────
INSERT INTO comentario
(comentario_id, texto, data_comentario, gestor_id, entrevista_id)
VALUES
(1, 'Candidato demonstrou domínio de C# e conhecimentos consistentes em .NET.', '2026-11-10 11:10:00', 3, 1),
(2, 'Apresentou boa capacidade analítica e experiência com SQL.', '2026-11-11 14:50:00', 3, 2),
(3, 'Demonstrou boa compreensão de sistemas e integrações.', '2026-11-12 10:10:00', 4, 3);

-- ─── AVALIAÇÕES DE HABILIDADES NAS ENTREVISTAS ─────────────────────────────
INSERT INTO avaliacao_habilidade
(habilidade_id, candidato_id, entrevista_id, nivel_avaliado)
VALUES
(1, 5, 1, 5),
(2, 5, 1, 5),
(3, 5, 1, 4),
(4, 5, 1, 5),
(6, 6, 2, 5),
(7, 6, 2, 4),
(1, 7, 3, 4),
(2, 7, 3, 4);

-- ─── INFORMAÇÕES DOS CANDIDATOS ────────────────────────────────────────────
INSERT INTO informacao_candidato
(info_candidato_id, telefone, data_nascimento, resumo, candidato_id,
 situacao_profissional, formacao_academica, idioma)
VALUES
(
    1,
    '(11) 99999-1111',
    '1998-05-12',
    'Desenvolvedora Full Stack com experiência em C#, .NET, PostgreSQL e desenvolvimento de APIs.',
    5,
    'empregado',
    '[{"instituicao": "Universidade de São Paulo", "curso": "Sistemas de Informação", "nivel": "Bacharelado", "ano_conclusao": 2020}]'::JSONB,
    '[{"idioma": "Português", "nivel": "Nativo"}, {"idioma": "Inglês", "nivel": "Intermediário"}]'::JSONB
),
(
    2,
    '(11) 98888-2222',
    '1995-09-20',
    'Analista de Dados com experiência em SQL, Python, Power BI e construção de indicadores.',
    6,
    'empregado',
    '[{"instituicao": "Universidade Estadual de Campinas", "curso": "Estatística", "nivel": "Bacharelado", "ano_conclusao": 2019}]'::JSONB,
    '[{"idioma": "Português", "nivel": "Nativo"}, {"idioma": "Inglês", "nivel": "Avançado"}]'::JSONB
),
(
    3,
    '(21) 97777-3333',
    '1997-02-15',
    'Desenvolvedora de sistemas com experiência em aplicações web e soluções para o setor de saúde.',
    7,
    'empregado',
    '[{"instituicao": "Universidade Federal do Rio de Janeiro", "curso": "Ciência da Computação", "nivel": "Bacharelado", "ano_conclusao": 2020}]'::JSONB,
    '[{"idioma": "Português", "nivel": "Nativo"}, {"idioma": "Inglês", "nivel": "Intermediário"}]'::JSONB
),
(
    4,
    '(31) 96666-4444',
    '1999-11-08',
    'Desenvolvedor frontend com foco em React, JavaScript e experiência do usuário.',
    8,
    'desempregado',
    '[{"instituicao": "PUC Minas", "curso": "Análise e Desenvolvimento de Sistemas", "nivel": "Tecnólogo", "ano_conclusao": 2021}]'::JSONB,
    '[{"idioma": "Português", "nivel": "Nativo"}, {"idioma": "Inglês", "nivel": "Básico"}]'::JSONB
);

-- ─── CURRÍCULOS ────────────────────────────────────────────────────────────
INSERT INTO curriculo_candidato
(curriculo_candidato_id, curriculo_convertido, candidato_id)
VALUES
(
    1,
    '{"nome": "Ana Beatriz Souza", "objetivo": "Desenvolvedora Backend", "competencias": ["C#", ".NET", "PostgreSQL", "Entity Framework Core"], "experiencias": [{"empresa": "DevSolutions", "cargo": "Desenvolvedora Full Stack"}, {"empresa": "CodeLab", "cargo": "Desenvolvedora Backend"}]}'::JSONB,
    5
),
(
    2,
    '{"nome": "Bruno Martins", "objetivo": "Analista de Dados", "competencias": ["Python", "SQL", "Power BI"], "experiencias": [{"empresa": "DataCorp", "cargo": "Analista de Dados Júnior"}, {"empresa": "Analytics Pro", "cargo": "Analista de Dados Pleno"}]}'::JSONB,
    6
),
(
    3,
    '{"nome": "Carla Oliveira", "objetivo": "Desenvolvedora de Sistemas", "competencias": ["C#", ".NET", "JavaScript", "React"], "experiencias": [{"empresa": "Sistemas Brasil", "cargo": "Analista de Sistemas"}, {"empresa": "HealthTech", "cargo": "Desenvolvedora de Sistemas"}]}'::JSONB,
    7
),
(
    4,
    '{"nome": "Diego Santos", "objetivo": "Desenvolvedor Frontend", "competencias": ["JavaScript", "React", "Git"], "experiencias": [{"empresa": "WebStart", "cargo": "Desenvolvedor Frontend"}]}'::JSONB,
    8
);

-- ─── CANDIDATURAS (CURRÍCULO × VAGA) ───────────────────────────────────────
-- Inserido aqui porque depende de curriculo_candidato (acima) e de vaga.
-- candidato_vaga foi substituída por curriculo_vaga no schema atual.
-- Mapeamento: currículo 1 → candidato 5, 2 → 6, 3 → 7, 4 → 8
INSERT INTO curriculo_vaga (curriculo_id, vaga_id, data_hora_inscricao) VALUES
(1, 1, '2026-10-15 09:30:00'),
(2, 2, '2026-10-16 10:15:00'),
(3, 1, '2026-10-17 14:00:00'),
(3, 3, '2026-10-18 11:20:00'),
(4, 1, '2026-10-19 08:45:00'),
(4, 3, '2026-10-20 16:30:00');

-- ─── ATUALIZAR SEQUÊNCIAS SERIAL ───────────────────────────────────────────
SELECT setval(pg_get_serial_sequence('empresa', 'empresa_id'), COALESCE((SELECT MAX(empresa_id) FROM empresa), 0) + 1, FALSE);
SELECT setval(pg_get_serial_sequence('usuario', 'usuario_id'), COALESCE((SELECT MAX(usuario_id) FROM usuario), 0) + 1, FALSE);
SELECT setval(pg_get_serial_sequence('experiencia_profissional', 'experiencia_profissional_id'), COALESCE((SELECT MAX(experiencia_profissional_id) FROM experiencia_profissional), 0) + 1, FALSE);
SELECT setval(pg_get_serial_sequence('anotacao', 'anotacao_id'), COALESCE((SELECT MAX(anotacao_id) FROM anotacao), 0) + 1, FALSE);
SELECT setval(pg_get_serial_sequence('area_ocupacao', 'area_ocupacao_id'), COALESCE((SELECT MAX(area_ocupacao_id) FROM area_ocupacao), 0) + 1, FALSE);
SELECT setval(pg_get_serial_sequence('setor_ocupacao', 'setor_ocupacao_id'), COALESCE((SELECT MAX(setor_ocupacao_id) FROM setor_ocupacao), 0) + 1, FALSE);
SELECT setval(pg_get_serial_sequence('vaga', 'vaga_id'), COALESCE((SELECT MAX(vaga_id) FROM vaga), 0) + 1, FALSE);
SELECT setval(pg_get_serial_sequence('habilidade', 'habilidade_id'), COALESCE((SELECT MAX(habilidade_id) FROM habilidade), 0) + 1, FALSE);
SELECT setval(pg_get_serial_sequence('questao', 'questao_id'), COALESCE((SELECT MAX(questao_id) FROM questao), 0) + 1, FALSE);
SELECT setval(pg_get_serial_sequence('alternativa', 'alternativa_id'), COALESCE((SELECT MAX(alternativa_id) FROM alternativa), 0) + 1, FALSE);
SELECT setval(pg_get_serial_sequence('processo_seletivo', 'processo_seletivo_id'), COALESCE((SELECT MAX(processo_seletivo_id) FROM processo_seletivo), 0) + 1, FALSE);
SELECT setval(pg_get_serial_sequence('entrevista', 'entrevista_id'), COALESCE((SELECT MAX(entrevista_id) FROM entrevista), 0) + 1, FALSE);
SELECT setval(pg_get_serial_sequence('comentario', 'comentario_id'), COALESCE((SELECT MAX(comentario_id) FROM comentario), 0) + 1, FALSE);
SELECT setval(pg_get_serial_sequence('informacao_candidato', 'info_candidato_id'), COALESCE((SELECT MAX(info_candidato_id) FROM informacao_candidato), 0) + 1, FALSE);
SELECT setval(pg_get_serial_sequence('curriculo_candidato', 'curriculo_candidato_id'), COALESCE((SELECT MAX(curriculo_candidato_id) FROM curriculo_candidato), 0) + 1, FALSE);

COMMIT;

-- ─── VERIFICAÇÃO PÓS-INSERT (opcional) ─────────────────────────────────────
-- Confirma que os hashes estão no formato correto e que nenhum hash coincide
-- com outro (salts únicos ⇒ hashes únicos, mesmo com a mesma senha "123"):
--
-- SELECT usuario_id, email, LEFT(senha, 27) AS prefixo_argon2id, senha
-- FROM usuario ORDER BY usuario_id;
