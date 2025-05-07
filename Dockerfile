# Stage 1: build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy your three csproj files into subfolders that match your repo
COPY ["MotorcycleForum.Web/MotorcycleForum.Web.csproj", "MotorcycleForum.Web/"]
COPY ["MotorcycleForum.Data/MotorcycleForum.Data.csproj", "MotorcycleForum.Data/"]
COPY ["MotorcycleForum.Services/MotorcycleForum.Services.csproj", "MotorcycleForum.Services/"]

# Restore, then copy the rest of the code
RUN dotnet restore "MotorcycleForum.Web/MotorcycleForum.Web.csproj"
COPY . .

# Publish your web app
WORKDIR /src/MotorcycleForum.Web
RUN dotnet publish -c Release -o /app/publish

# Stage 2: runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "MotorcycleForum.Web.dll"]
