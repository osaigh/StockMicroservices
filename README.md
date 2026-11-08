# Stock Microservices

A reference stock-trading demo built as a set of small services. It shows authentication with
IdentityServer, an Ocelot API gateway, a MongoDB-backed ASP.NET Core API, RabbitMQ messaging with
MassTransit, a React web client, and a background market-price publisher.

Everything runs with Docker Compose for local development or with Kubernetes (`k8/`) when you want
to exercise the deployment path.

## Architecture

```
Browser
  |
  | :44100
  v
React web client (nginx)
  |                \
  | OIDC redirect   \ REST + bearer token
  v                  v
IdentityServer     Ocelot API Gateway :44405
(Duende 6)           |
                     v
                  Stock API :44300 (direct)
                   |          \
                   v           v
                 MongoDB     RabbitMQ <--- StockMarketUpdater
```

| Service | Local URL | Purpose |
| --- | --- | --- |
| Web client | http://localhost:44100 | React SPA |
| IdentityServer | http://localhost:44401 | Login, OIDC discovery, tokens |
| API gateway | http://localhost:44405 | Public API entry point |
| Stock API | http://localhost:44300/swagger | Direct API access and Swagger |
| MongoDB | `localhost:27017` | Persistence |
| Mongo Express | http://localhost:8081 | Database UI |
| RabbitMQ management | http://localhost:15672 | Queues (user/password from `.env`) |

Demo login: **bob / password**

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) (for running or testing services outside Docker)
- Docker Desktop (Compose v2)
- Node.js 20+ (only if you want to run the React client outside Docker)
- `kubectl` with Kustomize support (for the Kubernetes path)

## Quick start with Docker Compose

```bash
docker compose up --build
```

Then open <http://localhost:44100>, sign in with `bob` / `password`, and the stock table loads through
the gateway.

Useful commands:

```bash
docker compose ps
docker compose logs -f stockapi
docker compose down          # keep data
docker compose down -v       # delete MongoDB, RabbitMQ and identity keys
```

The compose file builds every image from this repository. Override the development credentials by
copying `.env.example` to `.env` if you want different values.

## Quick start with Kubernetes

The manifests use Kustomize, have readiness/liveness probes, resource requests/limits, and persistent
volumes for MongoDB, RabbitMQ and the identity signing keys. Services are `ClusterIP`; use port
forwarding for local access.

Image builds require Node.js 20+ because the React client is built on the host. This avoids relying
on npm access from inside Docker. Use `-RebuildClient` / `--rebuild-client` after changing frontend code.

### Docker Desktop

```powershell
.\k8\deploy.ps1 -ForwardPorts
```

or:

```bash
bash k8/deploy.sh
bash k8/port-forward.sh
```

### minikube / kind

Build the images inside the cluster's Docker daemon, then deploy:

```bash
# minikube
eval "$(minikube docker-env)"
bash k8/deploy.sh
bash k8/port-forward.sh
```

For kind:

```bash
bash k8/build-images.sh
kind load docker-image stockidentityserver:latest stockapi:latest stockapigateway:latest stockmarketupdater:latest stockwebclient:latest
bash k8/deploy.sh --skip-build
bash k8/port-forward.sh
```

The build scripts are:

- `k8/build-images.ps1` / `k8/build-images.sh` — rebuilds the React bundle, the nginx image, and the .NET images.
- `k8/deploy.ps1` / `k8/deploy.sh` — builds images (unless `-SkipBuild` / `--skip-build`), applies Kustomize, restarts the app deployments, and waits for rollout.
- `k8/port-forward.ps1` / `k8/port-forward.sh` — exposes the cluster services on the same local ports used by Compose.

If Node.js is not available, build everything with `docker compose build` and then run
`deploy.ps1 -SkipBuild` / `deploy.sh --skip-build`.

Port forwarding is tied to the current pod. If you rebuild and redeploy, stop the old
`port-forward` script and start it again.

If Docker cannot reach `registry.npmjs.org`, the k8 build scripts still work because the React
bundle is built on the host first.

To publish images to a registry, override them without editing manifests:

```bash
kubectl kustomize k8 | sed 's#stockapi:latest#registry.example.com/stockapi:1.0.0#' | kubectl apply -f -
# or, from a kustomize overlay:
kustomize edit set image stockapi=registry.example.com/stockapi:1.0.0
```

The Kubernetes secret in `k8/secret.yaml` contains **development credentials**. Replace it before
deploying anywhere shared.

## Running services locally without full Compose

Start the infrastructure:

```bash
docker compose up -d mongo rabbitmq
```

Then run the services in separate terminals:

```bash
# Identity server (http://localhost:44401)
dotnet run --project StockMicroservices.IdentityServer

# API (http://localhost:44300)
dotnet run --project StockMicroservices.API --launch-profile StockMicroservices.API

# Gateway (http://localhost:44405)
dotnet run --project StockMicroservices.APIGateway

# Market simulator
dotnet run --project StockMicroservices.StockMarketUpdater --launch-profile StockMicroservices.StockMarketUpdater

# React client (http://localhost:44100)
cd StockMicroservices.WebClient/ClientApp
npm ci
npm start
```

The API seeds seven demo stocks on first start when the MongoDB collection is empty. Seeding is
controlled by `Database:SeedOnStartup` and is safe to leave enabled on a fresh demo environment.

## Configuration

Each .NET service reads normal ASP.NET Core configuration (`appsettings.json`, environment variables,
user secrets). Environment variables use `__` for nested keys (for example `RabbitMq__Hostname`).

| Setting | Used by | Compose default | Kubernetes value |
| --- | --- | --- | --- |
| `IdentityServerUrl` | API, gateway | `http://stockidentityserver` | `http://stockidentityserver` |
| `APIScope` | API, gateway | `StockMicroservicesAPI` | `StockMicroservicesAPI` |
| `Database__ConnectionString` | API | `mongodb://root:admin1@mongo:27017/?authSource=admin` | from Secret |
| `Database__Name` | API | `stocks` | `stocks` |
| `Database__SeedOnStartup` | API | `true` | `true` |
| `RabbitMq__Hostname` / `Port` | API, updater | `rabbitmq` / `5672` | `rabbitmq` / `5672` |
| `RabbitMq__Username` / `Password` | API, updater | from `.env` | from Secret |
| `WebClientUrl` | IdentityServer | `http://localhost:44100` | `http://localhost:44100` |
| `Cors__AllowedOrigins__0` | API, gateway, identity | `http://localhost:44100` | `http://localhost:44100` |

The React client is built with `REACT_APP_IDENTITY_URL`, `REACT_APP_API_URL` and
`REACT_APP_IDENTITY_CLIENT_ID`. Compose passes them as Docker build arguments; running `npm start`
uses `.env.development` and serves on port 44100.

## Tests

```bash
dotnet test
```

Unit tests cover the controllers. Integration tests use `WebApplicationFactory` with an in-process
MongoDB instance from Mongo2Go, so Docker does not need to be running.

## Solution layout

| Path | Description |
| --- | --- |
| `StockMicroservices.API` | ASP.NET Core API, MongoDB repository, MassTransit consumer, health checks |
| `StockMicroservices.API.Tests` | Unit and integration tests |
| `StockMicroservices.APIGateway` | Ocelot gateway with JWT validation |
| `StockMicroservices.IdentityServer` | Duende IdentityServer, login/registration UI |
| `StockMicroservices.StockMarketUpdater` | Background publisher of simulated price changes |
| `StockMicroservices.EventBus.Common` | Shared message contracts and queue names |
| `StockMicroservices.WebClient/ClientApp` | React SPA served by nginx |
| `k8` | Kustomize manifests and deploy/port-forward scripts |
| `docker-compose.yml` | Local multi-container environment |

## Production notes

This project is a demo. Before exposing it beyond a local environment:

- Replace all development credentials and store them in a secret manager.
- Terminate TLS in front of the gateway and identity server, and enable `IdentityServer:RequireHttpsMetadata`.
- Replace the demo developer signing key with a real certificate (`IdentityServer:CertificatePath` and
  `IdentityServer:CertificatePassword`), or keep the `identity-keys` volume safe and backed up.
- Identity users are stored in an in-memory EF Core database and reset on restart. Point
  `ApplicationDbContext` at a persistent database for real use.
- The SPA uses the implicit flow for demo simplicity. Prefer authorization code + PKCE, and review
  Duende IdentityServer licensing for production.
- Put an Ingress in front of the services instead of using `kubectl port-forward`.
- Pin image tags and add a CI pipeline that runs `dotnet test`, builds the images, and scans them.
