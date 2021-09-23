# pull official base image
FROM node:14.1-alpine AS builder

# set working directory
WORKDIR /app

# add `/app/node_modules/.bin` to $PATH
ENV PATH /app/node_modules/.bin:$PATH

# install app dependencies
COPY ["StockMicroservices.WebClient/ClientApp/package.json", "./"]
COPY ["StockMicroservices.WebClient/ClientApp/package-lock.json", "./"]
RUN npm install --silent
RUN npm install react-scripts@4.0.3 -g --silent

# add app
COPY ["StockMicroservices.WebClient/ClientApp", "./"]
RUN npm run build

FROM nginx:1.17-alpine
RUN apk --no-cache add curl
RUN curl -L https://github.com/a8m/envsubst/releases/download/v1.1.0/envsubst-`uname -s`-`uname -m` -o envsubst && \
    chmod +x envsubst && \
    mv envsubst /usr/local/bin
COPY "StockMicroservices.WebClient/ClientApp/nginx.config" /etc/nginx/nginx.template
CMD ["/bin/sh", "-c", "envsubst < /etc/nginx/nginx.template > /etc/nginx/conf.d/default.conf && nginx -g 'daemon off;'"]
COPY --from=builder /app/build /usr/share/nginx/html
COPY --from=builder /app/.cert /etc/ssl