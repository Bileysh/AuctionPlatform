FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY AuctionPlatform.sln .
COPY AuctionPlatform.Domain/AuctionPlatform.Domain.csproj             AuctionPlatform.Domain/
COPY AuctionPlatform.Application/AuctionPlatform.Application.csproj   AuctionPlatform.Application/
COPY AuctionPlatform.Infrastructure/AuctionPlatform.Infrastructure.csproj AuctionPlatform.Infrastructure/
COPY AuctionPlatform.WebApi/AuctionPlatform.WebApi.csproj             AuctionPlatform.WebApi/

RUN dotnet restore AuctionPlatform.sln

COPY . .
RUN dotnet publish AuctionPlatform.WebApi/AuctionPlatform.WebApi.csproj \
    -c Release \
    -o /app/publish \
    --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "AuctionPlatform.WebApi.dll"]
