#!/usr/bin/env bash
set -euo pipefail

ROOT="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
cd "$ROOT"

if [[ "${1:-}" != "--skip-build" ]]; then
  echo "Building container images..."
  bash "$ROOT/k8/build-images.sh"

  for deployment in stockidentityserver stockapi stockapigateway stockmarketupdater stockwebclient
  do
    echo "Restarting deployment/$deployment to pick up the new image..."
    kubectl -n stock rollout restart "deployment/$deployment"
  done
fi

echo "Applying Kubernetes manifests from k8/..."
kubectl apply -k k8

for workload in \
  statefulset/mongo \
  statefulset/rabbitmq \
  deployment/mongo-express \
  deployment/stockidentityserver \
  deployment/stockapi \
  deployment/stockapigateway \
  deployment/stockmarketupdater \
  deployment/stockwebclient
do
  echo "Waiting for $workload..."
  kubectl -n stock rollout status "$workload" --timeout=180s
done

cat <<'EOF'

Deployment complete.
  Web client  : http://localhost:44100
  Identity    : http://localhost:44401
  API gateway : http://localhost:44405
  API (direct): http://localhost:44300/swagger
  Mongo UI    : http://localhost:8081

Run ./k8/port-forward.sh to expose the services locally.
EOF
