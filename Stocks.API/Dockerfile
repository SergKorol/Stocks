# Base image for running the application (runtime only)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Build image (includes SDK for building the app)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
USER root
# Copy project files
COPY ["Stocks.API/Stocks.API.csproj", "Stocks.API/"]
COPY ["Stocks.Database/Stocks.Database.csproj", "Stocks.Database/"]

# Restore dependencies
RUN dotnet restore "Stocks.API/Stocks.API.csproj"
RUN dotnet restore "Stocks.Database/Stocks.Database.csproj"
COPY . .

# Ensure the global tools path is in the PATH environment variable
ENV PATH="$PATH:/root/.dotnet/tools"

# Copy the rest of the files
COPY . .

# Build the projects
WORKDIR "/src/Stocks.API"
RUN dotnet build "Stocks.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

WORKDIR "/src/Stocks.Database"
RUN dotnet build "Stocks.Database.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Run EF Core migrations in the build stage (SDK is available)
WORKDIR "/src/Stocks.Database"
RUN dotnet ef database update

# Publish the API
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Stocks.API/Stocks.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final stage (runtime only, no SDK)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY . .
ENTRYPOINT ["dotnet", "Stocks.API.dll"]
