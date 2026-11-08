FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY Directory.Build.props ./
COPY StockMicroservices.StockMarketUpdater/StockMicroservices.StockMarketUpdater.csproj StockMicroservices.StockMarketUpdater/
COPY StockMicroservices.EventBus.Common/StockMicroservices.EventBus.Common.csproj StockMicroservices.EventBus.Common/
RUN dotnet restore StockMicroservices.StockMarketUpdater/StockMicroservices.StockMarketUpdater.csproj

COPY . .
RUN dotnet publish StockMicroservices.StockMarketUpdater/StockMicroservices.StockMarketUpdater.csproj \
    --configuration Release \
    --output /app/publish \
    /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/runtime:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "StockMicroservices.StockMarketUpdater.dll"]
