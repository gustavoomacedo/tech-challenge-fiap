# Use the official RabbitMQ image with management plugin
FROM rabbitmq:management

# Set environment variables (alterar se necess�rio)
ENV RABBITMQ_DEFAULT_USER=guest
ENV RABBITMQ_DEFAULT_PASS=guest

# Expondo as portas necess�rias para comunica��o e acesso � interface
EXPOSE 5672 15672

# Comando de inicializa��o do RabbitMQ
CMD ["rabbitmq-server"]