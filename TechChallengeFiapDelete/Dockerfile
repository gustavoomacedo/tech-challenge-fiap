FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
ENV ASPNETCORE_ENVIRONMENT=Production
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["TechChallengeFiapDelete.csproj", "."]
RUN dotnet restore "./TechChallengeFiapDelete.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "./TechChallengeFiapDelete.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./TechChallengeFiapDelete.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Configurar a aplica��o para rodar apenas no HTTP
ENV ASPNETCORE_URLS=http://+:80


# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TechChallengeFiapDelete.dll"]
