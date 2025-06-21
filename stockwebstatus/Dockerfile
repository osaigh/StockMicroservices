# Stage 1: Build Angular app
FROM node:22.16.0-alpine AS build

# Set working directory
WORKDIR /app

ENV PATH /app/node_modules/.bin:$PATH

# Copy package files and install dependencies
COPY ["stockwebstatus/package.json", "./"]
COPY ["stockwebstatus/package-lock.json", "./"]
RUN npm install --silent

# Copy the rest of the app and build it
COPY ["stockwebstatus", "./"]
RUN npm run build

# Stage 2: Serve with Nginx
FROM nginx:1.23-alpine
COPY "stockwebstatus/nginx.config" /etc/nginx/conf.d/default.conf
EXPOSE 80
COPY --from=build /app/dist/stockwebstatus/browser /usr/share/nginx/html
COPY "stockwebstatus/docker-entrypoint.sh" /docker-entrypoint.sh
RUN chmod +x /docker-entrypoint.sh
ENTRYPOINT ["/docker-entrypoint.sh"]
CMD ["nginx", "-g", "daemon off;"]

# Copy built app from Stage 1
#COPY --from=build /app/dist/your-angular-app-name /usr/share/nginx/html

# Optional: Replace default nginx config with a custom one
#COPY nginx.conf /etc/nginx/conf.d/default.conf

# Expose default port
#EXPOSE 80

# Start nginx
#CMD ["nginx", "-g", "daemon off;"]
