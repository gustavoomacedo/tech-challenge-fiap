# Use a imagem base do SQL Server
FROM mcr.microsoft.com/mssql/server:2022-latest

# Defina variáveis de ambiente
ENV ACCEPT_EULA=Y
ENV SA_PASSWORD=YourStrong!Passw0rd

# Crie o diretório de dados do SQL Server (opcional)
RUN mkdir -p /var/opt/mssql

# Exponha a porta do SQL Server
EXPOSE 1433

# Comando para rodar o SQL Server
CMD /opt/mssql/bin/sqlservr
