#!/usr/bin/env bash
# Testes de integração dos fluxos críticos — requer API + banco com seed.
# Uso: BASE_URL=... TOKEN_RH=... TOKEN_CANDIDATO=... ./tests/integration.sh
set -u
BASE_URL="${BASE_URL:-http://localhost:5052}"
TOKEN_RH="${TOKEN_RH:-}"
TOKEN_CANDIDATO="${TOKEN_CANDIDATO:-}"
falhas=0

checar() {
  local descricao="$1" esperado="$2" obtido="$3"
  if [ "$obtido" = "$esperado" ]; then echo "OK   [$obtido] $descricao"; else echo "FALHA [$obtido != $esperado] $descricao"; falhas=$((falhas + 1)); fi
}

checar "lista de processos (RH)" 200 "$(curl -s -o /dev/null -w '%{http_code}' -H "Authorization: Bearer $TOKEN_RH" "$BASE_URL/api/v1/ProcessoSeletivo")"
checar "lista de entrevistas (RH)" 200 "$(curl -s -o /dev/null -w '%{http_code}' -H "Authorization: Bearer $TOKEN_RH" "$BASE_URL/api/v1/Entrevista?pagina=1&tamanhoPagina=10")"
checar "fluxo de candidatura (Candidato)" 201 "$(curl -s -o /dev/null -w '%{http_code}' -X POST -H "Authorization: Bearer $TOKEN_CANDIDATO" "$BASE_URL/api/v1/CurriculoVaga/EfetuarCandidatura/curriculo/1/vaga/1")"

echo
if [ "$falhas" -eq 0 ]; then echo "Integração OK"; else echo "$falhas falha(s)"; exit 1; fi
