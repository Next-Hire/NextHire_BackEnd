# Rotação de segredos

Checklist para rotacionar credenciais sem interromper o ambiente:

1. **JWT_KEY**: gere uma nova chave (`openssl rand -base64 48`), atualize `JWT_KEY` no `.env`/secret manager e reinicie a API. Tokens antigos deixam de valer — avise o front.
2. **SMTP (Gmail app password)**: revogue a senha de app atual, gere outra, atualize `SMTP_PASS`/`MAIL_CODE_PASS` e reinicie.
3. **Banco**: crie o novo usuário/senha no Postgres, atualize `CONNECTION_STRING` (e `POSTGRES_PASSWORD` no compose) e reinicie API e containers.
4. **Redis**: habilite `requirepass`, atualize `REDIS_CONNECTION` e reinicie.
5. **AWS / Gemini**: gere novas chaves nos consoles, atualize `AWS_*`/`GEMINI_API_KEY`, revogue as antigas.
6. **Fora do Git**: mantenha `.env` no `.gitignore` (já está). Use `.env.example` como referência.
7. **Após rotação**: reinicie a API, rode `tests/contract.sh` e valide login + envio de e-mail.
