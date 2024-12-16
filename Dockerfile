FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Önce proje dosyalarını kopyala
COPY ["Cryptotracker.Api/Cryptotracker.Api.csproj", "Cryptotracker.Api/"]
COPY ["Cryptotracker.Shared/Cryptotracker.Shared.csproj", "Cryptotracker.Shared/"]


# Restore işlemi
RUN dotnet restore "Cryptotracker.Api/Cryptotracker.Api.csproj"

# Tüm solution'ı kopyala
COPY . .

# Build
RUN dotnet build "Cryptotracker.Api/Cryptotracker.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Cryptotracker.Api/Cryptotracker.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Cryptotracker.Api.dll"]
