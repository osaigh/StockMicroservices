FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY Directory.Build.props ./
COPY StockMicroservices.APIGateway/StockMicroservices.APIGateway.csproj StockMicroservices.APIGateway/
RUN dotnet restore StockMicroservices.APIGateway/StockMicroservices.APIGateway.csproj

COPY . .
RUN dotnet publish StockMicroservices.APIGateway/StockMicroservices.APIGateway.csproj \
    --configuration Release \
    --output /app/publish \
    /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "StockMicroservices.APIGateway.dll"]
