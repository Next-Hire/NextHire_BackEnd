CREATE DATABASE IF NOT EXISTS nexthire_db;
CREATE SCHEMA IF NOT EXISTS general;

CREATE TYPE tipo_usuario_enum AS ENUM('candidato', 'gestor', 'analista_rh', 'adm');
CREATE TYPE tipo_questao_enum AS ENUM('verdadeiro_falso', 'unica_escolha', 'multipla_escolha', 'dissertativa');
CREATE TYPE classificacao_questao_enum AS ENUM('comportamental', 'tecnica', 'situacional');
CREATE TYPE status_relacao_enum AS ENUM('avanca', 'nao_avanca', 'considera');
CREATE TYPE nivel_prioridade_enum AS ENUM('alta', 'media', 'baixa');
CREATE TYPE escala_trabalho_enum AS ENUM('12x36', '5x2', '4x3', '15x15', '30x30');
CREATE TYPE modelo_trabalo_enum AS ENUM('hibrido', 'presencial', 'remoto');


 CREATE TABLE IF NOT EXISTS empresa (
    empresa_id SERIAL PRIMARY KEY,
    nome TEXT NOT NULL,
    status_empresa BOOLEAN DEFAULT true
);

CREATE TABLE IF NOT EXISTS usuario(
    usuario_id SERIAL PRIMARY KEY,
    nome TEXT NOT NULL,
    email VARCHAR(100) NOT NULL,
    senha VARCHAR(max) NOT NULL,
    primeiro_acesso BOOLEAN DEFAULT true,
    status_usuario BOOLEAN DEFAULT true,
    tipo_usuario tipo_usuario_enum NOT NULL

);

CREATE TABLE IF NOT EXISTS anotacao (
    anotacao_id SERIAL PRIMARY KEY,
    comentario TEXT NOT NULL,
    candidato_id INTEGER NOT NULL REFERENCES(usuario.usuario_id),
    gestor_id INTEGER NOT NULL REFERENCES(usuario.usuario_id)
);


CREATE TABLE IF NOT EXISTS empresa_funcionario (
    empresa_id INTEGER NOT NULL REFERENCES(usuario.usuario_id) NOT NULL,
    funcionario_id INTEGER NOT NULL REFERENCES(usuario.usuario_id) NOT NULL,
    

    CONSTRAINT pk_empresa_funcionario PRIMARY KEY(empresa_id, funcionario_id)
);

CREATE TABLE IF NOT EXISTS area_ocupacao (
    area_ocupacao_id SERIAL PRIMARY KEY,
    nome TEXT NOT NULL

);

CREATE TABLE IF NOT EXISTS gestor_area (
    gestor_id INTEGER NOT NULL REFERENCES(usuario.usuario_id),
    area_ocupacao_id INTEGER NOT NULL REFERENCES(area_ocupacao.area_ocupacao_id),

    CONSTRAINT pk_gestor_area PRIMARY KEY(gestor_id, area_ocupacao_id)

);

CREATE TABLE IF NOT EXISTS setor_ocupacao (
    setor_ocupacao_id SERIAL PRIMARY KEY,
    nome TEXT NOT NULL,
    empresa_id INTEGER NOT NULL REFERENCES(empresa.empresa_id),
    area_ocupacao_id INTEGER NOT NULL REFERENCES (area_ocupacao.area_ocupacao_id)
);

CREATE TABLE IF NOT EXISTS gestor_setor (
    gestor_id INTEGER NOT NULL REFERENCES(usuario.usuario_id),
    setor_ocupacao_id INTEGER NOT NULL REFERENCES(setor_ocupacao.setor_ocupacao_id),

    CONSTRAINT pk_gestor_setor PRIMARY KEY(gestor_id, setor_ocupacao_id)
);

CREATE TABLE IF NOT EXISTS vaga  (
    vaga_id SERIAL PRIMARY KEY,
    titulo TEXT NOT NULL,
    data_abertura TIMESTAMP DEFAULT CLOCK_TIMESTAMP(),
    data_fechamento TIMESTAMP NOT NULL,
    descricao TEXT NOT NULL,
    status_vaga BOOLEAN DEFAULT true,
    
    escala_vaga  escala_trabalho_enum NOT NULL,
    modelo_trabalho_vaga modelo_trabalo_enum NOT NULL,
    nivel_prioridade_vaga nivel_prioridade_enum NOT NULL,

    empresa_id INTEGER NOT NULL REFERENCES(empresa.empresa_id),
    analista_rh INTEGER NOT NULL REFERENCES(usuario.usuario_id),
);


CREATE TABLE IF NOT EXISTS gestor_vaga(
    gestor_id INTEGER NOT NULL REFERENCES(usuario.usuario_id),
    vaga_id INTEGER NOT NULL REFERENCES(vaga.vaga_id),

    CONSTRAINT pk_gestor_vaga PRIMARY KEY(gestor_id, vaga_id)
);

CREATE TABLE IF NOT EXISTS  candidato_vaga (
    candidato_id INTEGER NOT NULL REFERENCES(usuario.usuario_id),
    vaga_id INTEGER NOT NULL REFERENCES(vaga.vaga_id),
    data_hora_inscricao TIMESTAMP DEFAULT CLOCK_TIMESTAMP(),

    CONSTRAINT pk_candidato_vaga PRIMARY KEY (candidato_id, vaga_id)

);



CREATE TABLE IF NOT EXISTS habilidade (
    habilidade_id SERIAL PRIMARY KEY,
    nome TEXT NOT NULL
);

CREATE TABLE IF NOT EXISTS habilidade_vaga  (
    habilidade_id INTEGER NOT NULL REFERENCES(habilidade.habilidade_id),
    vaga_id INTEGER NOT NULL REFERENCES(vaga.vaga_id),

    CONSTRAINT pk_habilidade_vaga PRIMARY KEY(habilidade_id, vaga_id)
);

CREATE TABLE IF NOT EXISTS habilidade_candidato (
    habilidade_id INTEGER NOT NULL REFERENCES(habilidade.habilidade_id),
    candidato_id INTEGER NOT NULL REFERENCES(candidato.candidato_id),
    nivel INTEGER NOT NULL,

    CONSTRAINT pk_habilidade_candidato PRIMARY KEY(habilidade_id, candidato_id)
    
);

CREATE TABLE IF NOT EXISTS questao (
    questao_id SERIAL PRIMARY KEY,
    texto_questao TEXT NOT NULL,
    classsificacao_questao classificacao_questao_enum NOT NULL,
    tipo_questao tipo_questao_enum NOT NULL,

    vaga_id INTEGER NOT NULL REFERENCES (vaga.vaga_id)
);

CREATE TABLE IF NOT EXISTS alternativa (
    alternativa_id SERIAL PRIMARY KEY,
    texto_alternativa TEXT NOT NULL,
    alternativa_correta BOOLEAN NOT NULL,
    questao_id INTEGER NOT NULL REFERENCES(questao.questao_id)

);

--


CREATE TABLE IF NOT EXISTS alternativa(
    alternativa_id SERIAL PRIMARY KEY,
    texto TEXT NOT NULL,
    alternativa_correta BOOLEAN NOT NULL
    
);




CREATE TABLE IF NOT EXISTS  (

);

CREATE TABLE IF NOT EXISTS (

);

CREATE TABLE IF NOT EXISTS (

);