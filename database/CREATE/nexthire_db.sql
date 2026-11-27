--
-- PostgreSQL database dump
--

\restrict MukRPzKYgcuQQ8xjvooehouYrCFbDeOVcxGoRvakCtdrcdG4mxNmCP51pt8J4db

-- Dumped from database version 18.7 (Debian 18.7-1.pgdg13+5)
-- Dumped by pg_dump version 18.7 (Debian 18.7-1.pgdg13+5)

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET transaction_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

--
-- Name: classificacao_questao_enum; Type: TYPE; Schema: public; Owner: -
--

CREATE TYPE public.classificacao_questao_enum AS ENUM (
    'comportamental',
    'tecnica',
    'situacional'
);


--
-- Name: escala_trabalho_enum; Type: TYPE; Schema: public; Owner: -
--

CREATE TYPE public.escala_trabalho_enum AS ENUM (
    '12x36',
    '5x2',
    '4x3',
    '15x15',
    '30x30'
);


--
-- Name: modelo_trabalho_enum; Type: TYPE; Schema: public; Owner: -
--

CREATE TYPE public.modelo_trabalho_enum AS ENUM (
    'hibrido',
    'presencial',
    'remoto'
);


--
-- Name: modelo_vaga_enum; Type: TYPE; Schema: public; Owner: -
--

CREATE TYPE public.modelo_vaga_enum AS ENUM (
    'hibrido',
    'presencial',
    'semi-presencial'
);


--
-- Name: nivel_prioridade_enum; Type: TYPE; Schema: public; Owner: -
--

CREATE TYPE public.nivel_prioridade_enum AS ENUM (
    'alta',
    'media',
    'baixa'
);


--
-- Name: prioridade_vaga_enum; Type: TYPE; Schema: public; Owner: -
--

CREATE TYPE public.prioridade_vaga_enum AS ENUM (
    'baixo',
    'medio',
    'alto'
);


--
-- Name: situacao_profissional_enum; Type: TYPE; Schema: public; Owner: -
--

CREATE TYPE public.situacao_profissional_enum AS ENUM (
    'empregado',
    'desempregado'
);


--
-- Name: status_relacao_enum; Type: TYPE; Schema: public; Owner: -
--

CREATE TYPE public.status_relacao_enum AS ENUM (
    'avanca',
    'nao_avanca',
    'considera'
);


--
-- Name: tipo_questao_enum; Type: TYPE; Schema: public; Owner: -
--

CREATE TYPE public.tipo_questao_enum AS ENUM (
    'verdadeiro_falso',
    'unica_escolha',
    'multipla_escolha',
    'dissertativa'
);


--
-- Name: tipo_usuario_enum; Type: TYPE; Schema: public; Owner: -
--

CREATE TYPE public.tipo_usuario_enum AS ENUM (
    'candidato',
    'gestor',
    'analista_rh',
    'adm'
);


--
-- Name: tipo_vaga_enum; Type: TYPE; Schema: public; Owner: -
--

CREATE TYPE public.tipo_vaga_enum AS ENUM (
    'nova_funcao',
    'substituicao',
    'expansao_equipe'
);


SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- Name: alternativa; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.alternativa (
    alternativa_id integer NOT NULL,
    texto_alternativa text NOT NULL,
    alternativa_correta boolean NOT NULL,
    questao_id integer NOT NULL
);


--
-- Name: alternativa_alternativa_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.alternativa_alternativa_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: alternativa_alternativa_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.alternativa_alternativa_id_seq OWNED BY public.alternativa.alternativa_id;


--
-- Name: anotacao; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.anotacao (
    anotacao_id integer NOT NULL,
    conteudo text NOT NULL,
    candidato_id integer NOT NULL,
    gestor_id integer NOT NULL
);


--
-- Name: anotacao_anotacao_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.anotacao_anotacao_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: anotacao_anotacao_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.anotacao_anotacao_id_seq OWNED BY public.anotacao.anotacao_id;


--
-- Name: area_ocupacao; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.area_ocupacao (
    area_ocupacao_id integer NOT NULL,
    nome text NOT NULL
);


--
-- Name: area_ocupacao_area_ocupacao_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.area_ocupacao_area_ocupacao_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: area_ocupacao_area_ocupacao_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.area_ocupacao_area_ocupacao_id_seq OWNED BY public.area_ocupacao.area_ocupacao_id;


--
-- Name: avaliacao_habilidade; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.avaliacao_habilidade (
    habilidade_id integer NOT NULL,
    candidato_id integer NOT NULL,
    entrevista_id integer NOT NULL,
    nivel_avaliado integer NOT NULL
);


--
-- Name: candidato_entrevista; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.candidato_entrevista (
    candidato_id integer NOT NULL,
    entrevista_id integer NOT NULL
);


--
-- Name: candidato_processo_seletivo; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.candidato_processo_seletivo (
    processo_seletivo_id integer NOT NULL,
    candidato_id integer NOT NULL,
    status_relacao public.status_relacao_enum NOT NULL
);


--
-- Name: candidato_questao; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.candidato_questao (
    questao_id integer NOT NULL,
    candidato_id integer NOT NULL,
    reposta_questao jsonb NOT NULL
);


--
-- Name: comentario; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.comentario (
    comentario_id integer NOT NULL,
    texto text NOT NULL,
    data_comentario timestamp without time zone DEFAULT CURRENT_TIMESTAMP,
    gestor_id integer NOT NULL,
    entrevista_id integer NOT NULL
);


--
-- Name: comentario_comentario_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.comentario_comentario_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: comentario_comentario_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.comentario_comentario_id_seq OWNED BY public.comentario.comentario_id;


--
-- Name: curriculo_candidato; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.curriculo_candidato (
    curriculo_candidato_id integer NOT NULL,
    curriculo_convertido jsonb NOT NULL,
    candidato_id integer NOT NULL
);


--
-- Name: curriculo_candidato_curriculo_candidato_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.curriculo_candidato_curriculo_candidato_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: curriculo_candidato_curriculo_candidato_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.curriculo_candidato_curriculo_candidato_id_seq OWNED BY public.curriculo_candidato.curriculo_candidato_id;


--
-- Name: curriculo_vaga; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.curriculo_vaga (
    curriculo_id integer NOT NULL,
    vaga_id integer NOT NULL,
    data_hora_inscricao timestamp without time zone DEFAULT CURRENT_TIMESTAMP
);


--
-- Name: empresa; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.empresa (
    empresa_id integer NOT NULL,
    nome text NOT NULL,
    status_empresa boolean DEFAULT true
);


--
-- Name: empresa_empresa_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.empresa_empresa_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: empresa_empresa_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.empresa_empresa_id_seq OWNED BY public.empresa.empresa_id;


--
-- Name: empresa_funcionario; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.empresa_funcionario (
    empresa_id integer NOT NULL,
    funcionario_id integer NOT NULL
);


--
-- Name: entrevista; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.entrevista (
    entrevista_id integer NOT NULL,
    nome_entrevista text NOT NULL,
    horario_inicio timestamp without time zone DEFAULT CURRENT_TIMESTAMP,
    horario_fim timestamp without time zone NOT NULL,
    status_entrevista boolean DEFAULT true,
    processo_seletivo_id integer NOT NULL,
    entrevista_link character varying
);


--
-- Name: entrevista_entrevista_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.entrevista_entrevista_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: entrevista_entrevista_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.entrevista_entrevista_id_seq OWNED BY public.entrevista.entrevista_id;


--
-- Name: entrevistador_entrevista; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.entrevistador_entrevista (
    gestor_id integer NOT NULL,
    entrevista_id integer NOT NULL
);


--
-- Name: experiencia_profissional; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.experiencia_profissional (
    experiencia_profissional_id integer NOT NULL,
    nome_empresa character varying,
    cargo character varying,
    data_entrada date NOT NULL,
    data_saida date,
    atualmente_trabalho boolean DEFAULT true,
    usuario_id integer NOT NULL
);


--
-- Name: experiencia_profissional_experiencia_profissional_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.experiencia_profissional_experiencia_profissional_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: experiencia_profissional_experiencia_profissional_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.experiencia_profissional_experiencia_profissional_id_seq OWNED BY public.experiencia_profissional.experiencia_profissional_id;


--
-- Name: gestor_setor; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.gestor_setor (
    gestor_id integer NOT NULL,
    setor_ocupacao_id integer NOT NULL
);


--
-- Name: gestor_vaga; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.gestor_vaga (
    gestor_id integer NOT NULL,
    vaga_id integer NOT NULL
);


--
-- Name: habilidade; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.habilidade (
    habilidade_id integer NOT NULL,
    nome text NOT NULL,
    codigo character varying NOT NULL
);


--
-- Name: habilidade_candidato; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.habilidade_candidato (
    habilidade_id integer NOT NULL,
    candidato_id integer NOT NULL,
    nivel integer NOT NULL
);


--
-- Name: habilidade_habilidade_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.habilidade_habilidade_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: habilidade_habilidade_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.habilidade_habilidade_id_seq OWNED BY public.habilidade.habilidade_id;


--
-- Name: habilidade_vaga; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.habilidade_vaga (
    habilidade_id integer NOT NULL,
    vaga_id integer NOT NULL,
    nivel_habilidade integer
);


--
-- Name: informacao_candidato; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.informacao_candidato (
    info_candidato_id integer NOT NULL,
    telefone character varying NOT NULL,
    data_nascimento date NOT NULL,
    resumo text NOT NULL,
    candidato_id integer NOT NULL,
    situacao_profissional public.situacao_profissional_enum NOT NULL,
    formacao_academica jsonb NOT NULL,
    idioma jsonb NOT NULL
);


--
-- Name: informacao_candidato_info_candidato_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.informacao_candidato_info_candidato_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: informacao_candidato_info_candidato_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.informacao_candidato_info_candidato_id_seq OWNED BY public.informacao_candidato.info_candidato_id;


--
-- Name: processo_seletivo; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.processo_seletivo (
    processo_seletivo_id integer NOT NULL,
    titulo_processo text NOT NULL,
    data_abertura timestamp without time zone DEFAULT CURRENT_TIMESTAMP,
    data_fechamento timestamp without time zone NOT NULL,
    status_processo_seletivo boolean DEFAULT true,
    vaga_id integer NOT NULL
);


--
-- Name: processo_seletivo_processo_seletivo_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.processo_seletivo_processo_seletivo_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: processo_seletivo_processo_seletivo_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.processo_seletivo_processo_seletivo_id_seq OWNED BY public.processo_seletivo.processo_seletivo_id;


--
-- Name: questao; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.questao (
    questao_id integer NOT NULL,
    texto_questao text NOT NULL,
    classsificacao_questao public.classificacao_questao_enum NOT NULL,
    tipo_questao public.tipo_questao_enum NOT NULL,
    vaga_id integer NOT NULL
);


--
-- Name: questao_questao_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.questao_questao_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: questao_questao_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.questao_questao_id_seq OWNED BY public.questao.questao_id;


--
-- Name: setor_ocupacao; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.setor_ocupacao (
    setor_ocupacao_id integer NOT NULL,
    nome text NOT NULL,
    empresa_id integer NOT NULL,
    area_ocupacao_id integer NOT NULL
);


--
-- Name: setor_ocupacao_setor_ocupacao_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.setor_ocupacao_setor_ocupacao_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: setor_ocupacao_setor_ocupacao_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.setor_ocupacao_setor_ocupacao_id_seq OWNED BY public.setor_ocupacao.setor_ocupacao_id;


--
-- Name: usuario; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.usuario (
    usuario_id integer NOT NULL,
    nome text NOT NULL,
    email character varying NOT NULL,
    senha character varying,
    primeiro_acesso boolean DEFAULT true,
    status_usuario boolean DEFAULT true,
    tipo_usuario public.tipo_usuario_enum NOT NULL,
    is_oauth boolean DEFAULT false
);


--
-- Name: usuario_area; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.usuario_area (
    usuario_id integer NOT NULL,
    area_ocupacao_id integer NOT NULL
);


--
-- Name: usuario_usuario_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.usuario_usuario_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: usuario_usuario_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.usuario_usuario_id_seq OWNED BY public.usuario.usuario_id;


--
-- Name: vaga; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.vaga (
    vaga_id integer NOT NULL,
    titulo text NOT NULL,
    data_abertura timestamp without time zone DEFAULT CURRENT_TIMESTAMP,
    data_fechamento timestamp without time zone NOT NULL,
    descricao text NOT NULL,
    remuneracao numeric(10,2) NOT NULL,
    idiomas_desejados jsonb NOT NULL,
    tempo_experiencia_desejado numeric(10,2),
    escala_vaga public.escala_trabalho_enum NOT NULL,
    status_vaga boolean DEFAULT true,
    modelo_trabalho_vaga public.modelo_trabalho_enum NOT NULL,
    nivel_prioridade_vaga public.nivel_prioridade_enum NOT NULL,
    tipo_vaga public.tipo_vaga_enum NOT NULL,
    empresa_id integer NOT NULL,
    analista_rh integer NOT NULL,
    setor_id integer NOT NULL
);


--
-- Name: vaga_vaga_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.vaga_vaga_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: vaga_vaga_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.vaga_vaga_id_seq OWNED BY public.vaga.vaga_id;


--
-- Name: alternativa alternativa_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.alternativa ALTER COLUMN alternativa_id SET DEFAULT nextval('public.alternativa_alternativa_id_seq'::regclass);


--
-- Name: anotacao anotacao_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.anotacao ALTER COLUMN anotacao_id SET DEFAULT nextval('public.anotacao_anotacao_id_seq'::regclass);


--
-- Name: area_ocupacao area_ocupacao_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.area_ocupacao ALTER COLUMN area_ocupacao_id SET DEFAULT nextval('public.area_ocupacao_area_ocupacao_id_seq'::regclass);


--
-- Name: comentario comentario_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.comentario ALTER COLUMN comentario_id SET DEFAULT nextval('public.comentario_comentario_id_seq'::regclass);


--
-- Name: curriculo_candidato curriculo_candidato_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.curriculo_candidato ALTER COLUMN curriculo_candidato_id SET DEFAULT nextval('public.curriculo_candidato_curriculo_candidato_id_seq'::regclass);


--
-- Name: empresa empresa_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.empresa ALTER COLUMN empresa_id SET DEFAULT nextval('public.empresa_empresa_id_seq'::regclass);


--
-- Name: entrevista entrevista_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.entrevista ALTER COLUMN entrevista_id SET DEFAULT nextval('public.entrevista_entrevista_id_seq'::regclass);


--
-- Name: experiencia_profissional experiencia_profissional_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.experiencia_profissional ALTER COLUMN experiencia_profissional_id SET DEFAULT nextval('public.experiencia_profissional_experiencia_profissional_id_seq'::regclass);


--
-- Name: habilidade habilidade_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.habilidade ALTER COLUMN habilidade_id SET DEFAULT nextval('public.habilidade_habilidade_id_seq'::regclass);


--
-- Name: informacao_candidato info_candidato_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.informacao_candidato ALTER COLUMN info_candidato_id SET DEFAULT nextval('public.informacao_candidato_info_candidato_id_seq'::regclass);


--
-- Name: processo_seletivo processo_seletivo_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.processo_seletivo ALTER COLUMN processo_seletivo_id SET DEFAULT nextval('public.processo_seletivo_processo_seletivo_id_seq'::regclass);


--
-- Name: questao questao_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.questao ALTER COLUMN questao_id SET DEFAULT nextval('public.questao_questao_id_seq'::regclass);


--
-- Name: setor_ocupacao setor_ocupacao_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.setor_ocupacao ALTER COLUMN setor_ocupacao_id SET DEFAULT nextval('public.setor_ocupacao_setor_ocupacao_id_seq'::regclass);


--
-- Name: usuario usuario_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.usuario ALTER COLUMN usuario_id SET DEFAULT nextval('public.usuario_usuario_id_seq'::regclass);


--
-- Name: vaga vaga_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.vaga ALTER COLUMN vaga_id SET DEFAULT nextval('public.vaga_vaga_id_seq'::regclass);


--
-- Name: alternativa alternativa_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.alternativa
    ADD CONSTRAINT alternativa_pkey PRIMARY KEY (alternativa_id);


--
-- Name: anotacao anotacao_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.anotacao
    ADD CONSTRAINT anotacao_pkey PRIMARY KEY (anotacao_id);


--
-- Name: area_ocupacao area_ocupacao_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.area_ocupacao
    ADD CONSTRAINT area_ocupacao_pkey PRIMARY KEY (area_ocupacao_id);


--
-- Name: comentario comentario_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.comentario
    ADD CONSTRAINT comentario_pkey PRIMARY KEY (comentario_id);


--
-- Name: curriculo_candidato curriculo_candidato_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.curriculo_candidato
    ADD CONSTRAINT curriculo_candidato_pkey PRIMARY KEY (curriculo_candidato_id);


--
-- Name: empresa empresa_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.empresa
    ADD CONSTRAINT empresa_pkey PRIMARY KEY (empresa_id);


--
-- Name: entrevista entrevista_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.entrevista
    ADD CONSTRAINT entrevista_pkey PRIMARY KEY (entrevista_id);


--
-- Name: experiencia_profissional experiencia_profissional_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.experiencia_profissional
    ADD CONSTRAINT experiencia_profissional_pkey PRIMARY KEY (experiencia_profissional_id);


--
-- Name: habilidade habilidade_codigo_key; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.habilidade
    ADD CONSTRAINT habilidade_codigo_key UNIQUE (codigo);


--
-- Name: habilidade habilidade_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.habilidade
    ADD CONSTRAINT habilidade_pkey PRIMARY KEY (habilidade_id);


--
-- Name: informacao_candidato informacao_candidato_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.informacao_candidato
    ADD CONSTRAINT informacao_candidato_pkey PRIMARY KEY (info_candidato_id);


--
-- Name: avaliacao_habilidade pk_avaliacao_habilidade; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.avaliacao_habilidade
    ADD CONSTRAINT pk_avaliacao_habilidade PRIMARY KEY (habilidade_id, candidato_id, entrevista_id);


--
-- Name: candidato_entrevista pk_candidato_entrevista; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.candidato_entrevista
    ADD CONSTRAINT pk_candidato_entrevista PRIMARY KEY (candidato_id, entrevista_id);


--
-- Name: candidato_processo_seletivo pk_candidato_processo_seletivo; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.candidato_processo_seletivo
    ADD CONSTRAINT pk_candidato_processo_seletivo PRIMARY KEY (processo_seletivo_id, candidato_id);


--
-- Name: candidato_questao pk_candidato_questao; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.candidato_questao
    ADD CONSTRAINT pk_candidato_questao PRIMARY KEY (questao_id, candidato_id);


--
-- Name: curriculo_vaga pk_curriculo_vaga; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.curriculo_vaga
    ADD CONSTRAINT pk_curriculo_vaga PRIMARY KEY (curriculo_id, vaga_id);


--
-- Name: empresa_funcionario pk_empresa_funcionario; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.empresa_funcionario
    ADD CONSTRAINT pk_empresa_funcionario PRIMARY KEY (empresa_id, funcionario_id);


--
-- Name: entrevistador_entrevista pk_entrevistador_entrevista; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.entrevistador_entrevista
    ADD CONSTRAINT pk_entrevistador_entrevista PRIMARY KEY (gestor_id, entrevista_id);


--
-- Name: gestor_setor pk_gestor_setor; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.gestor_setor
    ADD CONSTRAINT pk_gestor_setor PRIMARY KEY (gestor_id, setor_ocupacao_id);


--
-- Name: gestor_vaga pk_gestor_vaga; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.gestor_vaga
    ADD CONSTRAINT pk_gestor_vaga PRIMARY KEY (gestor_id, vaga_id);


--
-- Name: habilidade_candidato pk_habilidade_candidato; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.habilidade_candidato
    ADD CONSTRAINT pk_habilidade_candidato PRIMARY KEY (habilidade_id, candidato_id);


--
-- Name: habilidade_vaga pk_habilidade_vaga; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.habilidade_vaga
    ADD CONSTRAINT pk_habilidade_vaga PRIMARY KEY (habilidade_id, vaga_id);


--
-- Name: usuario_area pk_usuario_area; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.usuario_area
    ADD CONSTRAINT pk_usuario_area PRIMARY KEY (usuario_id, area_ocupacao_id);


--
-- Name: processo_seletivo processo_seletivo_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.processo_seletivo
    ADD CONSTRAINT processo_seletivo_pkey PRIMARY KEY (processo_seletivo_id);


--
-- Name: questao questao_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.questao
    ADD CONSTRAINT questao_pkey PRIMARY KEY (questao_id);


--
-- Name: setor_ocupacao setor_ocupacao_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.setor_ocupacao
    ADD CONSTRAINT setor_ocupacao_pkey PRIMARY KEY (setor_ocupacao_id);


--
-- Name: usuario usuario_email_key; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.usuario
    ADD CONSTRAINT usuario_email_key UNIQUE (email);


--
-- Name: usuario usuario_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.usuario
    ADD CONSTRAINT usuario_pkey PRIMARY KEY (usuario_id);


--
-- Name: vaga vaga_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.vaga
    ADD CONSTRAINT vaga_pkey PRIMARY KEY (vaga_id);


--
-- Name: alternativa alternativa_questao_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.alternativa
    ADD CONSTRAINT alternativa_questao_id_fkey FOREIGN KEY (questao_id) REFERENCES public.questao(questao_id);


--
-- Name: anotacao anotacao_candidato_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.anotacao
    ADD CONSTRAINT anotacao_candidato_id_fkey FOREIGN KEY (candidato_id) REFERENCES public.usuario(usuario_id);


--
-- Name: anotacao anotacao_gestor_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.anotacao
    ADD CONSTRAINT anotacao_gestor_id_fkey FOREIGN KEY (gestor_id) REFERENCES public.usuario(usuario_id);


--
-- Name: avaliacao_habilidade avaliacao_habilidade_candidato_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.avaliacao_habilidade
    ADD CONSTRAINT avaliacao_habilidade_candidato_id_fkey FOREIGN KEY (candidato_id) REFERENCES public.usuario(usuario_id);


--
-- Name: avaliacao_habilidade avaliacao_habilidade_entrevista_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.avaliacao_habilidade
    ADD CONSTRAINT avaliacao_habilidade_entrevista_id_fkey FOREIGN KEY (entrevista_id) REFERENCES public.entrevista(entrevista_id);


--
-- Name: avaliacao_habilidade avaliacao_habilidade_habilidade_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.avaliacao_habilidade
    ADD CONSTRAINT avaliacao_habilidade_habilidade_id_fkey FOREIGN KEY (habilidade_id) REFERENCES public.habilidade(habilidade_id);


--
-- Name: candidato_entrevista candidato_entrevista_candidato_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.candidato_entrevista
    ADD CONSTRAINT candidato_entrevista_candidato_id_fkey FOREIGN KEY (candidato_id) REFERENCES public.usuario(usuario_id);


--
-- Name: candidato_entrevista candidato_entrevista_entrevista_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.candidato_entrevista
    ADD CONSTRAINT candidato_entrevista_entrevista_id_fkey FOREIGN KEY (entrevista_id) REFERENCES public.entrevista(entrevista_id);


--
-- Name: candidato_processo_seletivo candidato_processo_seletivo_candidato_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.candidato_processo_seletivo
    ADD CONSTRAINT candidato_processo_seletivo_candidato_id_fkey FOREIGN KEY (candidato_id) REFERENCES public.usuario(usuario_id);


--
-- Name: candidato_processo_seletivo candidato_processo_seletivo_processo_seletivo_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.candidato_processo_seletivo
    ADD CONSTRAINT candidato_processo_seletivo_processo_seletivo_id_fkey FOREIGN KEY (processo_seletivo_id) REFERENCES public.processo_seletivo(processo_seletivo_id);


--
-- Name: candidato_questao candidato_questao_candidato_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.candidato_questao
    ADD CONSTRAINT candidato_questao_candidato_id_fkey FOREIGN KEY (candidato_id) REFERENCES public.usuario(usuario_id);


--
-- Name: candidato_questao candidato_questao_questao_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.candidato_questao
    ADD CONSTRAINT candidato_questao_questao_id_fkey FOREIGN KEY (questao_id) REFERENCES public.questao(questao_id);


--
-- Name: comentario comentario_entrevista_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.comentario
    ADD CONSTRAINT comentario_entrevista_id_fkey FOREIGN KEY (entrevista_id) REFERENCES public.entrevista(entrevista_id);


--
-- Name: comentario comentario_gestor_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.comentario
    ADD CONSTRAINT comentario_gestor_id_fkey FOREIGN KEY (gestor_id) REFERENCES public.usuario(usuario_id);


--
-- Name: curriculo_candidato curriculo_candidato_candidato_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.curriculo_candidato
    ADD CONSTRAINT curriculo_candidato_candidato_id_fkey FOREIGN KEY (candidato_id) REFERENCES public.usuario(usuario_id);


--
-- Name: curriculo_vaga curriculo_vaga_curriculo_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.curriculo_vaga
    ADD CONSTRAINT curriculo_vaga_curriculo_id_fkey FOREIGN KEY (curriculo_id) REFERENCES public.curriculo_candidato(curriculo_candidato_id);


--
-- Name: curriculo_vaga curriculo_vaga_vaga_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.curriculo_vaga
    ADD CONSTRAINT curriculo_vaga_vaga_id_fkey FOREIGN KEY (vaga_id) REFERENCES public.vaga(vaga_id);


--
-- Name: empresa_funcionario empresa_funcionario_empresa_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.empresa_funcionario
    ADD CONSTRAINT empresa_funcionario_empresa_id_fkey FOREIGN KEY (empresa_id) REFERENCES public.empresa(empresa_id);


--
-- Name: empresa_funcionario empresa_funcionario_funcionario_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.empresa_funcionario
    ADD CONSTRAINT empresa_funcionario_funcionario_id_fkey FOREIGN KEY (funcionario_id) REFERENCES public.usuario(usuario_id);


--
-- Name: entrevista entrevista_processo_seletivo_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.entrevista
    ADD CONSTRAINT entrevista_processo_seletivo_id_fkey FOREIGN KEY (processo_seletivo_id) REFERENCES public.processo_seletivo(processo_seletivo_id);


--
-- Name: entrevistador_entrevista entrevistador_entrevista_entrevista_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.entrevistador_entrevista
    ADD CONSTRAINT entrevistador_entrevista_entrevista_id_fkey FOREIGN KEY (entrevista_id) REFERENCES public.entrevista(entrevista_id);


--
-- Name: entrevistador_entrevista entrevistador_entrevista_gestor_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.entrevistador_entrevista
    ADD CONSTRAINT entrevistador_entrevista_gestor_id_fkey FOREIGN KEY (gestor_id) REFERENCES public.usuario(usuario_id);


--
-- Name: experiencia_profissional experiencia_profissional_usuario_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.experiencia_profissional
    ADD CONSTRAINT experiencia_profissional_usuario_id_fkey FOREIGN KEY (usuario_id) REFERENCES public.usuario(usuario_id);


--
-- Name: gestor_setor gestor_setor_gestor_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.gestor_setor
    ADD CONSTRAINT gestor_setor_gestor_id_fkey FOREIGN KEY (gestor_id) REFERENCES public.usuario(usuario_id);


--
-- Name: gestor_setor gestor_setor_setor_ocupacao_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.gestor_setor
    ADD CONSTRAINT gestor_setor_setor_ocupacao_id_fkey FOREIGN KEY (setor_ocupacao_id) REFERENCES public.setor_ocupacao(setor_ocupacao_id);


--
-- Name: gestor_vaga gestor_vaga_gestor_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.gestor_vaga
    ADD CONSTRAINT gestor_vaga_gestor_id_fkey FOREIGN KEY (gestor_id) REFERENCES public.usuario(usuario_id);


--
-- Name: gestor_vaga gestor_vaga_vaga_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.gestor_vaga
    ADD CONSTRAINT gestor_vaga_vaga_id_fkey FOREIGN KEY (vaga_id) REFERENCES public.vaga(vaga_id);


--
-- Name: habilidade_candidato habilidade_candidato_candidato_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.habilidade_candidato
    ADD CONSTRAINT habilidade_candidato_candidato_id_fkey FOREIGN KEY (candidato_id) REFERENCES public.usuario(usuario_id);


--
-- Name: habilidade_candidato habilidade_candidato_habilidade_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.habilidade_candidato
    ADD CONSTRAINT habilidade_candidato_habilidade_id_fkey FOREIGN KEY (habilidade_id) REFERENCES public.habilidade(habilidade_id);


--
-- Name: habilidade_vaga habilidade_vaga_habilidade_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.habilidade_vaga
    ADD CONSTRAINT habilidade_vaga_habilidade_id_fkey FOREIGN KEY (habilidade_id) REFERENCES public.habilidade(habilidade_id);


--
-- Name: habilidade_vaga habilidade_vaga_vaga_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.habilidade_vaga
    ADD CONSTRAINT habilidade_vaga_vaga_id_fkey FOREIGN KEY (vaga_id) REFERENCES public.vaga(vaga_id);


--
-- Name: informacao_candidato informacao_candidato_candidato_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.informacao_candidato
    ADD CONSTRAINT informacao_candidato_candidato_id_fkey FOREIGN KEY (candidato_id) REFERENCES public.usuario(usuario_id);


--
-- Name: processo_seletivo processo_seletivo_vaga_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.processo_seletivo
    ADD CONSTRAINT processo_seletivo_vaga_id_fkey FOREIGN KEY (vaga_id) REFERENCES public.vaga(vaga_id);


--
-- Name: questao questao_vaga_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.questao
    ADD CONSTRAINT questao_vaga_id_fkey FOREIGN KEY (vaga_id) REFERENCES public.vaga(vaga_id);


--
-- Name: setor_ocupacao setor_ocupacao_area_ocupacao_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.setor_ocupacao
    ADD CONSTRAINT setor_ocupacao_area_ocupacao_id_fkey FOREIGN KEY (area_ocupacao_id) REFERENCES public.area_ocupacao(area_ocupacao_id);


--
-- Name: setor_ocupacao setor_ocupacao_empresa_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.setor_ocupacao
    ADD CONSTRAINT setor_ocupacao_empresa_id_fkey FOREIGN KEY (empresa_id) REFERENCES public.empresa(empresa_id);


--
-- Name: usuario_area usuario_area_area_ocupacao_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.usuario_area
    ADD CONSTRAINT usuario_area_area_ocupacao_id_fkey FOREIGN KEY (area_ocupacao_id) REFERENCES public.area_ocupacao(area_ocupacao_id);


--
-- Name: usuario_area usuario_area_usuario_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.usuario_area
    ADD CONSTRAINT usuario_area_usuario_id_fkey FOREIGN KEY (usuario_id) REFERENCES public.usuario(usuario_id);


--
-- Name: vaga vaga_analista_rh_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.vaga
    ADD CONSTRAINT vaga_analista_rh_fkey FOREIGN KEY (analista_rh) REFERENCES public.usuario(usuario_id);


--
-- Name: vaga vaga_empresa_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.vaga
    ADD CONSTRAINT vaga_empresa_id_fkey FOREIGN KEY (empresa_id) REFERENCES public.empresa(empresa_id);


--
-- Name: vaga vaga_setor_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.vaga
    ADD CONSTRAINT vaga_setor_id_fkey FOREIGN KEY (setor_id) REFERENCES public.setor_ocupacao(setor_ocupacao_id);


--
-- PostgreSQL database dump complete
--

\unrestrict MukRPzKYgcuQQ8xjvooehouYrCFbDeOVcxGoRvakCtdrcdG4mxNmCP51pt8J4db

