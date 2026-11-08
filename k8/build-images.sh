#!/usr/bin/env bash
set -euo pipefail

ROOT="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
CLIENT="$ROOT/StockMicroservices.WebClient/ClientApp"
cd "$ROOT"

if command -v npm >/dev/null 2>&1; then
  echo "Building the React client on the host..."
  pushd "$CLIENT" >/dev/null
  if [[ "${1:-}" == "--rebuild-client" || ! -d node_modules ]]; then
    npm ci --no-audit --no-fund
  fi
  npm run build
  popd >/dev/null
elif [[ -f "$CLIENT/build/index.html" ]]; then
  echo "npm is not installed; reusing the existing React build in ClientApp/build."
else
  echo "Node.js/npm is required to build the web client." >&2
  echo "Install Node 20+, or build the image with 'docker compose build'." >&2
  exit 1
fi

echo "Building the web client image from the prebuilt bundle..."
docker build -f "$CLIENT/Dockerfile.prebuilt" -t stockwebclient:latest "$CLIENT"

echo "Building the .NET service images..."
docker compose build stockidentityserver stockapi stockapigateway stockmarketupdater
