FROM postgres:18

ENV POSTGRES_USER=nexthire
ENV POSTGRES_DB=nexthire_db

COPY CREATE/nexthire_db.sql /docker-entrypoint-initdb.d/01_nexthire_db.sql
COPY INSERT/nexthire_database_insert.sql /docker-entrypoint-initdb.d/02_nexthire_database_insert.sql
