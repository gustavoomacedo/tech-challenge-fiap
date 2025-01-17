# Usar a imagem base oficial do Prometheus
FROM prom/prometheus:latest

# Copiar o arquivo de configuração para dentro do contêiner
COPY prometheus.yml /etc/prometheus/prometheus.yml

VOLUME ./data
EXPOSE 9090
