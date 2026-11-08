FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY Directory.Build.props ./
COPY StockMicroservices.IdentityServer/StockMicroservices.IdentityServer.csproj StockMicroservices.IdentityServer/
RUN dotnet restore StockMicroservices.IdentityServer/StockMicroservices.IdentityServer.csproj

COPY . .
RUN dotnet publish StockMicroservices.IdentityServer/StockMicroservices.IdentityServer.csproj \
    --configuration Release \
    --output /app/publish \
    /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80
RUN mkdir -p /app/keys
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "StockMicroservices.IdentityServer.dll"]
