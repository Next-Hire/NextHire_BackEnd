CREATE TYPE tipo_usuario_enum AS ENUM('candidato', 'gestor', 'analista_rh', 'adm');
CREATE TYPE tipo_questao_enum AS ENUM('verdadeiro_falso', 'unica_escolha', 'multipla_escolha', 'dissertativa');
CREATE TYPE classificacao_questao_enum AS ENUM('comportamental', 'tecnica', 'situacional');
CREATE TYPE status_relacao_enum AS ENUM('avanca', 'nao_avanca', 'considera');
CREATE TYPE nivel_prioridade_enum AS ENUM('alta', 'media', 'baixa');
CREATE TYPE escala_trabalho_enum AS ENUM('12x36', '5x2', '4x3', '15x15', '30x30');
CREATE TYPE modelo_trabalo_enum AS ENUM('hibrido', 'presencial', 'remoto');
CREATE TYPE situacao_profissional_enum AS ENUM ('empregado', 'desempregado');

CREATE TABLE IF NOT EXISTS empresa (
    empresa_id SERIAL PRIMARY KEY,
    nome TEXT NOT NULL,
    status_empresa BOOLEAN DEFAULT true
);

CREATE TABLE IF NOT EXISTS usuario(
    usuario_id SERIAL PRIMARY KEY,
    nome TEXT NOT NULL,
    email VARCHAR NOT NULL,
    senha VARCHAR NOT NULL,
    primeiro_acesso BOOLEAN DEFAULT true,
    status_usuario BOOLEAN DEFAULT true,
    tipo_usuario tipo_usuario_enum NOT NULL
);

CREATE TABLE IF NOT EXISTS experiencia_profissional (
    experiencia_profissional_id SERIAL PRIMARY KEY,
    nome_empresa VARCHAR,
    cargo VARCHAR,
    data_entrada DATE NOT NULL,
    data_saida DATE,
    atualmente_trabalho BOOLEAN DEFAULT true,
    usuario_id INTEGER NOT NULL REFERENCES usuario(usuario_id)
);

CREATE TABLE IF NOT EXISTS anotacao (
    anotacao_id SERIAL PRIMARY KEY,
    conteudo TEXT NOT NULL,
    candidato_id INTEGER NOT NULL REFERENCES usuario(usuario_id),
    gestor_id INTEGER NOT NULL REFERENCES usuario(usuario_id)
);

CREATE TABLE IF NOT EXISTS empresa_funcionario (
    empresa_id INTEGER NOT NULL REFERENCES empresa(empresa_id),
    funcionario_id INTEGER NOT NULL REFERENCES usuario(usuario_id),
    CONSTRAINT pk_empresa_funcionario PRIMARY KEY(empresa_id, funcionario_id)
);

CREATE TABLE IF NOT EXISTS area_ocupacao (
    area_ocupacao_id SERIAL PRIMARY KEY,
    nome TEXT NOT NULL
);

CREATE TABLE IF NOT EXISTS usuario_area (
    usuario_id INTEGER NOT NULL REFERENCES usuario(usuario_id),
    area_ocupacao_id INTEGER NOT NULL REFERENCES area_ocupacao(area_ocupacao_id),
    CONSTRAINT pk_usuario_area PRIMARY KEY(usuario_id, area_ocupacao_id)
);

CREATE TABLE IF NOT EXISTS setor_ocupacao (
    setor_ocupacao_id SERIAL PRIMARY KEY,
    nome TEXT NOT NULL,
    empresa_id INTEGER NOT NULL REFERENCES empresa(empresa_id),
    area_ocupacao_id INTEGER NOT NULL REFERENCES area_ocupacao(area_ocupacao_id)
);

CREATE TABLE IF NOT EXISTS gestor_setor (
    gestor_id INTEGER NOT NULL REFERENCES usuario(usuario_id),
    setor_ocupacao_id INTEGER NOT NULL REFERENCES setor_ocupacao(setor_ocupacao_id),
    CONSTRAINT pk_gestor_setor PRIMARY KEY(gestor_id, setor_ocupacao_id)
);

CREATE TABLE IF NOT EXISTS vaga (
    vaga_id SERIAL PRIMARY KEY,
    titulo TEXT NOT NULL,
    data_abertura TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    data_fechamento TIMESTAMP NOT NULL,
    descricao TEXT NOT NULL,
    status_vaga BOOLEAN DEFAULT true,
    idiomas_desejados JSONB NOT NULL,
    tempo_experiencia_desejado NUMERIC(10,2),
    escala_vaga escala_trabalho_enum NOT NULL,
    modelo_trabalho_vaga modelo_trabalo_enum NOT NULL,
    nivel_prioridade_vaga nivel_prioridade_enum NOT NULL,
    empresa_id INTEGER NOT NULL REFERENCES empresa(empresa_id),
    analista_rh INTEGER NOT NULL REFERENCES usuario(usuario_id)
);

CREATE TABLE IF NOT EXISTS gestor_vaga(
    gestor_id INTEGER NOT NULL REFERENCES usuario(usuario_id),
    vaga_id INTEGER NOT NULL REFERENCES vaga(vaga_id),
    CONSTRAINT pk_gestor_vaga PRIMARY KEY(gestor_id, vaga_id)
);

CREATE TABLE IF NOT EXISTS candidato_vaga (
    candidato_id INTEGER NOT NULL REFERENCES usuario(usuario_id),
    vaga_id INTEGER NOT NULL REFERENCES vaga(vaga_id),
    data_hora_inscricao TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT pk_candidato_vaga PRIMARY KEY (candidato_id, vaga_id)
);

CREATE TABLE IF NOT EXISTS habilidade (
    habilidade_id SERIAL PRIMARY KEY,
    nome TEXT NOT NULL
);

CREATE TABLE IF NOT EXISTS habilidade_vaga (
    habilidade_id INTEGER NOT NULL REFERENCES habilidade(habilidade_id),
    vaga_id INTEGER NOT NULL REFERENCES vaga(vaga_id),
    CONSTRAINT pk_habilidade_vaga PRIMARY KEY(habilidade_id, vaga_id)
);

CREATE TABLE IF NOT EXISTS habilidade_candidato (
    habilidade_id INTEGER NOT NULL REFERENCES habilidade(habilidade_id),
    candidato_id INTEGER NOT NULL REFERENCES usuario(usuario_id),
    nivel INTEGER NOT NULL,
    CONSTRAINT pk_habilidade_candidato PRIMARY KEY(habilidade_id, candidato_id)
);

CREATE TABLE IF NOT EXISTS questao (
    questao_id SERIAL PRIMARY KEY,
    texto_questao TEXT NOT NULL,
    classsificacao_questao classificacao_questao_enum NOT NULL,
    tipo_questao tipo_questao_enum NOT NULL,
    vaga_id INTEGER NOT NULL REFERENCES vaga(vaga_id)
);

CREATE TABLE IF NOT EXISTS alternativa (
    alternativa_id SERIAL PRIMARY KEY,
    texto_alternativa TEXT NOT NULL,
    alternativa_correta BOOLEAN NOT NULL,
    questao_id INTEGER NOT NULL REFERENCES questao(questao_id)
);

CREATE TABLE IF NOT EXISTS candidato_questao (
    questao_id INTEGER NOT NULL REFERENCES questao(questao_id),
    candidato_id INTEGER NOT NULL REFERENCES usuario(usuario_id),
    reposta_questao JSONB NOT NULL,
    CONSTRAINT pk_candidato_questao PRIMARY KEY(questao_id, candidato_id)
);

CREATE TABLE IF NOT EXISTS processo_seletivo (
    processo_seletivo_id SERIAL PRIMARY KEY,
    titulo_processo TEXT NOT NULL,
    data_abertura TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    data_fechamento TIMESTAMP NOT NULL,
    status_processo_seletivo BOOLEAN DEFAULT TRUE,
    vaga_id INTEGER NOT NULL REFERENCES vaga(vaga_id)
);

CREATE TABLE IF NOT EXISTS candidato_processo_seletivo (
    processo_seletivo_id INTEGER NOT NULL REFERENCES processo_seletivo(processo_seletivo_id),
    candidato_id INTEGER NOT NULL REFERENCES usuario(usuario_id),
    status_relacao status_relacao_enum NOT NULL,
    CONSTRAINT pk_candidato_processo_seletivo PRIMARY KEY(processo_seletivo_id, candidato_id)
);

CREATE TABLE IF NOT EXISTS entrevista (
    entrevista_id SERIAL PRIMARY KEY NOT NULL,
    nome_entrevista TEXT NOT NULL,
    horario_inicio TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    horario_fim TIMESTAMP NOT NULL,
    status_entrevista BOOLEAN DEFAULT true,
    processo_seletivo_id INTEGER NOT NULL REFERENCES processo_seletivo(processo_seletivo_id)
);

CREATE TABLE IF NOT EXISTS entrevistador_entrevista (
    gestor_id INTEGER NOT NULL REFERENCES usuario(usuario_id),
    entrevista_id INTEGER NOT NULL REFERENCES entrevista(entrevista_id),
    CONSTRAINT pk_entrevistador_entrevista PRIMARY KEY(gestor_id, entrevista_id)
);

CREATE TABLE IF NOT EXISTS candidato_entrevista (
    candidato_id INTEGER NOT NULL REFERENCES usuario(usuario_id),
    entrevista_id INTEGER NOT NULL REFERENCES entrevista(entrevista_id),
    CONSTRAINT pk_candidato_entrevista PRIMARY KEY(candidato_id, entrevista_id)
);

CREATE TABLE IF NOT EXISTS comentario(
    comentario_id SERIAL PRIMARY KEY,
    texto TEXT NOT NULL,
    data_comentario TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    gestor_id INTEGER NOT NULL REFERENCES usuario(usuario_id),
    entrevista_id INTEGER NOT NULL REFERENCES entrevista(entrevista_id)
);

CREATE TABLE IF NOT EXISTS avaliacao_habilidade(
    habilidade_id INTEGER NOT NULL REFERENCES habilidade(habilidade_id),
    candidato_id INTEGER NOT NULL REFERENCES usuario(usuario_id),
    entrevista_id INTEGER NOT NULL REFERENCES entrevista(entrevista_id),
    nivel_avaliado INT NOT NULL
);

CREATE TABLE IF NOT EXISTS informacao_candidato(
    info_candidato_id SERIAL PRIMARY KEY,
    telefone VARCHAR NOT NULL,
    data_nascimento DATE NOT NULL,
    resumo TEXT NOT NULL,
    candidato_id INTEGER NOT NULL REFERENCES usuario(usuario_id),
    situacao_profissional situacao_profissional_enum NOT NULL,
    formacao_academica JSONB NOT NULL,
    idioma JSONB NOT NULL
);

CREATE TABLE IF NOT EXISTS curriculo_candidato (
    curriculo_candidato_id SERIAL PRIMARY KEY,
    curriculo_convertido JSONB NOT NULL,
    candidato_id INTEGER NOT NULL REFERENCES usuario(usuario_id)
);
