# AGENTS.md

## Projeto

Backend da aplicação NextHire. API Web em .NET 10 (ASP.NET Core), com Entity Framework Core (PostgreSQL), autenticação JWT, cache Redis, integração AWS S3 e documentação Swagger.

## Estrutura (raiz)

- `NextHire_API/` — projeto principal da API (`NextHire_API.csproj`).
- `database/` — scripts SQL (`CREATE/`, `INSERT/`).

## Stack / dependências principais

- .NET 10 (`net10.0`), ASP.NET Core Web SDK
- Entity Framework Core 10 + Npgsql (PostgreSQL)
- JWT Bearer authentication
- Swagger / Swashbuckle
- StackExchange.Redis
- AWSSDK.S3
- Argon2 (Konscious) para hashing
- DotNetEnv para variáveis de ambiente

## Convenções

- Código organizado em camadas: `Controllers`, `Domains`, `DTOs`, `Repository`, `Contexts`, `Interfaces`, `Applications`, `Exceptions`.
- Configurações de build/artefatos (`bin/`, `obj/`) não devem ser editadas manualmente.
- Prompts do agente em `NextHire_API/Config/NextAgent/`.

## Comandos úteis

- Restaurar/build: `dotnet build NextHire_API/NextHire_API.csproj`
- Executar: `dotnet run --project NextHire_API/NextHire_API.csproj`
