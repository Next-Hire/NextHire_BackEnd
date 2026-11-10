
BEGIN;

-- EMPRESAS
INSERT INTO empresa (empresa_id, nome, status_empresa) VALUES
(1, 'TechNova Solutions', TRUE),
(2, 'Inova Saúde Digital', TRUE);

-- USUÁRIOS
INSERT INTO usuario
(usuario_id, nome, email, senha, primeiro_acesso, status_usuario, tipo_usuario)
VALUES
(1, 'Administrador NextHire', 'admin@nexthire.com', 'NextHire@123', FALSE, TRUE, 'adm'),
(2, 'Mariana Costa', 'mariana@technova.com', 'NextHire@123', FALSE, TRUE, 'analista_rh'),
(3, 'Rafael Almeida', 'rafael@technova.com', 'NextHire@123', FALSE, TRUE, 'gestor'),
(4, 'Fernanda Lima', 'fernanda@inovasaude.com', 'NextHire@123', FALSE, TRUE, 'gestor'),
(5, 'Ana Beatriz Souza', 'ana@email.com', 'NextHire@123', FALSE, TRUE, 'candidato'),
(6, 'Bruno Martins', 'bruno@email.com', 'NextHire@123', FALSE, TRUE, 'candidato'),
(7, 'Carla Oliveira', 'carla@email.com', 'NextHire@123', FALSE, TRUE, 'candidato'),
(8, 'Diego Santos', 'diego@email.com', 'NextHire@123', FALSE, TRUE, 'candidato'),
(9, 'Juliana Ribeiro', 'juliana@inovasaude.com', 'NextHire@123', FALSE, TRUE, 'analista_rh');

-- EXPERIÊNCIAS PROFISSIONAIS
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

-- ANOTAÇÕES
INSERT INTO anotacao
(anotacao_id, comentario, candidato_id, gestor_id)
VALUES
(1, 'Candidato apresenta boa experiência com desenvolvimento backend.', 5, 3),
(2, 'Possui conhecimentos sólidos em análise de dados.', 6, 3),
(3, 'Perfil interessante para vagas de tecnologia na área da saúde.', 7, 4);

-- ÁREAS DE OCUPAÇÃO
INSERT INTO area_ocupacao (area_ocupacao_id, nome) VALUES
(1, 'Tecnologia da Informação'),
(2, 'Recursos Humanos'),
(3, 'Saúde e Tecnologia'),
(4, 'Dados e Inteligência Artificial');

-- USUÁRIOS E ÁREAS
INSERT INTO usuario_area (usuario_id, area_ocupacao_id) VALUES
(5, 1),
(5, 4),
(6, 4),
(7, 1),
(7, 3),
(8, 1);

-- FUNCIONÁRIOS DAS EMPRESAS
INSERT INTO empresa_funcionario (empresa_id, funcionario_id) VALUES
(1, 2),
(1, 3),
(2, 4),
(2, 9);

-- SETORES DE OCUPAÇÃO
INSERT INTO setor_ocupacao
(setor_ocupacao_id, nome, empresa_id, area_ocupacao_id)
VALUES
(1, 'Desenvolvimento de Software', 1, 1),
(2, 'Engenharia de Dados', 1, 4),
(3, 'Recursos Humanos', 2, 2),
(4, 'Sistemas Hospitalares', 2, 3);

-- GESTORES DOS SETORES
INSERT INTO gestor_setor (gestor_id, setor_ocupacao_id) VALUES
(3, 1),
(3, 2),
(4, 3),
(4, 4);

-- VAGAS
INSERT INTO vaga
(vaga_id, titulo, data_abertura, data_fechamento, descricao, status_vaga,
 idiomas_desejados, tempo_experiencia_desejado, escala_vaga,
 modelo_trabalho_vaga, nivel_prioridade_vaga, empresa_id, analista_rh)
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
    2
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
    9
);

-- GESTORES RESPONSÁVEIS PELAS VAGAS
INSERT INTO gestor_vaga (gestor_id, vaga_id) VALUES
(3, 1),
(3, 2),
(4, 3);

-- CANDIDATURAS
INSERT INTO candidato_vaga (candidato_id, vaga_id, data_hora_inscricao) VALUES
(5, 1, '2026-10-15 09:30:00'),
(6, 2, '2026-10-16 10:15:00'),
(7, 1, '2026-10-17 14:00:00'),
(7, 3, '2026-10-18 11:20:00'),
(8, 1, '2026-10-19 08:45:00'),
(8, 3, '2026-10-20 16:30:00');

-- HABILIDADES
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

-- HABILIDADES EXIGIDAS PELAS VAGAS
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

-- HABILIDADES DOS CANDIDATOS
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

-- QUESTÕES
INSERT INTO questao
(questao_id, texto_questao, classsificacao_questao, tipo_questao, vaga_id)
VALUES
(1, 'Você possui experiência profissional com C# e .NET?', 'tecnica', 'verdadeiro_falso', 1),
(2, 'Qual tecnologia é utilizada para mapear objetos em bancos relacionais no .NET?', 'tecnica', 'unica_escolha', 1),
(3, 'Quais práticas são importantes para desenvolver uma API REST?', 'tecnica', 'multipla_escolha', 1),
(4, 'Descreva como você resolveria um problema de lentidão em uma consulta SQL.', 'tecnica', 'dissertativa', 2),
(5, 'Como você lidaria com um prazo curto e várias demandas urgentes?', 'comportamental', 'dissertativa', 1),
(6, 'Um cliente relata um erro crítico no sistema hospitalar. Qual seria sua primeira atitude?', 'situacional', 'unica_escolha', 3);

-- ALTERNATIVAS
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

-- RESPOSTAS DOS CANDIDATOS
INSERT INTO candidato_questao
(questao_id, candidato_id, reposta_questao)
VALUES
(1, 5, '{"resposta": true}'::JSONB),
(2, 5, '{"resposta": "Entity Framework Core"}'::JSONB),
(3, 5, '{"respostas": ["Utilizar códigos de status HTTP adequados", "Validar os dados recebidos", "Documentar os endpoints"]}'::JSONB),
(4, 6, '{"resposta": "Analisaria o plano de execução, verificaria índices e avaliaria a necessidade de otimizar a consulta."}'::JSONB),
(5, 5, '{"resposta": "Priorizaria as demandas por impacto e urgência, alinhando expectativas com a equipe."}'::JSONB),
(6, 7, '{"resposta": "Investigar a falha, avaliar o impacto e comunicar os envolvidos"}'::JSONB);

-- PROCESSOS SELETIVOS
INSERT INTO processo_seletivo
(processo_seletivo_id, titulo_processo, data_abertura, data_fechamento, status_processo_seletivo, vaga_id)
VALUES
(1, 'Processo Seletivo - Backend Pleno', '2026-10-01', '2026-12-31', TRUE, 1),
(2, 'Processo Seletivo - Analista de Dados', '2026-10-05', '2026-12-20', TRUE, 2),
(3, 'Processo Seletivo - Sistemas Hospitalares', '2026-10-10', '2027-01-15', TRUE, 3);

-- CANDIDATOS NOS PROCESSOS SELETIVOS
INSERT INTO candidato_processo_seletivo
(processo_seletivo_id, candidato_id, status_relacao)
VALUES
(1, 5, 'avanca'),
(1, 7, 'considera'),
(1, 8, 'nao_avanca'),
(2, 6, 'avanca'),
(3, 7, 'avanca'),
(3, 8, 'considera');

-- ENTREVISTAS
INSERT INTO entrevista
(entrevista_id, nome_entrevista, horario_inicio, horario_fim, status_entrevista, processo_seletivo_id)
VALUES
(1, 'Entrevista Técnica - Backend', '2026-11-10 10:00:00', '2026-11-10 11:00:00', TRUE, 1),
(2, 'Entrevista com RH - Dados', '2026-11-11 14:00:00', '2026-11-11 14:45:00', TRUE, 2),
(3, 'Entrevista Técnica - Sistemas Hospitalares', '2026-11-12 09:00:00', '2026-11-12 10:00:00', TRUE, 3);

-- ENTREVISTADORES
INSERT INTO entrevistador_entrevista (gestor_id, entrevista_id) VALUES
(3, 1),
(3, 2),
(4, 3);

-- CANDIDATOS NAS ENTREVISTAS
INSERT INTO candidato_entrevista (candidato_id, entrevista_id) VALUES
(5, 1),
(6, 2),
(7, 3);

-- COMENTÁRIOS DAS ENTREVISTAS
INSERT INTO comentario
(comentario_id, texto, data_comentario, gestor_id, entrevista_id)
VALUES
(1, 'Candidato demonstrou domínio de C# e conhecimentos consistentes em .NET.', '2026-11-10 11:10:00', 3, 1),
(2, 'Apresentou boa capacidade analítica e experiência com SQL.', '2026-11-11 14:50:00', 3, 2),
(3, 'Demonstrou boa compreensão de sistemas e integrações.', '2026-11-12 10:10:00', 4, 3);

-- AVALIAÇÕES DE HABILIDADES NAS ENTREVISTAS
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

-- INFORMAÇÕES DOS CANDIDATOS
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

-- CURRÍCULOS
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
-- ATUALIZAR SEQUÊNCIAS SERIAL
SELECT setval(pg_get_serial_sequence('empresa', 'empresa_id'), COALESCE(MAX(empresa_id), 1)) FROM empresa;
SELECT setval(pg_get_serial_sequence('usuario', 'usuario_id'), COALESCE(MAX(usuario_id), 1)) FROM usuario;
SELECT setval(pg_get_serial_sequence('experiencia_profissional', 'experiencia_profissional_id'), COALESCE(MAX(experiencia_profissional_id), 1)) FROM experiencia_profissional;
SELECT setval(pg_get_serial_sequence('anotacao', 'anotacao_id'), COALESCE(MAX(anotacao_id), 1)) FROM anotacao;
SELECT setval(pg_get_serial_sequence('area_ocupacao', 'area_ocupacao_id'), COALESCE(MAX(area_ocupacao_id), 1)) FROM area_ocupacao;
SELECT setval(pg_get_serial_sequence('setor_ocupacao', 'setor_ocupacao_id'), COALESCE(MAX(setor_ocupacao_id), 1)) FROM setor_ocupacao;
SELECT setval(pg_get_serial_sequence('vaga', 'vaga_id'), COALESCE(MAX(vaga_id), 1)) FROM vaga;
SELECT setval(pg_get_serial_sequence('habilidade', 'habilidade_id'), COALESCE(MAX(habilidade_id), 1)) FROM habilidade;
SELECT setval(pg_get_serial_sequence('questao', 'questao_id'), COALESCE(MAX(questao_id), 1)) FROM questao;
SELECT setval(pg_get_serial_sequence('alternativa', 'alternativa_id'), COALESCE(MAX(alternativa_id), 1)) FROM alternativa;
SELECT setval(pg_get_serial_sequence('processo_seletivo', 'processo_seletivo_id'), COALESCE(MAX(processo_seletivo_id), 1)) FROM processo_seletivo;
SELECT setval(pg_get_serial_sequence('entrevista', 'entrevista_id'), COALESCE(MAX(entrevista_id), 1)) FROM entrevista;
SELECT setval(pg_get_serial_sequence('comentario', 'comentario_id'), COALESCE(MAX(comentario_id), 1)) FROM comentario;
SELECT setval(pg_get_serial_sequence('informacao_candidato', 'info_candidato_id'), COALESCE(MAX(info_candidato_id), 1)) FROM informacao_candidato;
SELECT setval(pg_get_serial_sequence('curriculo_candidato', 'curriculo_candidato_id'), COALESCE(MAX(curriculo_candidato_id), 1)) FROM curriculo_candidato;

COMMIT;
