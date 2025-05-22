# 1) Base runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

# 2) Build & publish
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["MotorcycleForum/MotorcycleForum.Web/MotorcycleForum.Web.csproj", "MotorcycleForum/MotorcycleForum.Web/"]
COPY ["MotorcycleForum/MotorcycleForum.Data/MotorcycleForum.Data.csproj", "MotorcycleForum/MotorcycleForum.Data/"]
COPY ["MotorcycleForum/MotorcycleForum.Services/MotorcycleForum.Services.csproj", "MotorcycleForum/MotorcycleForum.Services/"]

RUN dotnet restore "MotorcycleForum/MotorcycleForum.Web/MotorcycleForum.Web.csproj"

COPY . .
WORKDIR "/src/MotorcycleForum/MotorcycleForum.Web"
RUN dotnet publish "./MotorcycleForum.Web.csproj" \
    -c $BUILD_CONFIGURATION \
    -o /app/publish \
    /p:UseAppHost=false

# 3) Final image
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "MotorcycleForum.Web.dll"]
