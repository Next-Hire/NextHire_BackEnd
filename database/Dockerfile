# Usa a imagem oficial do PostgreSQL
FROM postgres:latest

# Define variáveis de ambiente padrão para o banco (substitua pelos seus dados)
ENV POSTGRES_USER=nexthire
ENV POSTGRES_PASSWORD=nexthire
ENV POSTGRES_DB=nexthire_db

# Copia o script da pasta CREATE para o diretório de inicialização do Postgres
COPY CREATE/nexthire_db.sql /docker-entrypoint-initdb.d/