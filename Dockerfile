# build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# copy csproj files
COPY ["MotorcycleForum/MotorcycleForum.csproj", "MotorcycleForum/"]
COPY ["MotorcycleForum.Data/MotorcycleForum.Data.csproj", "MotorcycleForum.Data/"]
COPY ["MotorcycleForum.Services/MotorcycleForum.Services.csproj", "MotorcycleForum.Services/"]

# restore & publish
RUN dotnet restore "MotorcycleForum/MotorcycleForum.csproj"
COPY . .
WORKDIR /src/MotorcycleForum
RUN dotnet publish -c Release -o /app/publish

# runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "MotorcycleForum.dll"]
