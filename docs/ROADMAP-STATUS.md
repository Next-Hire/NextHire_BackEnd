# Roadmap da API — status

## Fase 0 · Segurança e estabilidade — concluída
- [Authorize] reativado nos 5 endpoints que estavam comentados
- Role `Administrador` padronizada para `Adm`
- `FallbackPolicy` exige autenticação por padrão (endpoints públicos usam `[AllowAnonymous]`)
- Middleware global de exceção (ProblemDetails: 400/401/404/500)
- `StatusCode(204, ...)` substituído por `NoContent()` em todos os controllers
- Listas vazias retornam `200 []` (processos, candidaturas, setores)
- Rate limit no `POST /api/v1/Usuario/enviar-codigo` (5 req/10 min por IP)
- Código de verificação por usuário (removido estado estático; uso via Redis)
- CORS restrito por `CORS_ORIGINS` (`.env`)
- Rotação de segredos documentada em `docs/SEGREDOS.md` + `.env.example`

## Fase 1 · Padronização de contrato — concluída
- Rotas de Curriculo e Entrevista migradas para `api/v1/`
- Enums serializados como string `snake_case` (ex.: `avanca`, `nao_avanca`)
- Datas em ISO 8601 com timezone (`2026-12-02T14:00:00Z`)
- Paginação (`pagina`, `tamanhoPagina`) em Processo Seletivo e Entrevista (retrocompatível)
- Swagger com comentários XML e exemplos de datas
- Testes de contrato em `tests/contract.sh`

## Fase 2 · Infraestrutura de e-mail — concluída
- `IEmailService` + SMTP por env, 12 templates HTML com fallback texto
- Outbox persistida com retry e idempotência; preferências de notificação; logs

## Fase 3 · Gatilhos por persona — concluída
- Candidato, Analista RH e Gestor (9 gatilhos; marcos configuráveis)

## Fase 4 · Qualidade contínua — concluída (base)
- Health checks em `/health` e `/health/live`
- CI de build em `.github/workflows/ci.yml`
- Testes de integração em `tests/integration.sh`
- Documentação de rotas em `docs/ROTAS.md`

### Pendências opcionais
- Paginação nas demais listas (Vaga, Candidato, etc.)
- Testes de integração automatizados no CI com serviços Postgres/Redis
- Rebuild da imagem `eukauecmp/nexthire-db` com o schema atualizado
