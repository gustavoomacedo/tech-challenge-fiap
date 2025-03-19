FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
ENV ASPNETCORE_ENVIRONMENT=Production
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["TechChallengeFiap.csproj", "."]
RUN dotnet restore "./TechChallengeFiap.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "./TechChallengeFiap.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./TechChallengeFiap.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Configurar a aplicação para rodar apenas no HTTP
ENV ASPNETCORE_URLS=http://+:80

RUN sed -i 's/\[openssl_init\]/# [openssl_init]/' /etc/ssl/openssl.cnf

RUN printf "\n\n[openssl_init]\nssl_conf = ssl_sect" >> /etc/ssl/openssl.cnf
RUN printf "\n\n[ssl_sect]\nsystem_default = ssl_default_sect" >> /etc/ssl/openssl.cnf
RUN printf "\n\n[ssl_default_sect]\nMinProtocol = TLSv1\nCipherString = DEFAULT@SECLEVEL=0\n" >> /etc/ssl/openssl.cnf

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TechChallengeFiap.dll"]
