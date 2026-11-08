FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY Directory.Build.props ./
COPY StockMicroservices.API/StockMicroservices.API.csproj StockMicroservices.API/
COPY StockMicroservices.EventBus.Common/StockMicroservices.EventBus.Common.csproj StockMicroservices.EventBus.Common/
RUN dotnet restore StockMicroservices.API/StockMicroservices.API.csproj

COPY . .
RUN dotnet publish StockMicroservices.API/StockMicroservices.API.csproj \
    --configuration Release \
    --output /app/publish \
    /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "StockMicroservices.API.dll"]
