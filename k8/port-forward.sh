#!/usr/bin/env bash
set -euo pipefail

kubectl -n stock port-forward svc/stockwebclient 44100:80 &
kubectl -n stock port-forward svc/stockidentityserver 44401:80 &
kubectl -n stock port-forward svc/stockapigateway 44405:80 &
kubectl -n stock port-forward svc/stockapi 44300:80 &
kubectl -n stock port-forward svc/mongo-express 8081:8081 &

trap 'kill $(jobs -p) 2>/dev/null || true' EXIT INT TERM

echo
echo "Open http://localhost:44100. Press Ctrl+C to stop forwarding."
wait
