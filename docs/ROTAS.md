# Rotas da API NextHire

Gerado automaticamente a partir dos controllers.

| Método | Rota | Acesso |
|---|---|---|
| GET | `api/v1/Anotacao` | Perfis: Adm |
| GET | `api/v1/Anotacao/{id}` | Perfis: Candidato, Adm |
| POST | `api/v1/Anotacao` | Perfis: Candidato, Adm |
| PUT | `api/v1/Anotacao/{id}` | Perfis: Candidato, Adm |
| DELETE | `api/v1/Anotacao/{id}` | Perfis: Candidato, Adm |
| GET | `api/v1/AreaOcupacao` | Público (anônimo) |
| GET | `api/v1/AreaOcupacao/{id}` | Público (anônimo) |
| GET | `api/v1/AreaOcupacao/nome/{nome}` | Público (anônimo) |
| POST | `api/v1/AreaOcupacao` | Perfis: Adm |
| PUT | `api/v1/AreaOcupacao/{id}` | Perfis: Adm |
| DELETE | `api/v1/AreaOcupacao/{id}` | Perfis: Adm |
| POST | `api/v1/Autenticacao` | Público (anônimo) |
| GET | `api/v1/AvaliacaoHabilidade` | Perfis: Gestor, AnalistaRh, Adm |
| GET | `api/v1/AvaliacaoHabilidade/candidato/{candidatoId:int}` | Perfis: Candidato, Gestor, AnalistaRh, Adm |
| GET | `api/v1/AvaliacaoHabilidade/{candidatoId:int}/{entrevistaId:int}/{habilidadeId:int}` | Perfis: Candidato, Gestor, AnalistaRh, Adm |
| POST | `api/v1/AvaliacaoHabilidade` | Perfis: AnalistaRh, Adm |
| PUT | `api/v1/AvaliacaoHabilidade/{candidatoId:int}/{entrevistaId:int}/{habilidadeId:int}` | Perfis: AnalistaRh, Adm |
| DELETE | `api/v1/AvaliacaoHabilidade/{candidatoId:int}/{entrevistaId:int}/{habilidadeId:int}` | Perfis: AnalistaRh, Adm |
| GET | `api/v1/CandidatoProcessoSeletivo` | Público (anônimo) |
| GET | `api/v1/CandidatoProcessoSeletivo/aprovados/{processoId:int}` | Público (anônimo) |
| GET | `api/v1/CandidatoProcessoSeletivo/candidato/{candidatoId:int}/processo/{processoSeletivoId:int}` | Público (anônimo) |
| GET | `api/v1/CandidatoProcessoSeletivo/processo/{processoSeletivoId:int}` | Público (anônimo) |
| GET | `api/v1/CandidatoProcessoSeletivo/candidato/{candidatoId:int}` | Público (anônimo) |
| GET | `api/v1/CandidatoProcessoSeletivo/processo/candidatos/{processoSeletivoId:int}` | Público (anônimo) |
| POST | `api/v1/CandidatoProcessoSeletivo` | Perfis: Adm, AnalistaRh, Gestor |
| POST | `api/v1/CandidatoProcessoSeletivo/candidatos` | Perfis: Adm, AnalistaRh, Gestor |
| PUT | `api/v1/CandidatoProcessoSeletivo/candidato/{candidatoId}/processo/{processoSeletivoId}` | Perfis: Adm, AnalistaRh, Gestor |
| DELETE | `api/v1/CandidatoProcessoSeletivo/candidato/{candidatoId:int}/processo/{processoSeletivoId:int}` | Perfis: Adm, AnalistaRh, Gestor |
| GET | `api/v1/CandidatoQuestao/existe` | Perfis: Candidato, Gestor, AnalistaRh, Adm |
| GET | `api/v1/CandidatoQuestao/questao/{questaoId}/candidato/{candidatoId}` | Perfis: Candidato, Gestor, AnalistaRh, Adm |
| GET | `api/v1/CandidatoQuestao/candidato/{candidatoId}` | Perfis: Candidato, Gestor, AnalistaRh, Adm |
| GET | `api/v1/CandidatoQuestao/questao/{questaoId}` | Perfis: Candidato, Gestor, AnalistaRh, Adm |
| GET | `api/v1/CandidatoQuestao/candidato/{candidatoId}/vaga/{vagaId}` | Perfis: Candidato, Gestor, AnalistaRh, Adm |
| GET | `api/v1/CandidatoQuestao/em-branco/candidato/{candidatoId}/vaga/{vagaId}` | Perfis: Candidato, Gestor, AnalistaRh, Adm |
| GET | `api/v1/CandidatoQuestao/certas/candidato/{candidatoId}` | Perfis: Candidato, Gestor, AnalistaRh, Adm |
| POST | `api/v1/CandidatoQuestao` | Perfis: Candidato, AnalistaRh, Adm |
| DELETE | `api/v1/CandidatoQuestao/candidato/{candidatoId}/questao/{questaoId}` | Perfis: AnalistaRh, Adm |
| GET | `api/v1/Comentario/comentarios` | Perfis: Gestor, AnalistaRh, Adm |
| GET | `api/v1/Comentario/entrevista/{entrevistaId}` | Perfis: Gestor, AnalistaRh, Adm |
| GET | `api/v1/Comentario/gestor/{gestorId}` | Perfis: Gestor, AnalistaRh, Adm |
| GET | `api/v1/Comentario/{comentarioId}` | Perfis: Gestor, AnalistaRh, Adm |
| POST | `api/v1/Comentario` | Perfis: Gestor |
| PATCH | `api/v1/Comentario/{comentarioId}` | Perfis: Gestor, Adm |
| DELETE | `api/v1/Comentario/{comentarioId}` | Perfis: Gestor, Adm |
| GET | `api/v1/Curriculo/GerarLinkPelaAWS` | Autenticado (fallback) |
| GET | `api/v1/Curriculo/Listagem` | Autenticado (fallback) |
| POST | `api/v1/Curriculo/CriarCurriculoCandidato/candidato/{candidatoId}/json` | Autenticado (fallback) |
| POST | `api/v1/Curriculo/CriarCurriculoCandidato/candidato/{candidatoId}/arquivo` | Autenticado (fallback) |
| POST | `api/v1/Curriculo/curriculo/gerarJSON` | Autenticado (fallback) |
| GET | `api/v1/CurriculoVaga/Candidaturas/empresa/{empresaId}` | Perfis: AnalistaRh, Adm |
| GET | `api/v1/CurriculoVaga/CandidatosPorVagaId/{vagaId}` | Perfis: AnalistaRh, Adm |
| GET | `api/v1/CurriculoVaga/VagasPorCandidatoId/{candidatoId}` | Perfis: Candidato, Gestor, AnalistaRh, Adm |
| POST | `api/v1/CurriculoVaga/EfetuarCandidatura/curriculo/{curriculoId}/vaga/{vagaId}` | Perfis: Candidato |
| DELETE | `api/v1/CurriculoVaga/CancelarCandidatura/curriculo/{curriculoId}/vaga/{vagaId}` | Perfis: Candidato |
| POST | `api/v1/CurriculoVaga/AvaliarCandidatura/vaga/{vagaId}/candidato/{candidatoId}` | Perfis: AnalistaRh, Adm |
| GET | `api/v1/CurriculoVaga/Avaliacao/vaga/{vagaId}/candidato/{candidatoId}` | Perfis: AnalistaRh, Adm |
| GET | `api/v1/CurriculoVaga/AvaliacoesPorVagaId/{vagaId}` | Perfis: AnalistaRh, Adm |
| GET | `api/v1/Empresa` | Público (anônimo) |
| GET | `api/v1/Empresa/BuscarPorNome/{nome}` | Público (anônimo) |
| GET | `api/v1/Empresa/BuscarPorId/{id}` | Público (anônimo) |
| POST | `api/v1/Empresa` | Perfis: Adm |
| PUT | `api/v1/Empresa/Atualizar/{id}` | Perfis: Adm |
| DELETE | `api/v1/Empresa` | Perfis: Adm |
| GET | `api/v1/Entrevista` | Autenticado (fallback) |
| GET | `api/v1/Entrevista/status` | Autenticado (fallback) |
| GET | `api/v1/Entrevista/hoje` | Autenticado (fallback) |
| GET | `api/v1/Entrevista/candidato/{candidatoId:int}` | Autenticado (fallback) |
| GET | `api/v1/Entrevista/{id:int}` | Autenticado (fallback) |
| POST | `api/v1/Entrevista` | Autenticado (fallback) |
| PATCH | `api/v1/Entrevista/{entrevistaId:int}/candidato/{candidatoId:int}` | Autenticado (fallback) |
| PATCH | `api/v1/Entrevista/{entrevistaId:int}/data` | Autenticado (fallback) |
| DELETE | `api/v1/Entrevista/{id:int}` | Autenticado (fallback) |
| GET | `api/v1/ExperienciaProfissional` | Perfis: Candidato, Gestor, AnalistaRh, Adm |
| GET | `api/v1/ExperienciaProfissional/{experienciaProfissionalId}` | Perfis: Candidato, Gestor, AnalistaRh, Adm |
| POST | `api/v1/ExperienciaProfissional` | Perfis: Candidato, Adm |
| PUT | `api/v1/ExperienciaProfissional/{experienciaProfissionalId}` | Perfis: Candidato, Adm |
| DELETE | `api/v1/ExperienciaProfissional` | Perfis: Candidato, Adm |
| GET | `api/v1/Habilidade/rbosf/codigo/{codigo}` | Público (anônimo) |
| GET | `api/v1/Habilidade/rbosf/termo/{termo}` | Público (anônimo) |
| POST | `api/v1/Habilidade/rbosf/importar` | Perfis: AnalistaRh, Adm |
| GET | `api/v1/Habilidade` | Público (anônimo) |
| GET | `api/v1/Habilidade/{id}` | Público (anônimo) |
| POST | `api/v1/Habilidade` | Perfis: AnalistaRh, Adm |
| PUT | `api/v1/Habilidade/{id}` | Perfis: AnalistaRh, Adm |
| DELETE | `api/v1/Habilidade/{id}` | Perfis: AnalistaRh, Adm |
| GET | `api/v1/InformacaoCandidato` | Perfis: Gestor, AnalistaRh, Adm |
| GET | `api/v1/InformacaoCandidato/{id:int}` | Perfis: Candidato, Gestor, AnalistaRh, Adm |
| GET | `api/v1/InformacaoCandidato/candidato/{candidatoId:int}` | Perfis: Candidato, Gestor, AnalistaRh, Adm |
| POST | `api/v1/InformacaoCandidato` | Perfis: Candidato, Adm |
| PUT | `api/v1/InformacaoCandidato/{id:int}` | Perfis: Candidato, Adm |
| DELETE | `api/v1/InformacaoCandidato/{id:int}` | Perfis: Candidato, Adm |
| GET | `api/v1/ProcessoSeletivo` | Perfis: AnalistaRh, Gestor |
| GET | `api/v1/ProcessoSeletivo/Empresa/{empresaId}` | Perfis: AnalistaRh, Gestor |
| GET | `api/v1/ProcessoSeletivo/Empresa/Setor/{setorId}` | Perfis: AnalistaRh, Gestor |
| GET | `api/v1/ProcessoSeletivo/Empresa/Area/{areaId}` | Perfis: AnalistaRh, Gestor |
| GET | `api/v1/ProcessoSeletivo/ProcessoSeletivo/{processoId}` | Perfis: AnalistaRh, Gestor, Candidato |
| POST | `api/v1/ProcessoSeletivo` | Perfis: AnalistaRh |
| PATCH | `api/v1/ProcessoSeletivo/ProcessoSeletivo/Atualizar/{processoId}` | Perfis: AnalistaRh |
| PATCH | `api/v1/ProcessoSeletivo/ProcessoSeletivo/Remover/{processoId}` | Perfis: AnalistaRh |
| GET | `api/v1/ProcessoSeletivo/ProcessoSeletivo/status/{status}` | Autenticado (fallback) |
| GET | `api/v1/Questao/vaga/{vagaId:int}` | Autenticado (fallback) |
| GET | `api/v1/Questao/{questaoId:int}` | Autenticado (fallback) |
| POST | `api/v1/Questao/vaga/{vagaId:int}/gerar` | Autenticado (fallback) |
| POST | `api/v1/Questao/vaga/{vagaId:int}/gerar-mais` | Autenticado (fallback) |
| POST | `api/v1/Questao/vaga/{vagaId:int}` | Autenticado (fallback) |
| PATCH | `api/v1/Questao/{questaoId:int}` | Autenticado (fallback) |
| DELETE | `api/v1/Questao/{questaoId:int}` | Autenticado (fallback) |
| GET | `api/v1/Questao/verificar/candidato/{candidatoId:int}/vaga/{vagaId:int}` | Autenticado (fallback) |
| GET | `api/v1/Setor_Ocupacao` | Público (anônimo) |
| GET | `api/v1/Setor_Ocupacao/BuscarPorId/{setorId}` | Público (anônimo) |
| GET | `api/v1/Setor_Ocupacao/BuscarPorNome/{setorNome}` | Público (anônimo) |
| GET | `api/v1/Setor_Ocupacao/BuscarPorArea/{areaId}` | Público (anônimo) |
| GET | `api/v1/Setor_Ocupacao/ListarPorEmpresaEArea` | Público (anônimo) |
| GET | `api/v1/Setor_Ocupacao/BuscarPorEmpresa` | Público (anônimo) |
| POST | `api/v1/Setor_Ocupacao` | Perfis: AnalistaRh, Adm |
| PUT | `api/v1/Setor_Ocupacao/Atualizar/{setorId}` | Perfis: AnalistaRh, Adm |
| DELETE | `api/v1/Setor_Ocupacao` | Perfis: AnalistaRh, Adm |
| GET | `api/v1/Usuario/usuarios` | Perfis: AnalistaRh, Adm |
| GET | `api/v1/Usuario/funcionarios` | Perfis: AnalistaRh, Adm |
| GET | `api/v1/Usuario/candidatos` | Perfis: AnalistaRh, Adm |
| GET | `api/v1/Usuario/funcionario/{funcionarioId}` | Perfis: AnalistaRh, Adm |
| GET | `api/v1/Usuario/candidato/{candidatoId}` | Perfis: AnalistaRh, Adm |
| POST | `api/v1/Usuario/funcionarios/{empresaId}` | Perfis: Adm |
| POST | `api/v1/Usuario/candidato` | Público (anônimo) |
| PATCH | `api/v1/Usuario/{usuarioId}` | Autenticado |
| POST | `api/v1/Usuario/enviar-codigo` | Público (anônimo) |
| PUT | `api/v1/Usuario/senha` | Público (anônimo) |
| DELETE | `api/v1/Usuario/{usuarioId}` | Autenticado |
| GET | `api/v1/Vaga/vagas` | Público (anônimo) |
| GET | `api/v1/Vaga/empresa/{empresaId}` | Público (anônimo) |
| GET | `api/v1/Vaga/setor/{setorId}` | Autenticado (fallback) |
| GET | `api/v1/Vaga/titulo` | Público (anônimo) |
| GET | `api/v1/Vaga/status` | Público (anônimo) |
| GET | `api/v1/Vaga/{vagaId}` | Público (anônimo) |
| POST | `api/v1/Vaga` | Perfis: AnalistaRh, Adm |
| PATCH | `api/v1/Vaga/{vagaId}` | Perfis: AnalistaRh, Adm |
| DELETE | `api/v1/Vaga/{vagaId}` | Perfis: AnalistaRh, Adm |
