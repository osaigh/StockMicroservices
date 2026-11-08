# Stock web client

React 18 SPA for the Stock Microservices demo.

```bash
npm ci
npm start          # http://localhost:44100 (uses the client_id_react OAuth client)
npm run build      # production bundle in build/
npm test
```

Runtime endpoints are configured at build time:

| Variable | Default |
| --- | --- |
| `REACT_APP_IDENTITY_URL` | `http://localhost:44401` |
| `REACT_APP_API_URL` | `http://localhost:44405` |
| `REACT_APP_IDENTITY_CLIENT_ID` | `client_id_react` |

The Docker image is built by `docker-compose.yml`; in Kubernetes the image is built from the same
Dockerfile with the same build arguments. See the root `README.md` for the full setup.
