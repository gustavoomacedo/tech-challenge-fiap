using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using TechChallengeFiapConsumerUpdate.Infrastructure.DTOs;
using TechChallengeFiapConsumerUpdate.Infrastructure.Repository;
using TechChallengeFiapConsumerUpdate.Interfaces;

public class Worker : BackgroundService
{
    private readonly IConfiguration _configuration;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<Worker> _logger;

    public Worker(ILogger<Worker> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Worker started at: {time}", DateTimeOffset.Now);

        while (!stoppingToken.IsCancellationRequested)
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest",
            };
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            channel.QueueDeclare(queue: "contactUpdateQueue", durable: false, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(channel);
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
                    var contact = JsonSerializer.Deserialize<ContactUpdateRequestDTO>(message);

                    // Process & Save to DB
                    await contactService.updateContactAsync(contact);

                    // ACK message
                    channel.BasicAck(ea.DeliveryTag, false);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro: {ex.Message}");
                    // Não dar ACK, a mensagem ficará na fila ou DLQ
                }

            };

            channel.BasicConsume(queue: "contactUpdateQueue", autoAck: true, consumer: consumer);

            await Task.Delay(2000,stoppingToken);
        }

        _logger.LogInformation("Worker stopping at: {time}", DateTimeOffset.Now);
    }
}
