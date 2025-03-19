FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
ENV ASPNETCORE_ENVIRONMENT=Production
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["TechChallengeFiapUpdate.csproj", "."]
RUN dotnet restore "./TechChallengeFiapUpdate.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "./TechChallengeFiapUpdate.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./TechChallengeFiapUpdate.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Configurar a aplicação para rodar apenas no HTTP
ENV ASPNETCORE_URLS=http://+:80


# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TechChallengeFiapUpdate.dll"]
