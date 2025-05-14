# Use the official RabbitMQ image with management plugin
FROM rabbitmq:management

# Set environment variables (alterar se necessário)
ENV RABBITMQ_DEFAULT_USER=guest
ENV RABBITMQ_DEFAULT_PASS=guest

# Expondo as portas necessárias para comunicação e acesso à interface
EXPOSE 5672 15672

# Comando de inicialização do RabbitMQ
CMD ["rabbitmq-server"]