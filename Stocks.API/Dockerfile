# Base image for running the application
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["Stocks.API/Stocks.API.csproj", "Stocks.API/"]
COPY ["Stocks.Database/Stocks.Database.csproj", "Stocks.Database/"]

RUN dotnet restore "Stocks.API/Stocks.API.csproj"
RUN dotnet restore "Stocks.Database/Stocks.Database.csproj"

COPY . .

# Build the projects
WORKDIR "/src/Stocks.API"
RUN dotnet build "Stocks.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

WORKDIR "/src/Stocks.Database"
RUN dotnet build "Stocks.Database.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
WORKDIR "/src/Stocks.API"
RUN dotnet publish -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Stocks.API.dll"]