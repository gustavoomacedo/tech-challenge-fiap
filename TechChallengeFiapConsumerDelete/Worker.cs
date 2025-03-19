using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using TechChallengeFiapConsumerDelete.Infrastructure.Repository;
using TechChallengeFiapConsumerDelete.Interfaces;

public class Worker : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<Worker> _logger;
    private IConnection _connection;
    private IModel _channel;

    public Worker(ILogger<Worker> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    public override Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Worker started at: {time}", DateTimeOffset.Now);

        var factory = new ConnectionFactory()
        {
            HostName = "rabbitmq", // Nome do serviço no docker-compose.yml
            UserName = "guest",
            Password = "guest"
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        _channel.QueueDeclare(
            queue: "contactDeleteQueue",
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null);

        return base.StartAsync(cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Worker executed at: {time}", DateTimeOffset.Now);

        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += async (model, ea) =>
        {
            using var scope = _serviceProvider.CreateScope();

            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var contactService = scope.ServiceProvider.GetRequiredService<IContactService>();

            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            try
            {
                // Deserialize message
                var contact = JsonSerializer.Deserialize<int>(message);

                // Process & Save to DB
                await contactService.deleteContactAsync(contact);

                // ACK message
                _channel.BasicAck(ea.DeliveryTag, false);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao processar mensagem: {ex.Message}");
                _channel.BasicNack(ea.DeliveryTag, false, requeue: true);
                // Não dar ACK para que a mensagem permaneça na fila para nova tentativa
            }
        };

        _channel.BasicConsume(queue: "contactDeleteQueue", autoAck: false, consumer: consumer);

        await Task.Delay(Timeout.Infinite, stoppingToken); // Mantém o Worker rodando
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Worker stopping...");
        _channel?.Close();
        _connection?.Close();
        return base.StopAsync(cancellationToken);
    }
}
