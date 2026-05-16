# Etapa 1: Runtime base (ligero, solo para ejecutar la app compilada)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

# Etapa 2: SDK (pesado, incluye herramientas de compilación)
FROM mcr.microsoft.com/dotnet/sdk:10.0.300 AS build
WORKDIR /src

# Copiar los archivos de proyecto (.csproj) conservando la estructura de carpetas
# ¿Por qué? Para restaurar dependencias aprovechando la caché de capas de Docker antes de copiar todo el código.
COPY ["src/FixFlow.API/FixFlow.API.csproj", "src/FixFlow.API/"]
COPY ["src/FixFlow.Shared/FixFlow.Shared.csproj", "src/FixFlow.Shared/"]
RUN dotnet restore "src/FixFlow.API/FixFlow.API.csproj"

# Copiar el resto del código fuente
COPY . .

# Cambiar el directorio de trabajo a la API y compilar
WORKDIR "/src/src/FixFlow.API"
RUN dotnet build "FixFlow.API.csproj" -c Release -o /app/build

# Etapa 3: Publicación (optimiza los binarios)
FROM build AS publish
RUN dotnet publish "FixFlow.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Etapa 4: Imagen Final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FixFlow.API.dll"]