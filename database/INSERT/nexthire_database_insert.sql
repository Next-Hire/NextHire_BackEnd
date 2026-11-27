--
-- PostgreSQL database dump
--

\restrict PrYJ9744wApKLg2rMccNhFxcMEDBU16dpP3iMWg9Sv6AdmcXFxP8NsyURKVAcIG

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
-- Data for Name: area_ocupacao; Type: TABLE DATA; Schema: public; Owner: -
--

SET SESSION AUTHORIZATION DEFAULT;

ALTER TABLE public.area_ocupacao DISABLE TRIGGER ALL;

INSERT INTO public.area_ocupacao (area_ocupacao_id, nome) VALUES (1, 'Tecnologia da Informação');
INSERT INTO public.area_ocupacao (area_ocupacao_id, nome) VALUES (2, 'Recursos Humanos');
INSERT INTO public.area_ocupacao (area_ocupacao_id, nome) VALUES (3, 'Saúde e Tecnologia');
INSERT INTO public.area_ocupacao (area_ocupacao_id, nome) VALUES (4, 'Dados e Inteligência Artificial');
INSERT INTO public.area_ocupacao (area_ocupacao_id, nome) VALUES (5, 'Finanças e Bancos');
INSERT INTO public.area_ocupacao (area_ocupacao_id, nome) VALUES (6, 'Varejo e E-commerce');
INSERT INTO public.area_ocupacao (area_ocupacao_id, nome) VALUES (7, 'Indústria e Manufatura');
INSERT INTO public.area_ocupacao (area_ocupacao_id, nome) VALUES (8, 'Logística e Supply Chain');
INSERT INTO public.area_ocupacao (area_ocupacao_id, nome) VALUES (9, 'Construção Civil e Engenharia');
INSERT INTO public.area_ocupacao (area_ocupacao_id, nome) VALUES (10, 'Educação');
INSERT INTO public.area_ocupacao (area_ocupacao_id, nome) VALUES (11, 'Agronegócio');
INSERT INTO public.area_ocupacao (area_ocupacao_id, nome) VALUES (12, 'Energia e Saneamento');
INSERT INTO public.area_ocupacao (area_ocupacao_id, nome) VALUES (13, 'Marketing e Comunicação');
INSERT INTO public.area_ocupacao (area_ocupacao_id, nome) VALUES (14, 'Vendas e Desenvolvimento de Negócios');
INSERT INTO public.area_ocupacao (area_ocupacao_id, nome) VALUES (15, 'Administração e Gestão');
INSERT INTO public.area_ocupacao (area_ocupacao_id, nome) VALUES (16, 'Jurídico, Compliance e Privacidade');
INSERT INTO public.area_ocupacao (area_ocupacao_id, nome) VALUES (17, 'Atendimento e Experiência do Cliente');
INSERT INTO public.area_ocupacao (area_ocupacao_id, nome) VALUES (18, 'Design e Experiência do Usuário');
INSERT INTO public.area_ocupacao (area_ocupacao_id, nome) VALUES (19, 'Segurança da Informação e Cibersegurança');
INSERT INTO public.area_ocupacao (area_ocupacao_id, nome) VALUES (20, 'Sustentabilidade e ESG');
INSERT INTO public.area_ocupacao (area_ocupacao_id, nome) VALUES (21, 'Farmacêutico e Biotecnologia');
INSERT INTO public.area_ocupacao (area_ocupacao_id, nome) VALUES (22, 'Alimentação e Food Service');
INSERT INTO public.area_ocupacao (area_ocupacao_id, nome) VALUES (23, 'Turismo e Hospitalidade');
INSERT INTO public.area_ocupacao (area_ocupacao_id, nome) VALUES (24, 'Mercado Imobiliário');
INSERT INTO public.area_ocupacao (area_ocupacao_id, nome) VALUES (25, 'Automotivo e Mobilidade');
INSERT INTO public.area_ocupacao (area_ocupacao_id, nome) VALUES (26, 'Telecomunicações');
INSERT INTO public.area_ocupacao (area_ocupacao_id, nome) VALUES (27, 'Setor Público e Terceiro Setor');
INSERT INTO public.area_ocupacao (area_ocupacao_id, nome) VALUES (28, 'Mineração, Metalurgia e Papel');
INSERT INTO public.area_ocupacao (area_ocupacao_id, nome) VALUES (29, 'Química e Materiais');
INSERT INTO public.area_ocupacao (area_ocupacao_id, nome) VALUES (30, 'Beleza, Cosmética e Bem-estar');
INSERT INTO public.area_ocupacao (area_ocupacao_id, nome) VALUES (31, 'Consultoria e Serviços Profissionais');
INSERT INTO public.area_ocupacao (area_ocupacao_id, nome) VALUES (32, 'Facilities e Serviços Gerais');


ALTER TABLE public.area_ocupacao ENABLE TRIGGER ALL;

--
-- Data for Name: empresa; Type: TABLE DATA; Schema: public; Owner: -
--

ALTER TABLE public.empresa DISABLE TRIGGER ALL;

INSERT INTO public.empresa (empresa_id, nome, status_empresa) VALUES (1, 'TechNova Solutions', true);
INSERT INTO public.empresa (empresa_id, nome, status_empresa) VALUES (2, 'Inova Saúde Digital', true);
INSERT INTO public.empresa (empresa_id, nome, status_empresa) VALUES (3, 'Banco Horizonte', true);
INSERT INTO public.empresa (empresa_id, nome, status_empresa) VALUES (4, 'Varejo Prime', true);
INSERT INTO public.empresa (empresa_id, nome, status_empresa) VALUES (5, 'Indústria Aurora', true);
INSERT INTO public.empresa (empresa_id, nome, status_empresa) VALUES (6, 'Logística Rota Brasil', true);
INSERT INTO public.empresa (empresa_id, nome, status_empresa) VALUES (7, 'Construtora Marco', true);
INSERT INTO public.empresa (empresa_id, nome, status_empresa) VALUES (8, 'EducaMais', true);
INSERT INTO public.empresa (empresa_id, nome, status_empresa) VALUES (9, 'AgroVale', true);
INSERT INTO public.empresa (empresa_id, nome, status_empresa) VALUES (10, 'Energia Nova', true);
INSERT INTO public.empresa (empresa_id, nome, status_empresa) VALUES (11, 'Agência Pulsar', true);
INSERT INTO public.empresa (empresa_id, nome, status_empresa) VALUES (12, 'Grupo Venda Certa', true);
INSERT INTO public.empresa (empresa_id, nome, status_empresa) VALUES (13, 'FarmaVida', true);
INSERT INTO public.empresa (empresa_id, nome, status_empresa) VALUES (14, 'Sabores do Brasil', true);
INSERT INTO public.empresa (empresa_id, nome, status_empresa) VALUES (15, 'Instituto Cidadania', true);
INSERT INTO public.empresa (empresa_id, nome, status_empresa) VALUES (16, 'Advocacia Lima', true);
INSERT INTO public.empresa (empresa_id, nome, status_empresa) VALUES (17, 'Beleza Natural', true);
INSERT INTO public.empresa (empresa_id, nome, status_empresa) VALUES (18, 'Consultoria Estratégica', true);
INSERT INTO public.empresa (empresa_id, nome, status_empresa) VALUES (19, 'Telecom Conecta', true);
INSERT INTO public.empresa (empresa_id, nome, status_empresa) VALUES (20, 'Mobilidade Urbana', true);


ALTER TABLE public.empresa ENABLE TRIGGER ALL;

--
-- Data for Name: setor_ocupacao; Type: TABLE DATA; Schema: public; Owner: -
--

ALTER TABLE public.setor_ocupacao DISABLE TRIGGER ALL;

INSERT INTO public.setor_ocupacao (setor_ocupacao_id, nome, empresa_id, area_ocupacao_id) VALUES (1, 'Desenvolvimento de Software', 1, 1);
INSERT INTO public.setor_ocupacao (setor_ocupacao_id, nome, empresa_id, area_ocupacao_id) VALUES (2, 'Engenharia de Dados', 1, 4);
INSERT INTO public.setor_ocupacao (setor_ocupacao_id, nome, empresa_id, area_ocupacao_id) VALUES (3, 'Recursos Humanos', 2, 2);
INSERT INTO public.setor_ocupacao (setor_ocupacao_id, nome, empresa_id, area_ocupacao_id) VALUES (4, 'Sistemas Hospitalares', 2, 3);
INSERT INTO public.setor_ocupacao (setor_ocupacao_id, nome, empresa_id, area_ocupacao_id) VALUES (5, 'Infraestrutura e Cloud', 1, 1);
INSERT INTO public.setor_ocupacao (setor_ocupacao_id, nome, empresa_id, area_ocupacao_id) VALUES (6, 'Segurança da Informação', 1, 19);
INSERT INTO public.setor_ocupacao (setor_ocupacao_id, nome, empresa_id, area_ocupacao_id) VALUES (7, 'Qualidade de Software e Testes', 1, 1);
INSERT INTO public.setor_ocupacao (setor_ocupacao_id, nome, empresa_id, area_ocupacao_id) VALUES (8, 'Produto e Plataformas', 1, 1);
INSERT INTO public.setor_ocupacao (setor_ocupacao_id, nome, empresa_id, area_ocupacao_id) VALUES (9, 'Suporte e Operações de TI', 1, 1);
INSERT INTO public.setor_ocupacao (setor_ocupacao_id, nome, empresa_id, area_ocupacao_id) VALUES (10, 'Inteligência Artificial e Machine Learning', 1, 4);
INSERT INTO public.setor_ocupacao (setor_ocupacao_id, nome, empresa_id, area_ocupacao_id) VALUES (11, 'Analytics e Business Intelligence', 1, 4);
INSERT INTO public.setor_ocupacao (setor_ocupacao_id, nome, empresa_id, area_ocupacao_id) VALUES (12, 'Marketing Digital e Growth', 11, 13);
INSERT INTO public.setor_ocupacao (setor_ocupacao_id, nome, empresa_id, area_ocupacao_id) VALUES (13, 'Criação e Conteúdo', 11, 13);
INSERT INTO public.setor_ocupacao (setor_ocupacao_id, nome, empresa_id, area_ocupacao_id) VALUES (14, 'Mídia e Performance', 11, 13);
INSERT INTO public.setor_ocupacao (setor_ocupacao_id, nome, empresa_id, area_ocupacao_id) VALUES (15, 'Product Design e UX', 11, 18);
INSERT INTO public.setor_ocupacao (setor_ocupacao_id, nome, empresa_id, area_ocupacao_id) VALUES (16, 'Recrutamento e Seleção', 2, 2);
INSERT INTO public.setor_ocupacao (setor_ocupacao_id, nome, empresa_id, area_ocupacao_id) VALUES (17, 'Departamento Pessoal', 2, 2);
INSERT INTO public.setor_ocupacao (setor_ocupacao_id, nome, empresa_id, area_ocupacao_id) VALUES (18, 'Treinamento e Desenvolvimento', 2, 2);
INSERT INTO public.setor_ocupacao (setor_ocupacao_id, nome, empresa_id, area_ocupacao_id) VALUES (19, 'Saúde Digital e Telemedicina', 2, 3);
INSERT INTO public.setor_ocupacao (setor_ocupacao_id, nome, empresa_id, area_ocupacao_id) VALUES (20, 'Hospitais e Clínicas', 2, 3);
INSERT INTO public.setor_ocupacao (setor_ocupacao_id, nome, empresa_id, area_ocupacao_id) VALUES (21, 'Planos de Saúde', 2, 3);
INSERT INTO public.setor_ocupacao (setor_ocupacao_id, nome, empresa_id, area_ocupacao_id) VALUES (22, 'Diagnóstico e Laboratórios', 2, 3);
INSERT INTO public.setor_ocupacao (setor_ocupacao_id, nome, empresa_id, area_ocupacao_id) VALUES (23, 'Farmácias e Drogarias', 13, 21);
INSERT INTO public.setor_ocupacao (setor_ocupacao_id, nome, empresa_id, area_ocupacao_id) VALUES (24, 'Pesquisa Clínica e Biotecnologia', 13, 21);
INSERT INTO public.setor_ocupacao (setor_ocupacao_id, nome, empresa_id, area_ocupacao_id) VALUES (25, 'Bancos Digitais', 3, 5);
INSERT INTO public.setor_ocupacao (setor_ocupacao_id, nome, empresa_id, area_ocupacao_id) VALUES (26, 'Fintechs e Meios de Pagamento', 3, 5);
INSERT INTO public.setor_ocupacao (setor_ocupacao_id, nome, empresa_id, area_ocupacao_id) VALUES (27, 'Seguros e Previdência', 3, 5);
INSERT INTO public.setor_ocupacao (setor_ocupacao_id, nome, empresa_id, area_ocupacao_id) VALUES (28, 'Crédito e Cobrança', 3, 5);
INSERT INTO public.setor_ocupacao (setor_ocupacao_id, nome, empresa_id, area_ocupacao_id) VALUES (29, 'Auditoria e Consultoria Financeira', 3, 5);
INSERT INTO public.setor_ocupacao (setor_ocupacao_id, nome, empresa_id, area_ocupacao_id) VALUES (30, 'Varejo Omnichannel', 4, 6);
INSERT INTO public.setor_ocupacao (setor_ocupacao_id, nome, empresa_id, area_ocupacao_id) VALUES (31, 'E-commerce e Marketplace', 4, 6);
INSERT INTO public.setor_ocupacao (setor_ocupacao_id, nome, empresa_id, area_ocupacao_id) VALUES (32, 'Supermercados e Atacarejo', 4, 6);
INSERT INTO public.setor_ocupacao (setor_ocupacao_id, nome, empresa_id, area_ocupacao_id) VALUES (33, 'Moda e Vestuário', 4, 6);
INSERT INTO public.setor_ocupacao (setor_ocupacao_id, nome, empresa_id, area_ocupacao_id) VALUES (34, 'Transporte e Frotas', 6, 8);
INSERT INTO public.setor_ocupacao (setor_ocupacao_id, nome, empresa_id, area_ocupacao_id) VALUES (35, 'Armazenagem e Centros de Distribuição', 6, 8);
INSERT INTO public.setor_ocupacao (setor_ocupacao_id, nome, empresa_id, area_ocupacao_id) VALUES (36, 'Última Milha e Delivery', 6, 8);
INSERT INTO public.setor_ocupacao (setor_ocupacao_id, nome, empresa_id, area_ocupacao_id) VALUES (37, 'Comércio Exterior', 6, 8);
INSERT INTO public.setor_ocupacao (setor_ocupacao_id, nome, empresa_id, area_ocupacao_id) VALUES (38, 'Indústria Automotiva', 5, 25);
INSERT INTO public.setor_ocupacao (setor_ocupacao_id, nome, empresa_id, area_ocupacao_id) VALUES (39, 'Metalurgia e Siderurgia', 5, 28);
INSERT INTO public.setor_ocupacao (setor_ocupacao_id, nome, empresa_id, area_ocupacao_id) VALUES (40, 'Alimentos e Bebidas', 5, 7);
INSERT INTO public.setor_ocupacao (setor_ocupacao_id, nome, empresa_id, area_ocupacao_id) VALUES (41, 'Bens de Consumo', 5, 7);
INSERT INTO public.setor_ocupacao (setor_ocupacao_id, nome, empresa_id, area_ocupacao_id) VALUES (42, 'Máquinas e Equipamentos', 5, 7);
INSERT INTO public.setor_ocupacao (setor_ocupacao_id, nome, empresa_id, area_ocupacao_id) VALUES (43, 'Química e Petroquímica', 5, 29);
INSERT INTO public.setor_ocupacao (setor_ocupacao_id, nome, empresa_id, area_ocupacao_id) VALUES (44, 'Papel e Celulose', 5, 28);
INSERT INTO public.setor_ocupacao (setor_ocupacao_id, nome, empresa_id, area_ocupacao_id) VALUES (45, 'Energia Renovável', 10, 12);
INSERT INTO public.setor_ocupacao (setor_ocupacao_id, nome, empresa_id, area_ocupacao_id) VALUES (46, 'Petróleo e Gás', 10, 12);
INSERT INTO public.setor_ocupacao (setor_ocupacao_id, nome, empresa_id, area_ocupacao_id) VALUES (47, 'Saneamento e Utilities', 10, 12);
INSERT INTO public.setor_ocupacao (setor_ocupacao_id, nome, empresa_id, area_ocupacao_id) VALUES (48, 'Mineração', 5, 28);
INSERT INTO public.setor_ocupacao (setor_ocupacao_id, nome, empresa_id, area_ocupacao_id) VALUES (49, 'Construção e Infraestrutura', 7, 9);
INSERT INTO public.setor_ocupacao (setor_ocupacao_id, nome, empresa_id, area_ocupacao_id) VALUES (50, 'Incorporação e Real Estate', 7, 24);
INSERT INTO public.setor_ocupacao (setor_ocupacao_id, nome, empresa_id, area_ocupacao_id) VALUES (51, 'Projetos e Instalações', 7, 9);
INSERT INTO public.setor_ocupacao (setor_ocupacao_id, nome, empresa_id, area_ocupacao_id) VALUES (52, 'Educação Básica', 8, 10);
INSERT INTO public.setor_ocupacao (setor_ocupacao_id, nome, empresa_id, area_ocupacao_id) VALUES (53, 'Ensino Superior', 8, 10);
INSERT INTO public.setor_ocupacao (setor_ocupacao_id, nome, empresa_id, area_ocupacao_id) VALUES (54, 'EdTech', 8, 10);
INSERT INTO public.setor_ocupacao (setor_ocupacao_id, nome, empresa_id, area_ocupacao_id) VALUES (55, 'Treinamento Corporativo', 8, 10);
INSERT INTO public.setor_ocupacao (setor_ocupacao_id, nome, empresa_id, area_ocupacao_id) VALUES (56, 'Agronegócio', 9, 11);
INSERT INTO public.setor_ocupacao (setor_ocupacao_id, nome, empresa_id, area_ocupacao_id) VALUES (57, 'Agricultura de Precisão', 9, 11);
INSERT INTO public.setor_ocupacao (setor_ocupacao_id, nome, empresa_id, area_ocupacao_id) VALUES (58, 'Pecuária e Proteína Animal', 9, 11);
INSERT INTO public.setor_ocupacao (setor_ocupacao_id, nome, empresa_id, area_ocupacao_id) VALUES (59, 'Advocacia Empresarial', 16, 16);
INSERT INTO public.setor_ocupacao (setor_ocupacao_id, nome, empresa_id, area_ocupacao_id) VALUES (60, 'Compliance e Privacidade', 16, 16);
INSERT INTO public.setor_ocupacao (setor_ocupacao_id, nome, empresa_id, area_ocupacao_id) VALUES (61, 'Contratos e Licitações', 16, 16);
INSERT INTO public.setor_ocupacao (setor_ocupacao_id, nome, empresa_id, area_ocupacao_id) VALUES (62, 'SAC e Atendimento', 12, 17);
INSERT INTO public.setor_ocupacao (setor_ocupacao_id, nome, empresa_id, area_ocupacao_id) VALUES (63, 'Customer Success', 12, 17);
INSERT INTO public.setor_ocupacao (setor_ocupacao_id, nome, empresa_id, area_ocupacao_id) VALUES (64, 'Vendas B2B', 12, 14);
INSERT INTO public.setor_ocupacao (setor_ocupacao_id, nome, empresa_id, area_ocupacao_id) VALUES (65, 'Inside Sales', 12, 14);
INSERT INTO public.setor_ocupacao (setor_ocupacao_id, nome, empresa_id, area_ocupacao_id) VALUES (66, 'Gestão Pública', 15, 27);
INSERT INTO public.setor_ocupacao (setor_ocupacao_id, nome, empresa_id, area_ocupacao_id) VALUES (67, 'Terceiro Setor e ONGs', 15, 27);
INSERT INTO public.setor_ocupacao (setor_ocupacao_id, nome, empresa_id, area_ocupacao_id) VALUES (68, 'Hotéis e Pousadas', 14, 23);
INSERT INTO public.setor_ocupacao (setor_ocupacao_id, nome, empresa_id, area_ocupacao_id) VALUES (69, 'Eventos e Turismo', 14, 23);
INSERT INTO public.setor_ocupacao (setor_ocupacao_id, nome, empresa_id, area_ocupacao_id) VALUES (70, 'Restaurantes e Food Service', 14, 22);
INSERT INTO public.setor_ocupacao (setor_ocupacao_id, nome, empresa_id, area_ocupacao_id) VALUES (71, 'Telecomunicações', 19, 26);
INSERT INTO public.setor_ocupacao (setor_ocupacao_id, nome, empresa_id, area_ocupacao_id) VALUES (72, 'Infraestrutura de Telecom', 19, 26);
INSERT INTO public.setor_ocupacao (setor_ocupacao_id, nome, empresa_id, area_ocupacao_id) VALUES (73, 'Beleza e Estética', 17, 30);
INSERT INTO public.setor_ocupacao (setor_ocupacao_id, nome, empresa_id, area_ocupacao_id) VALUES (74, 'Cosméticos e Cuidados Pessoais', 17, 30);
INSERT INTO public.setor_ocupacao (setor_ocupacao_id, nome, empresa_id, area_ocupacao_id) VALUES (75, 'Facilities e Serviços Gerais', 18, 32);
INSERT INTO public.setor_ocupacao (setor_ocupacao_id, nome, empresa_id, area_ocupacao_id) VALUES (76, 'Consultoria Estratégica', 18, 31);
INSERT INTO public.setor_ocupacao (setor_ocupacao_id, nome, empresa_id, area_ocupacao_id) VALUES (77, 'ESG e Economia Circular', 18, 20);
INSERT INTO public.setor_ocupacao (setor_ocupacao_id, nome, empresa_id, area_ocupacao_id) VALUES (78, 'Mobilidade Urbana', 20, 25);
INSERT INTO public.setor_ocupacao (setor_ocupacao_id, nome, empresa_id, area_ocupacao_id) VALUES (79, 'Administração e Backoffice', 18, 15);


ALTER TABLE public.setor_ocupacao ENABLE TRIGGER ALL;

--
-- Data for Name: usuario; Type: TABLE DATA; Schema: public; Owner: -
--

ALTER TABLE public.usuario DISABLE TRIGGER ALL;

INSERT INTO public.usuario (usuario_id, nome, email, senha, primeiro_acesso, status_usuario, tipo_usuario, is_oauth) VALUES (1, 'Administrador NextHire', 'admin@nexthire.com', '$argon2id$v=19$m=19456,t=2,p=1$40o4/uZ6PxatwMEcx1Y7/g$WBTmXs/9qbpz24VGOzd0Wr2eoiNWjTKv+wFi5XprgUQ', false, true, 'adm', false);
INSERT INTO public.usuario (usuario_id, nome, email, senha, primeiro_acesso, status_usuario, tipo_usuario, is_oauth) VALUES (2, 'Mariana Costa', 'mariana@technova.com', '$argon2id$v=19$m=19456,t=2,p=1$h8HC16pxUPC+jo+ecCpK1Q$kEaeTDVIZOgVuQHf077PbZ+mLkN9wVdnyl9FHbpYkoY', false, true, 'analista_rh', false);
INSERT INTO public.usuario (usuario_id, nome, email, senha, primeiro_acesso, status_usuario, tipo_usuario, is_oauth) VALUES (3, 'Rafael Almeida', 'rafael@technova.com', '$argon2id$v=19$m=19456,t=2,p=1$oKdo6GPvhwNDSHJ21LriOA$JqZfJWJwOjjxkUM5XrIZUS6LXOQBAKlbHkBrU8z277c', false, true, 'gestor', false);
INSERT INTO public.usuario (usuario_id, nome, email, senha, primeiro_acesso, status_usuario, tipo_usuario, is_oauth) VALUES (4, 'Fernanda Lima', 'fernanda@inovasaude.com', '$argon2id$v=19$m=19456,t=2,p=1$u9HsUohUreJgcyaWIGwmkA$nMkquLqY6LAfIXbGpHlHv4FYHGOg6u53yOU91/1c7ZA', false, true, 'gestor', false);
INSERT INTO public.usuario (usuario_id, nome, email, senha, primeiro_acesso, status_usuario, tipo_usuario, is_oauth) VALUES (5, 'Ana Beatriz Souza', 'ana@email.com', '$argon2id$v=19$m=19456,t=2,p=1$l+UMPh+QFnyl7rCKyM0KLQ$M6beWKZIv54zjI3vG72RC5w8O4BYRzMDoLry8zHtsRs', false, true, 'candidato', false);
INSERT INTO public.usuario (usuario_id, nome, email, senha, primeiro_acesso, status_usuario, tipo_usuario, is_oauth) VALUES (6, 'Bruno Martins', 'bruno@email.com', '$argon2id$v=19$m=19456,t=2,p=1$vcUJASVyIBizc/76N0TITw$FONTwH7i29ZfmlteckAeBX7dUoLJmjm+hlipAwCRCHE', false, true, 'candidato', false);
INSERT INTO public.usuario (usuario_id, nome, email, senha, primeiro_acesso, status_usuario, tipo_usuario, is_oauth) VALUES (7, 'Carla Oliveira', 'carla@email.com', '$argon2id$v=19$m=19456,t=2,p=1$wwMY4QvEnk5t+hDBmsvq/w$RcDdpk9kKhOTyW/ub0q4WqnG2lvj69m8Rha3tm3LxLQ', false, true, 'candidato', false);
INSERT INTO public.usuario (usuario_id, nome, email, senha, primeiro_acesso, status_usuario, tipo_usuario, is_oauth) VALUES (8, 'Diego Santos', 'diego@email.com', '$argon2id$v=19$m=19456,t=2,p=1$jxCi9z3syfaqOkrKw8FqNQ$b7qrT6N3U1Kyn07PtHkEZXa6fsXuhclllr8ltf/ZZ7A', false, true, 'candidato', false);
INSERT INTO public.usuario (usuario_id, nome, email, senha, primeiro_acesso, status_usuario, tipo_usuario, is_oauth) VALUES (9, 'Juliana Ribeiro', 'juliana@inovasaude.com', '$argon2id$v=19$m=19456,t=2,p=1$L3duIDOMxlmU2qTQFZMGbg$Zyoa4GyPCob8IL4swievG9Lx4NkQaNrvv3dXL9QOIZ4', false, true, 'analista_rh', false);
INSERT INTO public.usuario (usuario_id, nome, email, senha, primeiro_acesso, status_usuario, tipo_usuario, is_oauth) VALUES (10, 'Kaue Candidato', 'kauecandidato@email.com', '$argon2id$v=19$m=19456,t=2,p=1$WWJibDhTMnBZV2xZTzlKSg$hZ9hkQf2z4/XpJMAUU5rgFTRNnSf49lomylOSUlbJ1E', false, true, 'candidato', false);
INSERT INTO public.usuario (usuario_id, nome, email, senha, primeiro_acesso, status_usuario, tipo_usuario, is_oauth) VALUES (11, 'Nicollas Candidato', 'nicollascandidato@email.com', '$argon2id$v=19$m=19456,t=2,p=1$cU1qVTZoM09TZzVCNnBRTw$BVMvXHLBke/kqlPtSmgtRjKRosv327iSCtVI/sZjvkw', false, true, 'candidato', false);
INSERT INTO public.usuario (usuario_id, nome, email, senha, primeiro_acesso, status_usuario, tipo_usuario, is_oauth) VALUES (12, 'Vitor Candidato', 'vitorcandidato@email.com', '$argon2id$v=19$m=19456,t=2,p=1$NEtWcHA4TTVkZnpSRTVYbg$/Uu8tq5TFnAvO0rghZ372B8Po1jPLauz6g0DV4nSw9g', false, true, 'candidato', false);
INSERT INTO public.usuario (usuario_id, nome, email, senha, primeiro_acesso, status_usuario, tipo_usuario, is_oauth) VALUES (13, 'Catarina Candidata', 'catarinacandidata@email.com', '$argon2id$v=19$m=19456,t=2,p=1$cVhPODhUU0Z1WnVjd05Zeg$P3S6n5CUzC89rcRguG7z3gxng2XZasAr49PB8ldTP20', false, true, 'candidato', false);
INSERT INTO public.usuario (usuario_id, nome, email, senha, primeiro_acesso, status_usuario, tipo_usuario, is_oauth) VALUES (14, 'Mayara Candidata', 'mayaracandidata@email.com', '$argon2id$v=19$m=19456,t=2,p=1$dElINzc5cXdabjFQM012cA$kh8lp0mnjZULGnc69Cvu6iiB2QKcrO6mn340a3w3bak', false, true, 'candidato', false);
INSERT INTO public.usuario (usuario_id, nome, email, senha, primeiro_acesso, status_usuario, tipo_usuario, is_oauth) VALUES (15, 'Kaue Adm', 'kaueadm@email.com', '$argon2id$v=19$m=19456,t=2,p=1$UldFQmJuVlBvd2RWbFMxNQ$c0ydnRIODWVqQuX6z/C1XeExQ1NcGX8rhoQ4CbziUw8', false, true, 'adm', false);
INSERT INTO public.usuario (usuario_id, nome, email, senha, primeiro_acesso, status_usuario, tipo_usuario, is_oauth) VALUES (16, 'Nicollas Adm', 'nicollasadm@email.com', '$argon2id$v=19$m=19456,t=2,p=1$WkdOYU5yN1ZibXh5bGRxbA$fdeiB3J8dQjhRt2S8sjo+pDCYx0b4cN+E8fqGvjfmb0', false, true, 'adm', false);
INSERT INTO public.usuario (usuario_id, nome, email, senha, primeiro_acesso, status_usuario, tipo_usuario, is_oauth) VALUES (17, 'Vitor Adm', 'vitoradm@email.com', '$argon2id$v=19$m=19456,t=2,p=1$akU5ZFI5dTU5Z3hySG5uOA$D+SjUtStR/VNHQXpfm7z5m57R79Nkve16rd9MbyyKt4', false, true, 'adm', false);
INSERT INTO public.usuario (usuario_id, nome, email, senha, primeiro_acesso, status_usuario, tipo_usuario, is_oauth) VALUES (18, 'Catarina Adm', 'catarinaadm@email.com', '$argon2id$v=19$m=19456,t=2,p=1$MVY2eUFtbGQ5eXFTMjVZTg$kJ5sY7qsYOMDezTBgfVOy4dxgr4BWhZ003MxmyaKKbo', false, true, 'adm', false);
INSERT INTO public.usuario (usuario_id, nome, email, senha, primeiro_acesso, status_usuario, tipo_usuario, is_oauth) VALUES (19, 'Mayara Adm', 'mayaraadm@email.com', '$argon2id$v=19$m=19456,t=2,p=1$VGxRNFY0NFljVk5FZDRLcQ$DPejqCjpvgqf8uBGHjGywVNbmaP+u6Mcaym1Vdaqrdo', false, true, 'adm', false);
INSERT INTO public.usuario (usuario_id, nome, email, senha, primeiro_acesso, status_usuario, tipo_usuario, is_oauth) VALUES (20, 'Kaue Gestor', 'kauegestor@email.com', '$argon2id$v=19$m=19456,t=2,p=1$WXdtN0libGhuaVhlQjVOeg$Z6DIkuGjpYWgFQEhze/Gecs+1P1q91keBdLWp6rpR0E', false, true, 'gestor', false);
INSERT INTO public.usuario (usuario_id, nome, email, senha, primeiro_acesso, status_usuario, tipo_usuario, is_oauth) VALUES (21, 'Nicollas Gestor', 'nicollasgestor@email.com', '$argon2id$v=19$m=19456,t=2,p=1$Mm1jMElKQVV5RndFM1VjUQ$Cs19Hcce+fkNN4hazomKQJABULLNIZjoevKiWnPZyaE', false, true, 'gestor', false);
INSERT INTO public.usuario (usuario_id, nome, email, senha, primeiro_acesso, status_usuario, tipo_usuario, is_oauth) VALUES (22, 'Vitor Gestor', 'vitorgestor@email.com', '$argon2id$v=19$m=19456,t=2,p=1$TzBmeTJJd0I3UFpKcEhFcA$XljCEX4jSKM1R9PmoFWPSeQaSAiZ4zXH7fuyLjgRZ9A', false, true, 'gestor', false);
INSERT INTO public.usuario (usuario_id, nome, email, senha, primeiro_acesso, status_usuario, tipo_usuario, is_oauth) VALUES (23, 'Catarina Gestora', 'catarinagestora@email.com', '$argon2id$v=19$m=19456,t=2,p=1$UnNIak9hSWdkNXowUEV2aA$63uTsQTMHNe3GGqyUItKXgs8+wzfaCiBWY4XoFS9rEw', false, true, 'gestor', false);
INSERT INTO public.usuario (usuario_id, nome, email, senha, primeiro_acesso, status_usuario, tipo_usuario, is_oauth) VALUES (24, 'Mayara Gestora', 'mayaragestora@email.com', '$argon2id$v=19$m=19456,t=2,p=1$WFhJVXFoUWpFcnFFSGtrTQ$bJAtJ+CuCQYCpI1xqK/fVxIhpGQ8HMhTEANAlvNFVvQ', false, true, 'gestor', false);
INSERT INTO public.usuario (usuario_id, nome, email, senha, primeiro_acesso, status_usuario, tipo_usuario, is_oauth) VALUES (25, 'Kaue Analista', 'kaueanalista@email.com', '$argon2id$v=19$m=19456,t=2,p=1$dTFXOTNKcFdlZzVUSEgwQw$IspNxHTGS0g6pIiqh4N0tmtx2rOe/gFNfrll9sE5EW0', false, true, 'analista_rh', false);
INSERT INTO public.usuario (usuario_id, nome, email, senha, primeiro_acesso, status_usuario, tipo_usuario, is_oauth) VALUES (26, 'Nicollas Analista', 'nicollasanalista@email.com', '$argon2id$v=19$m=19456,t=2,p=1$cFBwOUw4cE82MlpiYnJuMA$M+P8QaoSKbXCwvTJbrMckSIvnWCEICO0SZSW5BO9oCY', false, true, 'analista_rh', false);
INSERT INTO public.usuario (usuario_id, nome, email, senha, primeiro_acesso, status_usuario, tipo_usuario, is_oauth) VALUES (27, 'Vitor Analista', 'vitoranalista@email.com', '$argon2id$v=19$m=19456,t=2,p=1$QkxyTXE5NGl2UVNzQWpZNA$fYxMw/R6VQ0WGuKCLumRbgpoPNPKLR8YI8FijGsGS4o', false, true, 'analista_rh', false);
INSERT INTO public.usuario (usuario_id, nome, email, senha, primeiro_acesso, status_usuario, tipo_usuario, is_oauth) VALUES (28, 'Catarina Analista', 'catarinaanalista@email.com', '$argon2id$v=19$m=19456,t=2,p=1$VnN6aUo3TXNDWnNtam1kbg$/Ts3VJGuX8kJKJW04K/2y00vP0ZsamUZt9x55jYeMjg', false, true, 'analista_rh', false);
INSERT INTO public.usuario (usuario_id, nome, email, senha, primeiro_acesso, status_usuario, tipo_usuario, is_oauth) VALUES (29, 'Mayara Analista', 'mayaraanalista@email.com', '$argon2id$v=19$m=19456,t=2,p=1$emY2bDdkYXZ3ZkRYVGY0RA$nS6R1y+/iho/K6RQPNQ8XeXL4WIppTFOjhdRooqbh5o', false, true, 'analista_rh', false);


ALTER TABLE public.usuario ENABLE TRIGGER ALL;

--
-- Data for Name: vaga; Type: TABLE DATA; Schema: public; Owner: -
--

ALTER TABLE public.vaga DISABLE TRIGGER ALL;

INSERT INTO public.vaga (vaga_id, titulo, data_abertura, data_fechamento, descricao, remuneracao, idiomas_desejados, tempo_experiencia_desejado, escala_vaga, status_vaga, modelo_trabalho_vaga, nivel_prioridade_vaga, tipo_vaga, empresa_id, analista_rh, setor_id) VALUES (1, 'Desenvolvedor Backend Pleno', '2026-10-01 00:00:00', '2026-12-31 00:00:00', 'Desenvolvimento e manutenção de APIs REST utilizando C#, .NET, PostgreSQL e Entity Framework Core.', 8500.00, '["Português", "Inglês"]', 2.00, '5x2', true, 'hibrido', 'alta', 'nova_funcao', 1, 2, 1);
INSERT INTO public.vaga (vaga_id, titulo, data_abertura, data_fechamento, descricao, remuneracao, idiomas_desejados, tempo_experiencia_desejado, escala_vaga, status_vaga, modelo_trabalho_vaga, nivel_prioridade_vaga, tipo_vaga, empresa_id, analista_rh, setor_id) VALUES (2, 'Analista de Dados Pleno', '2026-10-05 00:00:00', '2026-12-20 00:00:00', 'Análise de dados, construção de dashboards, consultas SQL e desenvolvimento de indicadores de negócio.', 7200.00, '["Português", "Inglês"]', 2.00, '5x2', true, 'remoto', 'media', 'substituicao', 1, 2, 2);
INSERT INTO public.vaga (vaga_id, titulo, data_abertura, data_fechamento, descricao, remuneracao, idiomas_desejados, tempo_experiencia_desejado, escala_vaga, status_vaga, modelo_trabalho_vaga, nivel_prioridade_vaga, tipo_vaga, empresa_id, analista_rh, setor_id) VALUES (3, 'Desenvolvedor de Sistemas Hospitalares', '2026-10-10 00:00:00', '2027-01-15 00:00:00', 'Desenvolvimento de sistemas para gestão hospitalar, integração de APIs e manutenção de aplicações.', 9200.00, '["Português"]', 2.00, '5x2', true, 'presencial', 'alta', 'expansao_equipe', 2, 9, 4);
INSERT INTO public.vaga (vaga_id, titulo, data_abertura, data_fechamento, descricao, remuneracao, idiomas_desejados, tempo_experiencia_desejado, escala_vaga, status_vaga, modelo_trabalho_vaga, nivel_prioridade_vaga, tipo_vaga, empresa_id, analista_rh, setor_id) VALUES (4, 'Desenvolvedor Full Stack Sênior', '2026-11-24 00:00:00', '2027-01-31 00:00:00', 'Desenvolvimento de aplicações web full stack, APIs e evolução de plataformas internas.', 12500.00, '["Português", "Inglês"]', 5.00, '5x2', true, 'hibrido', 'alta', 'nova_funcao', 1, 25, 8);
INSERT INTO public.vaga (vaga_id, titulo, data_abertura, data_fechamento, descricao, remuneracao, idiomas_desejados, tempo_experiencia_desejado, escala_vaga, status_vaga, modelo_trabalho_vaga, nivel_prioridade_vaga, tipo_vaga, empresa_id, analista_rh, setor_id) VALUES (5, 'Engenheiro de Dados Sênior', '2026-11-24 00:00:00', '2027-01-31 00:00:00', 'Construção e manutenção de pipelines de dados, modelagem analítica e integração com cloud.', 13500.00, '["Português", "Inglês"]', 4.00, '5x2', true, 'remoto', 'alta', 'expansao_equipe', 1, 25, 2);
INSERT INTO public.vaga (vaga_id, titulo, data_abertura, data_fechamento, descricao, remuneracao, idiomas_desejados, tempo_experiencia_desejado, escala_vaga, status_vaga, modelo_trabalho_vaga, nivel_prioridade_vaga, tipo_vaga, empresa_id, analista_rh, setor_id) VALUES (6, 'Especialista em Segurança da Informação', '2026-11-24 00:00:00', '2027-01-31 00:00:00', 'Definição de controles de segurança, resposta a incidentes e conformidade com LGPD.', 14000.00, '["Português", "Inglês"]', 5.00, '5x2', true, 'hibrido', 'alta', 'substituicao', 1, 25, 6);
INSERT INTO public.vaga (vaga_id, titulo, data_abertura, data_fechamento, descricao, remuneracao, idiomas_desejados, tempo_experiencia_desejado, escala_vaga, status_vaga, modelo_trabalho_vaga, nivel_prioridade_vaga, tipo_vaga, empresa_id, analista_rh, setor_id) VALUES (7, 'Analista de RH Sênior', '2026-11-24 00:00:00', '2027-01-31 00:00:00', 'Condução de processos de recrutamento, desenvolvimento de pessoas e indicadores de RH.', 7800.00, '["Português"]', 4.00, '5x2', true, 'hibrido', 'media', 'substituicao', 2, 26, 16);
INSERT INTO public.vaga (vaga_id, titulo, data_abertura, data_fechamento, descricao, remuneracao, idiomas_desejados, tempo_experiencia_desejado, escala_vaga, status_vaga, modelo_trabalho_vaga, nivel_prioridade_vaga, tipo_vaga, empresa_id, analista_rh, setor_id) VALUES (8, 'Analista de Saúde Digital', '2026-11-24 00:00:00', '2027-01-31 00:00:00', 'Apoio à implantação de soluções digitais em saúde, integração de sistemas e análise de fluxos.', 8200.00, '["Português", "Inglês"]', 3.00, '5x2', true, 'remoto', 'media', 'expansao_equipe', 2, 26, 19);
INSERT INTO public.vaga (vaga_id, titulo, data_abertura, data_fechamento, descricao, remuneracao, idiomas_desejados, tempo_experiencia_desejado, escala_vaga, status_vaga, modelo_trabalho_vaga, nivel_prioridade_vaga, tipo_vaga, empresa_id, analista_rh, setor_id) VALUES (9, 'Engenheiro de Software Backend', '2026-11-24 00:00:00', '2027-01-31 00:00:00', 'Desenvolvimento de serviços financeiros escaláveis, APIs e integração com meios de pagamento.', 11000.00, '["Português", "Inglês"]', 4.00, '5x2', true, 'hibrido', 'alta', 'nova_funcao', 3, 27, 25);
INSERT INTO public.vaga (vaga_id, titulo, data_abertura, data_fechamento, descricao, remuneracao, idiomas_desejados, tempo_experiencia_desejado, escala_vaga, status_vaga, modelo_trabalho_vaga, nivel_prioridade_vaga, tipo_vaga, empresa_id, analista_rh, setor_id) VALUES (10, 'Analista de Risco e Crédito', '2026-11-24 00:00:00', '2027-01-31 00:00:00', 'Análise de risco de crédito, definição de políticas e monitoramento de carteira.', 9000.00, '["Português"]', 3.00, '5x2', true, 'presencial', 'media', 'substituicao', 3, 27, 28);
INSERT INTO public.vaga (vaga_id, titulo, data_abertura, data_fechamento, descricao, remuneracao, idiomas_desejados, tempo_experiencia_desejado, escala_vaga, status_vaga, modelo_trabalho_vaga, nivel_prioridade_vaga, tipo_vaga, empresa_id, analista_rh, setor_id) VALUES (11, 'Especialista em E-commerce', '2026-11-24 00:00:00', '2027-01-31 00:00:00', 'Gestão de operação de e-commerce, conversão, marketplace e integrações com logística.', 9500.00, '["Português", "Inglês"]', 4.00, '5x2', true, 'hibrido', 'alta', 'expansao_equipe', 4, 28, 31);
INSERT INTO public.vaga (vaga_id, titulo, data_abertura, data_fechamento, descricao, remuneracao, idiomas_desejados, tempo_experiencia_desejado, escala_vaga, status_vaga, modelo_trabalho_vaga, nivel_prioridade_vaga, tipo_vaga, empresa_id, analista_rh, setor_id) VALUES (12, 'Analista de Supply Chain', '2026-11-24 00:00:00', '2027-01-31 00:00:00', 'Planejamento de demanda, gestão de estoques e acompanhamento de indicadores logísticos.', 7600.00, '["Português"]', 3.00, '5x2', true, 'presencial', 'media', 'nova_funcao', 6, 29, 34);
INSERT INTO public.vaga (vaga_id, titulo, data_abertura, data_fechamento, descricao, remuneracao, idiomas_desejados, tempo_experiencia_desejado, escala_vaga, status_vaga, modelo_trabalho_vaga, nivel_prioridade_vaga, tipo_vaga, empresa_id, analista_rh, setor_id) VALUES (13, 'Coordenador de Última Milha', '2026-11-24 00:00:00', '2027-01-31 00:00:00', 'Coordenação de operação de entrega, roteirização e relacionamento com parceiros logísticos.', 8800.00, '["Português"]', 4.00, '5x2', true, 'hibrido', 'alta', 'substituicao', 6, 29, 36);


ALTER TABLE public.vaga ENABLE TRIGGER ALL;

--
-- Data for Name: questao; Type: TABLE DATA; Schema: public; Owner: -
--

ALTER TABLE public.questao DISABLE TRIGGER ALL;

INSERT INTO public.questao (questao_id, texto_questao, classsificacao_questao, tipo_questao, vaga_id) VALUES (1, 'Você possui experiência profissional com C# e .NET?', 'tecnica', 'verdadeiro_falso', 1);
INSERT INTO public.questao (questao_id, texto_questao, classsificacao_questao, tipo_questao, vaga_id) VALUES (2, 'Qual tecnologia é utilizada para mapear objetos em bancos relacionais no .NET?', 'tecnica', 'unica_escolha', 1);
INSERT INTO public.questao (questao_id, texto_questao, classsificacao_questao, tipo_questao, vaga_id) VALUES (3, 'Quais práticas são importantes para desenvolver uma API REST?', 'tecnica', 'multipla_escolha', 1);
INSERT INTO public.questao (questao_id, texto_questao, classsificacao_questao, tipo_questao, vaga_id) VALUES (4, 'Descreva como você resolveria um problema de lentidão em uma consulta SQL.', 'tecnica', 'dissertativa', 2);
INSERT INTO public.questao (questao_id, texto_questao, classsificacao_questao, tipo_questao, vaga_id) VALUES (5, 'Como você lidaria com um prazo curto e várias demandas urgentes?', 'comportamental', 'dissertativa', 1);
INSERT INTO public.questao (questao_id, texto_questao, classsificacao_questao, tipo_questao, vaga_id) VALUES (6, 'Um cliente relata um erro crítico no sistema hospitalar. Qual seria sua primeira atitude?', 'situacional', 'unica_escolha', 3);


ALTER TABLE public.questao ENABLE TRIGGER ALL;

--
-- Data for Name: alternativa; Type: TABLE DATA; Schema: public; Owner: -
--

ALTER TABLE public.alternativa DISABLE TRIGGER ALL;

INSERT INTO public.alternativa (alternativa_id, texto_alternativa, alternativa_correta, questao_id) VALUES (1, 'Sim', true, 1);
INSERT INTO public.alternativa (alternativa_id, texto_alternativa, alternativa_correta, questao_id) VALUES (2, 'Não', false, 1);
INSERT INTO public.alternativa (alternativa_id, texto_alternativa, alternativa_correta, questao_id) VALUES (3, 'Entity Framework Core', true, 2);
INSERT INTO public.alternativa (alternativa_id, texto_alternativa, alternativa_correta, questao_id) VALUES (4, 'Photoshop', false, 2);
INSERT INTO public.alternativa (alternativa_id, texto_alternativa, alternativa_correta, questao_id) VALUES (5, 'Visual Studio Code', false, 2);
INSERT INTO public.alternativa (alternativa_id, texto_alternativa, alternativa_correta, questao_id) VALUES (6, 'Utilizar códigos de status HTTP adequados', true, 3);
INSERT INTO public.alternativa (alternativa_id, texto_alternativa, alternativa_correta, questao_id) VALUES (7, 'Validar os dados recebidos', true, 3);
INSERT INTO public.alternativa (alternativa_id, texto_alternativa, alternativa_correta, questao_id) VALUES (8, 'Ignorar autenticação e autorização', false, 3);
INSERT INTO public.alternativa (alternativa_id, texto_alternativa, alternativa_correta, questao_id) VALUES (9, 'Documentar os endpoints', true, 3);
INSERT INTO public.alternativa (alternativa_id, texto_alternativa, alternativa_correta, questao_id) VALUES (10, 'Ignorar o cliente e continuar trabalhando', false, 6);
INSERT INTO public.alternativa (alternativa_id, texto_alternativa, alternativa_correta, questao_id) VALUES (11, 'Investigar a falha, avaliar o impacto e comunicar os envolvidos', true, 6);
INSERT INTO public.alternativa (alternativa_id, texto_alternativa, alternativa_correta, questao_id) VALUES (12, 'Excluir os dados do sistema', false, 6);


ALTER TABLE public.alternativa ENABLE TRIGGER ALL;

--
-- Data for Name: anotacao; Type: TABLE DATA; Schema: public; Owner: -
--

ALTER TABLE public.anotacao DISABLE TRIGGER ALL;

INSERT INTO public.anotacao (anotacao_id, conteudo, candidato_id, gestor_id) VALUES (1, 'Candidato apresenta boa experiência com desenvolvimento backend.', 5, 3);
INSERT INTO public.anotacao (anotacao_id, conteudo, candidato_id, gestor_id) VALUES (2, 'Possui conhecimentos sólidos em análise de dados.', 6, 3);
INSERT INTO public.anotacao (anotacao_id, conteudo, candidato_id, gestor_id) VALUES (3, 'Perfil interessante para vagas de tecnologia na área da saúde.', 7, 4);


ALTER TABLE public.anotacao ENABLE TRIGGER ALL;

--
-- Data for Name: processo_seletivo; Type: TABLE DATA; Schema: public; Owner: -
--

ALTER TABLE public.processo_seletivo DISABLE TRIGGER ALL;

INSERT INTO public.processo_seletivo (processo_seletivo_id, titulo_processo, data_abertura, data_fechamento, status_processo_seletivo, vaga_id) VALUES (1, 'Processo Seletivo - Backend Pleno', '2026-10-01 00:00:00', '2026-12-31 00:00:00', true, 1);
INSERT INTO public.processo_seletivo (processo_seletivo_id, titulo_processo, data_abertura, data_fechamento, status_processo_seletivo, vaga_id) VALUES (2, 'Processo Seletivo - Analista de Dados', '2026-10-05 00:00:00', '2026-12-20 00:00:00', true, 2);
INSERT INTO public.processo_seletivo (processo_seletivo_id, titulo_processo, data_abertura, data_fechamento, status_processo_seletivo, vaga_id) VALUES (3, 'Processo Seletivo - Sistemas Hospitalares', '2026-10-10 00:00:00', '2027-01-15 00:00:00', true, 3);


ALTER TABLE public.processo_seletivo ENABLE TRIGGER ALL;

--
-- Data for Name: entrevista; Type: TABLE DATA; Schema: public; Owner: -
--

ALTER TABLE public.entrevista DISABLE TRIGGER ALL;

INSERT INTO public.entrevista (entrevista_id, nome_entrevista, horario_inicio, horario_fim, status_entrevista, processo_seletivo_id, link) VALUES (1, 'Entrevista Técnica - Backend', '2026-11-10 10:00:00', '2026-11-10 11:00:00', true, 1, 'https://meet.nexthire.com/ficticio/entrevista-1');
INSERT INTO public.entrevista (entrevista_id, nome_entrevista, horario_inicio, horario_fim, status_entrevista, processo_seletivo_id, link) VALUES (2, 'Entrevista com RH - Dados', '2026-11-11 14:00:00', '2026-11-11 14:45:00', true, 2, 'https://meet.nexthire.com/ficticio/entrevista-2');
INSERT INTO public.entrevista (entrevista_id, nome_entrevista, horario_inicio, horario_fim, status_entrevista, processo_seletivo_id, link) VALUES (3, 'Entrevista Técnica - Sistemas Hospitalares', '2026-11-12 09:00:00', '2026-11-12 10:00:00', true, 3, 'https://meet.nexthire.com/ficticio/entrevista-3');


ALTER TABLE public.entrevista ENABLE TRIGGER ALL;

--
-- Data for Name: habilidade; Type: TABLE DATA; Schema: public; Owner: -
--

ALTER TABLE public.habilidade DISABLE TRIGGER ALL;

INSERT INTO public.habilidade (habilidade_id, nome, codigo) VALUES (1, 'C#', 'CSHARP');
INSERT INTO public.habilidade (habilidade_id, nome, codigo) VALUES (2, '.NET', 'DOTNET');
INSERT INTO public.habilidade (habilidade_id, nome, codigo) VALUES (3, 'PostgreSQL', 'POSTGRES');
INSERT INTO public.habilidade (habilidade_id, nome, codigo) VALUES (4, 'Entity Framework Core', 'EFCORE');
INSERT INTO public.habilidade (habilidade_id, nome, codigo) VALUES (5, 'Python', 'PYTHON');
INSERT INTO public.habilidade (habilidade_id, nome, codigo) VALUES (6, 'SQL', 'SQL');
INSERT INTO public.habilidade (habilidade_id, nome, codigo) VALUES (7, 'Power BI', 'POWERBI');
INSERT INTO public.habilidade (habilidade_id, nome, codigo) VALUES (8, 'JavaScript', 'JAVASCRIPT');
INSERT INTO public.habilidade (habilidade_id, nome, codigo) VALUES (9, 'React', 'REACT');
INSERT INTO public.habilidade (habilidade_id, nome, codigo) VALUES (10, 'Docker', 'DOCKER');
INSERT INTO public.habilidade (habilidade_id, nome, codigo) VALUES (11, 'Git', 'GIT');
INSERT INTO public.habilidade (habilidade_id, nome, codigo) VALUES (12, 'Comunicação', 'COMUNICACAO');


ALTER TABLE public.habilidade ENABLE TRIGGER ALL;

--
-- Data for Name: avaliacao_habilidade; Type: TABLE DATA; Schema: public; Owner: -
--

ALTER TABLE public.avaliacao_habilidade DISABLE TRIGGER ALL;

INSERT INTO public.avaliacao_habilidade (habilidade_id, candidato_id, entrevista_id, nivel_avaliado) VALUES (1, 5, 1, 5);
INSERT INTO public.avaliacao_habilidade (habilidade_id, candidato_id, entrevista_id, nivel_avaliado) VALUES (2, 5, 1, 5);
INSERT INTO public.avaliacao_habilidade (habilidade_id, candidato_id, entrevista_id, nivel_avaliado) VALUES (3, 5, 1, 4);
INSERT INTO public.avaliacao_habilidade (habilidade_id, candidato_id, entrevista_id, nivel_avaliado) VALUES (4, 5, 1, 5);
INSERT INTO public.avaliacao_habilidade (habilidade_id, candidato_id, entrevista_id, nivel_avaliado) VALUES (6, 6, 2, 5);
INSERT INTO public.avaliacao_habilidade (habilidade_id, candidato_id, entrevista_id, nivel_avaliado) VALUES (7, 6, 2, 4);
INSERT INTO public.avaliacao_habilidade (habilidade_id, candidato_id, entrevista_id, nivel_avaliado) VALUES (1, 7, 3, 4);
INSERT INTO public.avaliacao_habilidade (habilidade_id, candidato_id, entrevista_id, nivel_avaliado) VALUES (2, 7, 3, 4);


ALTER TABLE public.avaliacao_habilidade ENABLE TRIGGER ALL;

--
-- Data for Name: candidato_entrevista; Type: TABLE DATA; Schema: public; Owner: -
--

ALTER TABLE public.candidato_entrevista DISABLE TRIGGER ALL;

INSERT INTO public.candidato_entrevista (candidato_id, entrevista_id) VALUES (5, 1);
INSERT INTO public.candidato_entrevista (candidato_id, entrevista_id) VALUES (6, 2);
INSERT INTO public.candidato_entrevista (candidato_id, entrevista_id) VALUES (7, 3);


ALTER TABLE public.candidato_entrevista ENABLE TRIGGER ALL;

--
-- Data for Name: candidato_processo_seletivo; Type: TABLE DATA; Schema: public; Owner: -
--

ALTER TABLE public.candidato_processo_seletivo DISABLE TRIGGER ALL;

INSERT INTO public.candidato_processo_seletivo (processo_seletivo_id, candidato_id, status_relacao) VALUES (1, 5, 'avanca');
INSERT INTO public.candidato_processo_seletivo (processo_seletivo_id, candidato_id, status_relacao) VALUES (1, 7, 'considera');
INSERT INTO public.candidato_processo_seletivo (processo_seletivo_id, candidato_id, status_relacao) VALUES (1, 8, 'nao_avanca');
INSERT INTO public.candidato_processo_seletivo (processo_seletivo_id, candidato_id, status_relacao) VALUES (2, 6, 'avanca');
INSERT INTO public.candidato_processo_seletivo (processo_seletivo_id, candidato_id, status_relacao) VALUES (3, 7, 'avanca');
INSERT INTO public.candidato_processo_seletivo (processo_seletivo_id, candidato_id, status_relacao) VALUES (3, 8, 'considera');


ALTER TABLE public.candidato_processo_seletivo ENABLE TRIGGER ALL;

--
-- Data for Name: candidato_questao; Type: TABLE DATA; Schema: public; Owner: -
--

ALTER TABLE public.candidato_questao DISABLE TRIGGER ALL;

INSERT INTO public.candidato_questao (questao_id, candidato_id, reposta_questao) VALUES (1, 5, '{"resposta": true}');
INSERT INTO public.candidato_questao (questao_id, candidato_id, reposta_questao) VALUES (2, 5, '{"resposta": "Entity Framework Core"}');
INSERT INTO public.candidato_questao (questao_id, candidato_id, reposta_questao) VALUES (3, 5, '{"respostas": ["Utilizar códigos de status HTTP adequados", "Validar os dados recebidos", "Documentar os endpoints"]}');
INSERT INTO public.candidato_questao (questao_id, candidato_id, reposta_questao) VALUES (4, 6, '{"resposta": "Analisaria o plano de execução, verificaria índices e avaliaria a necessidade de otimizar a consulta."}');
INSERT INTO public.candidato_questao (questao_id, candidato_id, reposta_questao) VALUES (5, 5, '{"resposta": "Priorizaria as demandas por impacto e urgência, alinhando expectativas com a equipe."}');
INSERT INTO public.candidato_questao (questao_id, candidato_id, reposta_questao) VALUES (6, 7, '{"resposta": "Investigar a falha, avaliar o impacto e comunicar os envolvidos"}');


ALTER TABLE public.candidato_questao ENABLE TRIGGER ALL;

--
-- Data for Name: comentario; Type: TABLE DATA; Schema: public; Owner: -
--

ALTER TABLE public.comentario DISABLE TRIGGER ALL;

INSERT INTO public.comentario (comentario_id, texto, data_comentario, gestor_id, entrevista_id) VALUES (1, 'Candidato demonstrou domínio de C# e conhecimentos consistentes em .NET.', '2026-11-10 11:10:00', 3, 1);
INSERT INTO public.comentario (comentario_id, texto, data_comentario, gestor_id, entrevista_id) VALUES (2, 'Apresentou boa capacidade analítica e experiência com SQL.', '2026-11-11 14:50:00', 3, 2);
INSERT INTO public.comentario (comentario_id, texto, data_comentario, gestor_id, entrevista_id) VALUES (3, 'Demonstrou boa compreensão de sistemas e integrações.', '2026-11-12 10:10:00', 4, 3);


ALTER TABLE public.comentario ENABLE TRIGGER ALL;

--
-- Data for Name: curriculo_candidato; Type: TABLE DATA; Schema: public; Owner: -
--

ALTER TABLE public.curriculo_candidato DISABLE TRIGGER ALL;

INSERT INTO public.curriculo_candidato (curriculo_candidato_id, curriculo_convertido, candidato_id) VALUES (1, '{"nome": "Ana Beatriz Souza", "objetivo": "Desenvolvedora Backend", "competencias": ["C#", ".NET", "PostgreSQL", "Entity Framework Core"], "experiencias": [{"cargo": "Desenvolvedora Full Stack", "empresa": "DevSolutions"}, {"cargo": "Desenvolvedora Backend", "empresa": "CodeLab"}]}', 5);
INSERT INTO public.curriculo_candidato (curriculo_candidato_id, curriculo_convertido, candidato_id) VALUES (2, '{"nome": "Bruno Martins", "objetivo": "Analista de Dados", "competencias": ["Python", "SQL", "Power BI"], "experiencias": [{"cargo": "Analista de Dados Júnior", "empresa": "DataCorp"}, {"cargo": "Analista de Dados Pleno", "empresa": "Analytics Pro"}]}', 6);
INSERT INTO public.curriculo_candidato (curriculo_candidato_id, curriculo_convertido, candidato_id) VALUES (3, '{"nome": "Carla Oliveira", "objetivo": "Desenvolvedora de Sistemas", "competencias": ["C#", ".NET", "JavaScript", "React"], "experiencias": [{"cargo": "Analista de Sistemas", "empresa": "Sistemas Brasil"}, {"cargo": "Desenvolvedora de Sistemas", "empresa": "HealthTech"}]}', 7);
INSERT INTO public.curriculo_candidato (curriculo_candidato_id, curriculo_convertido, candidato_id) VALUES (4, '{"nome": "Diego Santos", "objetivo": "Desenvolvedor Frontend", "competencias": ["JavaScript", "React", "Git"], "experiencias": [{"cargo": "Desenvolvedor Frontend", "empresa": "WebStart"}]}', 8);


ALTER TABLE public.curriculo_candidato ENABLE TRIGGER ALL;

--
-- Data for Name: curriculo_vaga; Type: TABLE DATA; Schema: public; Owner: -
--

ALTER TABLE public.curriculo_vaga DISABLE TRIGGER ALL;

INSERT INTO public.curriculo_vaga (curriculo_id, vaga_id, data_hora_inscricao) VALUES (1, 1, '2026-10-15 09:30:00');
INSERT INTO public.curriculo_vaga (curriculo_id, vaga_id, data_hora_inscricao) VALUES (2, 2, '2026-10-16 10:15:00');
INSERT INTO public.curriculo_vaga (curriculo_id, vaga_id, data_hora_inscricao) VALUES (3, 1, '2026-10-17 14:00:00');
INSERT INTO public.curriculo_vaga (curriculo_id, vaga_id, data_hora_inscricao) VALUES (3, 3, '2026-10-18 11:20:00');
INSERT INTO public.curriculo_vaga (curriculo_id, vaga_id, data_hora_inscricao) VALUES (4, 1, '2026-10-19 08:45:00');
INSERT INTO public.curriculo_vaga (curriculo_id, vaga_id, data_hora_inscricao) VALUES (4, 3, '2026-10-20 16:30:00');


ALTER TABLE public.curriculo_vaga ENABLE TRIGGER ALL;

--
-- Data for Name: empresa_funcionario; Type: TABLE DATA; Schema: public; Owner: -
--

ALTER TABLE public.empresa_funcionario DISABLE TRIGGER ALL;

INSERT INTO public.empresa_funcionario (empresa_id, funcionario_id) VALUES (1, 2);
INSERT INTO public.empresa_funcionario (empresa_id, funcionario_id) VALUES (1, 3);
INSERT INTO public.empresa_funcionario (empresa_id, funcionario_id) VALUES (2, 4);
INSERT INTO public.empresa_funcionario (empresa_id, funcionario_id) VALUES (2, 9);
INSERT INTO public.empresa_funcionario (empresa_id, funcionario_id) VALUES (1, 20);
INSERT INTO public.empresa_funcionario (empresa_id, funcionario_id) VALUES (2, 21);
INSERT INTO public.empresa_funcionario (empresa_id, funcionario_id) VALUES (3, 22);
INSERT INTO public.empresa_funcionario (empresa_id, funcionario_id) VALUES (4, 23);
INSERT INTO public.empresa_funcionario (empresa_id, funcionario_id) VALUES (6, 24);
INSERT INTO public.empresa_funcionario (empresa_id, funcionario_id) VALUES (1, 25);
INSERT INTO public.empresa_funcionario (empresa_id, funcionario_id) VALUES (2, 26);
INSERT INTO public.empresa_funcionario (empresa_id, funcionario_id) VALUES (3, 27);
INSERT INTO public.empresa_funcionario (empresa_id, funcionario_id) VALUES (4, 28);
INSERT INTO public.empresa_funcionario (empresa_id, funcionario_id) VALUES (6, 29);


ALTER TABLE public.empresa_funcionario ENABLE TRIGGER ALL;

--
-- Data for Name: entrevistador_entrevista; Type: TABLE DATA; Schema: public; Owner: -
--

ALTER TABLE public.entrevistador_entrevista DISABLE TRIGGER ALL;

INSERT INTO public.entrevistador_entrevista (gestor_id, entrevista_id) VALUES (3, 1);
INSERT INTO public.entrevistador_entrevista (gestor_id, entrevista_id) VALUES (3, 2);
INSERT INTO public.entrevistador_entrevista (gestor_id, entrevista_id) VALUES (4, 3);


ALTER TABLE public.entrevistador_entrevista ENABLE TRIGGER ALL;

--
-- Data for Name: experiencia_profissional; Type: TABLE DATA; Schema: public; Owner: -
--

ALTER TABLE public.experiencia_profissional DISABLE TRIGGER ALL;

INSERT INTO public.experiencia_profissional (experiencia_profissional_id, nome_empresa, cargo, data_entrada, data_saida, atualmente_trabalho, usuario_id) VALUES (1, 'DevSolutions', 'Desenvolvedora Full Stack', '2021-03-01', '2023-06-30', false, 5);
INSERT INTO public.experiencia_profissional (experiencia_profissional_id, nome_empresa, cargo, data_entrada, data_saida, atualmente_trabalho, usuario_id) VALUES (2, 'CodeLab', 'Desenvolvedora Backend', '2023-07-01', NULL, true, 5);
INSERT INTO public.experiencia_profissional (experiencia_profissional_id, nome_empresa, cargo, data_entrada, data_saida, atualmente_trabalho, usuario_id) VALUES (3, 'DataCorp', 'Analista de Dados Júnior', '2020-02-01', '2022-12-31', false, 6);
INSERT INTO public.experiencia_profissional (experiencia_profissional_id, nome_empresa, cargo, data_entrada, data_saida, atualmente_trabalho, usuario_id) VALUES (4, 'Analytics Pro', 'Analista de Dados Pleno', '2023-01-01', NULL, true, 6);
INSERT INTO public.experiencia_profissional (experiencia_profissional_id, nome_empresa, cargo, data_entrada, data_saida, atualmente_trabalho, usuario_id) VALUES (5, 'Sistemas Brasil', 'Analista de Sistemas', '2019-05-01', '2022-08-31', false, 7);
INSERT INTO public.experiencia_profissional (experiencia_profissional_id, nome_empresa, cargo, data_entrada, data_saida, atualmente_trabalho, usuario_id) VALUES (6, 'HealthTech', 'Desenvolvedora de Sistemas', '2022-09-01', NULL, true, 7);
INSERT INTO public.experiencia_profissional (experiencia_profissional_id, nome_empresa, cargo, data_entrada, data_saida, atualmente_trabalho, usuario_id) VALUES (7, 'WebStart', 'Desenvolvedor Frontend', '2022-01-01', NULL, true, 8);


ALTER TABLE public.experiencia_profissional ENABLE TRIGGER ALL;

--
-- Data for Name: gestor_setor; Type: TABLE DATA; Schema: public; Owner: -
--

ALTER TABLE public.gestor_setor DISABLE TRIGGER ALL;

INSERT INTO public.gestor_setor (gestor_id, setor_ocupacao_id) VALUES (3, 1);
INSERT INTO public.gestor_setor (gestor_id, setor_ocupacao_id) VALUES (3, 2);
INSERT INTO public.gestor_setor (gestor_id, setor_ocupacao_id) VALUES (4, 3);
INSERT INTO public.gestor_setor (gestor_id, setor_ocupacao_id) VALUES (4, 4);
INSERT INTO public.gestor_setor (gestor_id, setor_ocupacao_id) VALUES (20, 1);
INSERT INTO public.gestor_setor (gestor_id, setor_ocupacao_id) VALUES (20, 5);
INSERT INTO public.gestor_setor (gestor_id, setor_ocupacao_id) VALUES (20, 7);
INSERT INTO public.gestor_setor (gestor_id, setor_ocupacao_id) VALUES (21, 3);
INSERT INTO public.gestor_setor (gestor_id, setor_ocupacao_id) VALUES (21, 16);
INSERT INTO public.gestor_setor (gestor_id, setor_ocupacao_id) VALUES (21, 17);
INSERT INTO public.gestor_setor (gestor_id, setor_ocupacao_id) VALUES (22, 25);
INSERT INTO public.gestor_setor (gestor_id, setor_ocupacao_id) VALUES (22, 26);
INSERT INTO public.gestor_setor (gestor_id, setor_ocupacao_id) VALUES (22, 28);
INSERT INTO public.gestor_setor (gestor_id, setor_ocupacao_id) VALUES (23, 30);
INSERT INTO public.gestor_setor (gestor_id, setor_ocupacao_id) VALUES (23, 31);
INSERT INTO public.gestor_setor (gestor_id, setor_ocupacao_id) VALUES (23, 33);
INSERT INTO public.gestor_setor (gestor_id, setor_ocupacao_id) VALUES (24, 34);
INSERT INTO public.gestor_setor (gestor_id, setor_ocupacao_id) VALUES (24, 35);
INSERT INTO public.gestor_setor (gestor_id, setor_ocupacao_id) VALUES (24, 36);


ALTER TABLE public.gestor_setor ENABLE TRIGGER ALL;

--
-- Data for Name: gestor_vaga; Type: TABLE DATA; Schema: public; Owner: -
--

ALTER TABLE public.gestor_vaga DISABLE TRIGGER ALL;

INSERT INTO public.gestor_vaga (gestor_id, vaga_id) VALUES (3, 1);
INSERT INTO public.gestor_vaga (gestor_id, vaga_id) VALUES (3, 2);
INSERT INTO public.gestor_vaga (gestor_id, vaga_id) VALUES (4, 3);
INSERT INTO public.gestor_vaga (gestor_id, vaga_id) VALUES (20, 4);
INSERT INTO public.gestor_vaga (gestor_id, vaga_id) VALUES (20, 5);
INSERT INTO public.gestor_vaga (gestor_id, vaga_id) VALUES (20, 6);
INSERT INTO public.gestor_vaga (gestor_id, vaga_id) VALUES (21, 7);
INSERT INTO public.gestor_vaga (gestor_id, vaga_id) VALUES (21, 8);
INSERT INTO public.gestor_vaga (gestor_id, vaga_id) VALUES (22, 9);
INSERT INTO public.gestor_vaga (gestor_id, vaga_id) VALUES (22, 10);
INSERT INTO public.gestor_vaga (gestor_id, vaga_id) VALUES (23, 11);
INSERT INTO public.gestor_vaga (gestor_id, vaga_id) VALUES (24, 12);
INSERT INTO public.gestor_vaga (gestor_id, vaga_id) VALUES (24, 13);


ALTER TABLE public.gestor_vaga ENABLE TRIGGER ALL;

--
-- Data for Name: habilidade_candidato; Type: TABLE DATA; Schema: public; Owner: -
--

ALTER TABLE public.habilidade_candidato DISABLE TRIGGER ALL;

INSERT INTO public.habilidade_candidato (habilidade_id, candidato_id, nivel) VALUES (1, 5, 5);
INSERT INTO public.habilidade_candidato (habilidade_id, candidato_id, nivel) VALUES (2, 5, 5);
INSERT INTO public.habilidade_candidato (habilidade_id, candidato_id, nivel) VALUES (3, 5, 4);
INSERT INTO public.habilidade_candidato (habilidade_id, candidato_id, nivel) VALUES (4, 5, 5);
INSERT INTO public.habilidade_candidato (habilidade_id, candidato_id, nivel) VALUES (10, 5, 3);
INSERT INTO public.habilidade_candidato (habilidade_id, candidato_id, nivel) VALUES (11, 5, 4);
INSERT INTO public.habilidade_candidato (habilidade_id, candidato_id, nivel) VALUES (5, 6, 5);
INSERT INTO public.habilidade_candidato (habilidade_id, candidato_id, nivel) VALUES (6, 6, 5);
INSERT INTO public.habilidade_candidato (habilidade_id, candidato_id, nivel) VALUES (7, 6, 4);
INSERT INTO public.habilidade_candidato (habilidade_id, candidato_id, nivel) VALUES (3, 6, 4);
INSERT INTO public.habilidade_candidato (habilidade_id, candidato_id, nivel) VALUES (1, 7, 4);
INSERT INTO public.habilidade_candidato (habilidade_id, candidato_id, nivel) VALUES (2, 7, 4);
INSERT INTO public.habilidade_candidato (habilidade_id, candidato_id, nivel) VALUES (3, 7, 3);
INSERT INTO public.habilidade_candidato (habilidade_id, candidato_id, nivel) VALUES (8, 7, 4);
INSERT INTO public.habilidade_candidato (habilidade_id, candidato_id, nivel) VALUES (9, 7, 4);
INSERT INTO public.habilidade_candidato (habilidade_id, candidato_id, nivel) VALUES (1, 8, 3);
INSERT INTO public.habilidade_candidato (habilidade_id, candidato_id, nivel) VALUES (8, 8, 5);
INSERT INTO public.habilidade_candidato (habilidade_id, candidato_id, nivel) VALUES (9, 8, 5);
INSERT INTO public.habilidade_candidato (habilidade_id, candidato_id, nivel) VALUES (11, 8, 4);


ALTER TABLE public.habilidade_candidato ENABLE TRIGGER ALL;

--
-- Data for Name: habilidade_vaga; Type: TABLE DATA; Schema: public; Owner: -
--

ALTER TABLE public.habilidade_vaga DISABLE TRIGGER ALL;

INSERT INTO public.habilidade_vaga (habilidade_id, vaga_id, nivel_habilidade) VALUES (1, 1, 4);
INSERT INTO public.habilidade_vaga (habilidade_id, vaga_id, nivel_habilidade) VALUES (2, 1, 4);
INSERT INTO public.habilidade_vaga (habilidade_id, vaga_id, nivel_habilidade) VALUES (3, 1, 3);
INSERT INTO public.habilidade_vaga (habilidade_id, vaga_id, nivel_habilidade) VALUES (4, 1, 4);
INSERT INTO public.habilidade_vaga (habilidade_id, vaga_id, nivel_habilidade) VALUES (10, 1, 5);
INSERT INTO public.habilidade_vaga (habilidade_id, vaga_id, nivel_habilidade) VALUES (11, 1, 3);
INSERT INTO public.habilidade_vaga (habilidade_id, vaga_id, nivel_habilidade) VALUES (5, 2, 4);
INSERT INTO public.habilidade_vaga (habilidade_id, vaga_id, nivel_habilidade) VALUES (6, 2, 3);
INSERT INTO public.habilidade_vaga (habilidade_id, vaga_id, nivel_habilidade) VALUES (7, 2, 5);
INSERT INTO public.habilidade_vaga (habilidade_id, vaga_id, nivel_habilidade) VALUES (3, 2, 4);
INSERT INTO public.habilidade_vaga (habilidade_id, vaga_id, nivel_habilidade) VALUES (1, 3, 3);
INSERT INTO public.habilidade_vaga (habilidade_id, vaga_id, nivel_habilidade) VALUES (2, 3, 4);
INSERT INTO public.habilidade_vaga (habilidade_id, vaga_id, nivel_habilidade) VALUES (3, 3, 3);
INSERT INTO public.habilidade_vaga (habilidade_id, vaga_id, nivel_habilidade) VALUES (8, 3, 5);
INSERT INTO public.habilidade_vaga (habilidade_id, vaga_id, nivel_habilidade) VALUES (11, 3, 4);


ALTER TABLE public.habilidade_vaga ENABLE TRIGGER ALL;

--
-- Data for Name: informacao_candidato; Type: TABLE DATA; Schema: public; Owner: -
--

ALTER TABLE public.informacao_candidato DISABLE TRIGGER ALL;

INSERT INTO public.informacao_candidato (info_candidato_id, telefone, data_nascimento, resumo, candidato_id, situacao_profissional, formacao_academica, idioma) VALUES (1, '(11) 99999-1111', '1998-05-12', 'Desenvolvedora Full Stack com experiência em C#, .NET, PostgreSQL e desenvolvimento de APIs.', 5, 'empregado', '[{"curso": "Sistemas de Informação", "nivel": "Bacharelado", "instituicao": "Universidade de São Paulo", "ano_conclusao": 2020}]', '[{"nivel": "Nativo", "idioma": "Português"}, {"nivel": "Intermediário", "idioma": "Inglês"}]');
INSERT INTO public.informacao_candidato (info_candidato_id, telefone, data_nascimento, resumo, candidato_id, situacao_profissional, formacao_academica, idioma) VALUES (2, '(11) 98888-2222', '1995-09-20', 'Analista de Dados com experiência em SQL, Python, Power BI e construção de indicadores.', 6, 'empregado', '[{"curso": "Estatística", "nivel": "Bacharelado", "instituicao": "Universidade Estadual de Campinas", "ano_conclusao": 2019}]', '[{"nivel": "Nativo", "idioma": "Português"}, {"nivel": "Avançado", "idioma": "Inglês"}]');
INSERT INTO public.informacao_candidato (info_candidato_id, telefone, data_nascimento, resumo, candidato_id, situacao_profissional, formacao_academica, idioma) VALUES (3, '(21) 97777-3333', '1997-02-15', 'Desenvolvedora de sistemas com experiência em aplicações web e soluções para o setor de saúde.', 7, 'empregado', '[{"curso": "Ciência da Computação", "nivel": "Bacharelado", "instituicao": "Universidade Federal do Rio de Janeiro", "ano_conclusao": 2020}]', '[{"nivel": "Nativo", "idioma": "Português"}, {"nivel": "Intermediário", "idioma": "Inglês"}]');
INSERT INTO public.informacao_candidato (info_candidato_id, telefone, data_nascimento, resumo, candidato_id, situacao_profissional, formacao_academica, idioma) VALUES (4, '(31) 96666-4444', '1999-11-08', 'Desenvolvedor frontend com foco em React, JavaScript e experiência do usuário.', 8, 'desempregado', '[{"curso": "Análise e Desenvolvimento de Sistemas", "nivel": "Tecnólogo", "instituicao": "PUC Minas", "ano_conclusao": 2021}]', '[{"nivel": "Nativo", "idioma": "Português"}, {"nivel": "Básico", "idioma": "Inglês"}]');
INSERT INTO public.informacao_candidato (info_candidato_id, telefone, data_nascimento, resumo, candidato_id, situacao_profissional, formacao_academica, idioma) VALUES (5, '(11) 95555-1010', '1999-03-14', 'Profissional de tecnologia com interesse em desenvolvimento backend, dados e cloud.', 10, 'empregado', '[{"curso": "Ciência da Computação", "nivel": "Bacharelado", "instituicao": "Universidade Presbiteriana Mackenzie", "ano_conclusao": 2021}]', '[{"nivel": "Nativo", "idioma": "Português"}, {"nivel": "Intermediário", "idioma": "Inglês"}]');
INSERT INTO public.informacao_candidato (info_candidato_id, telefone, data_nascimento, resumo, candidato_id, situacao_profissional, formacao_academica, idioma) VALUES (6, '(11) 95555-1111', '1997-07-22', 'Analista de dados com foco em SQL, visualização e apoio à tomada de decisão.', 11, 'empregado', '[{"curso": "Estatística", "nivel": "Bacharelado", "instituicao": "Universidade de São Paulo", "ano_conclusao": 2020}]', '[{"nivel": "Nativo", "idioma": "Português"}, {"nivel": "Avançado", "idioma": "Inglês"}]');
INSERT INTO public.informacao_candidato (info_candidato_id, telefone, data_nascimento, resumo, candidato_id, situacao_profissional, formacao_academica, idioma) VALUES (7, '(21) 95555-1212', '1996-01-09', 'Profissional de operações com experiência em varejo, e-commerce e logística.', 12, 'desempregado', '[{"curso": "Administração", "nivel": "Bacharelado", "instituicao": "Universidade Federal do Rio de Janeiro", "ano_conclusao": 2019}]', '[{"nivel": "Nativo", "idioma": "Português"}, {"nivel": "Intermediário", "idioma": "Espanhol"}]');
INSERT INTO public.informacao_candidato (info_candidato_id, telefone, data_nascimento, resumo, candidato_id, situacao_profissional, formacao_academica, idioma) VALUES (8, '(31) 95555-1313', '1998-10-30', 'Profissional de saúde e recursos humanos com foco em experiência do colaborador.', 13, 'empregado', '[{"curso": "Psicologia", "nivel": "Bacharelado", "instituicao": "PUC Minas", "ano_conclusao": 2021}]', '[{"nivel": "Nativo", "idioma": "Português"}, {"nivel": "Intermediário", "idioma": "Inglês"}]');
INSERT INTO public.informacao_candidato (info_candidato_id, telefone, data_nascimento, resumo, candidato_id, situacao_profissional, formacao_academica, idioma) VALUES (9, '(41) 95555-1414', '2000-06-18', 'Profissional de marketing e design com interesse em conteúdo, marca e experiência digital.', 14, 'desempregado', '[{"curso": "Design", "nivel": "Bacharelado", "instituicao": "Universidade Federal do Paraná", "ano_conclusao": 2022}]', '[{"nivel": "Nativo", "idioma": "Português"}, {"nivel": "Avançado", "idioma": "Inglês"}]');


ALTER TABLE public.informacao_candidato ENABLE TRIGGER ALL;

--
-- Data for Name: usuario_area; Type: TABLE DATA; Schema: public; Owner: -
--

ALTER TABLE public.usuario_area DISABLE TRIGGER ALL;

INSERT INTO public.usuario_area (usuario_id, area_ocupacao_id) VALUES (5, 1);
INSERT INTO public.usuario_area (usuario_id, area_ocupacao_id) VALUES (5, 4);
INSERT INTO public.usuario_area (usuario_id, area_ocupacao_id) VALUES (6, 4);
INSERT INTO public.usuario_area (usuario_id, area_ocupacao_id) VALUES (7, 1);
INSERT INTO public.usuario_area (usuario_id, area_ocupacao_id) VALUES (7, 3);
INSERT INTO public.usuario_area (usuario_id, area_ocupacao_id) VALUES (8, 1);
INSERT INTO public.usuario_area (usuario_id, area_ocupacao_id) VALUES (10, 1);
INSERT INTO public.usuario_area (usuario_id, area_ocupacao_id) VALUES (10, 4);
INSERT INTO public.usuario_area (usuario_id, area_ocupacao_id) VALUES (11, 5);
INSERT INTO public.usuario_area (usuario_id, area_ocupacao_id) VALUES (11, 4);
INSERT INTO public.usuario_area (usuario_id, area_ocupacao_id) VALUES (12, 6);
INSERT INTO public.usuario_area (usuario_id, area_ocupacao_id) VALUES (12, 8);
INSERT INTO public.usuario_area (usuario_id, area_ocupacao_id) VALUES (13, 3);
INSERT INTO public.usuario_area (usuario_id, area_ocupacao_id) VALUES (13, 2);
INSERT INTO public.usuario_area (usuario_id, area_ocupacao_id) VALUES (14, 13);
INSERT INTO public.usuario_area (usuario_id, area_ocupacao_id) VALUES (14, 18);


ALTER TABLE public.usuario_area ENABLE TRIGGER ALL;

--
-- Name: alternativa_alternativa_id_seq; Type: SEQUENCE SET; Schema: public; Owner: -
--

SELECT pg_catalog.setval('public.alternativa_alternativa_id_seq', 13, false);


--
-- Name: anotacao_anotacao_id_seq; Type: SEQUENCE SET; Schema: public; Owner: -
--

SELECT pg_catalog.setval('public.anotacao_anotacao_id_seq', 4, false);


--
-- Name: area_ocupacao_area_ocupacao_id_seq; Type: SEQUENCE SET; Schema: public; Owner: -
--

SELECT pg_catalog.setval('public.area_ocupacao_area_ocupacao_id_seq', 33, false);


--
-- Name: comentario_comentario_id_seq; Type: SEQUENCE SET; Schema: public; Owner: -
--

SELECT pg_catalog.setval('public.comentario_comentario_id_seq', 4, false);


--
-- Name: curriculo_candidato_curriculo_candidato_id_seq; Type: SEQUENCE SET; Schema: public; Owner: -
--

SELECT pg_catalog.setval('public.curriculo_candidato_curriculo_candidato_id_seq', 5, false);


--
-- Name: empresa_empresa_id_seq; Type: SEQUENCE SET; Schema: public; Owner: -
--

SELECT pg_catalog.setval('public.empresa_empresa_id_seq', 21, false);


--
-- Name: entrevista_entrevista_id_seq; Type: SEQUENCE SET; Schema: public; Owner: -
--

SELECT pg_catalog.setval('public.entrevista_entrevista_id_seq', 4, false);


--
-- Name: experiencia_profissional_experiencia_profissional_id_seq; Type: SEQUENCE SET; Schema: public; Owner: -
--

SELECT pg_catalog.setval('public.experiencia_profissional_experiencia_profissional_id_seq', 8, false);


--
-- Name: habilidade_habilidade_id_seq; Type: SEQUENCE SET; Schema: public; Owner: -
--

SELECT pg_catalog.setval('public.habilidade_habilidade_id_seq', 13, false);


--
-- Name: informacao_candidato_info_candidato_id_seq; Type: SEQUENCE SET; Schema: public; Owner: -
--

SELECT pg_catalog.setval('public.informacao_candidato_info_candidato_id_seq', 10, false);


--
-- Name: processo_seletivo_processo_seletivo_id_seq; Type: SEQUENCE SET; Schema: public; Owner: -
--

SELECT pg_catalog.setval('public.processo_seletivo_processo_seletivo_id_seq', 4, false);


--
-- Name: questao_questao_id_seq; Type: SEQUENCE SET; Schema: public; Owner: -
--

SELECT pg_catalog.setval('public.questao_questao_id_seq', 7, false);


--
-- Name: setor_ocupacao_setor_ocupacao_id_seq; Type: SEQUENCE SET; Schema: public; Owner: -
--

SELECT pg_catalog.setval('public.setor_ocupacao_setor_ocupacao_id_seq', 80, false);


--
-- Name: usuario_usuario_id_seq; Type: SEQUENCE SET; Schema: public; Owner: -
--

SELECT pg_catalog.setval('public.usuario_usuario_id_seq', 30, false);


--
-- Name: vaga_vaga_id_seq; Type: SEQUENCE SET; Schema: public; Owner: -
--

SELECT pg_catalog.setval('public.vaga_vaga_id_seq', 14, false);


--
-- PostgreSQL database dump complete
--

\unrestrict PrYJ9744wApKLg2rMccNhFxcMEDBU16dpP3iMWg9Sv6AdmcXFxP8NsyURKVAcIG

