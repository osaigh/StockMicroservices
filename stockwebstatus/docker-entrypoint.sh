#!/bin/sh

echo "Generating config.json..."

cat <<EOF > /usr/share/nginx/html/config.json
{
  "WEBSTATUSAPI": "${WEBSTATUSAPI}"
}
EOF

exec "$@"
