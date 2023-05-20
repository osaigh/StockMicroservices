# pull official base image
FROM node:16.20-alpine AS builder

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

FROM nginx:1.23-alpine
COPY "StockMicroservices.WebClient/ClientApp/nginx.config" /etc/nginx/conf.d/default.conf
EXPOSE 80
COPY --from=builder /app/build /usr/share/nginx/html
CMD ["nginx", "-g", "daemon off;"]