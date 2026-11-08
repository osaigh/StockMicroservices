FROM node:22-bookworm-slim AS build

WORKDIR /app

COPY StockMicroservices.WebClient/ClientApp/package.json StockMicroservices.WebClient/ClientApp/package-lock.json ./
RUN npm ci --no-audit --no-fund

COPY StockMicroservices.WebClient/ClientApp/ ./

ARG REACT_APP_IDENTITY_URL=http://localhost:44401
ARG REACT_APP_API_URL=http://localhost:44405
ARG REACT_APP_IDENTITY_CLIENT_ID=client_id_react
ENV REACT_APP_IDENTITY_URL=$REACT_APP_IDENTITY_URL \
    REACT_APP_API_URL=$REACT_APP_API_URL \
    REACT_APP_IDENTITY_CLIENT_ID=$REACT_APP_IDENTITY_CLIENT_ID

RUN npm run build

FROM nginx:1.27-alpine AS final

COPY StockMicroservices.WebClient/ClientApp/nginx.conf /etc/nginx/conf.d/default.conf
COPY --from=build /app/build /usr/share/nginx/html

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]
