#!/usr/bin/env bash
# Testes de contrato (smoke) — requer API em execução.
# Uso: BASE_URL=http://localhost:5052 ./tests/contract.sh
set -u
BASE_URL="${BASE_URL:-http://localhost:5052}"
falhas=0

checar() {
  local descricao="$1" esperado="$2" obtido="$3"
  if [ "$obtido" = "$esperado" ]; then
    echo "OK   [$obtido] $descricao"
  else
    echo "FALHA [$obtido != $esperado] $descricao"
    falhas=$((falhas + 1))
  fi
}

codigo() { curl -s -o /dev/null -w "%{http_code}" "$@"; }

checar "health check" 200 "$(codigo "$BASE_URL/health")"
checar "health/live" 200 "$(codigo "$BASE_URL/health/live")"
checar "swagger disponível" 200 "$(codigo "$BASE_URL/swagger/index.html")"
checar "rota protegida sem token retorna 401" 401 "$(codigo "$BASE_URL/api/v1/ProcessoSeletivo")"
checar "rota v1 de Curriculo existe" 401 "$(codigo "$BASE_URL/api/v1/Curriculo")"
checar "rota v1 de Entrevista existe" 401 "$(codigo "$BASE_URL/api/v1/Entrevista")"

echo
if [ "$falhas" -eq 0 ]; then echo "Contrato OK"; else echo "$falhas falha(s)"; exit 1; fi
