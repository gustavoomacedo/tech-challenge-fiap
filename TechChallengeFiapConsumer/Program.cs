using Microsoft.Extensions.Configuration;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System;
using System.Text;
using TechChallengeFiapConsumer.Interfaces;
using TechChallengeFiapConsumer.Infrastructure.DTOs;
using System.Text.Json;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using TechChallengeFiapConsumer.Infrastructure.Services;
using TechChallengeFiapConsumer.Infrastructure.Repository;

namespace TechChallengeFiapConsumer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Configuração do Host para o consumidor
            var host = Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                {

                    var connection = context.Configuration.GetConnectionString("ConnectionString");
                    services.AddDbContext<ApplicationDbContext>(options =>
                    {
                        options.UseSqlServer(connection);
                    });

                    //Dependence injection to repository
                    services.AddScoped<IContactRepository, ContactRepository>();

                    //Dependence injection to Service
                    services.AddScoped<IContactService, ContactService>();


                    // Registro do IContactService
                    services.AddScoped<IContactService, ContactService>();

                    services.AddHostedService<RabbitMQConsumerService>();
                })
                .Build();
            await host.RunAsync();
        }
    }

    public class RabbitMQConsumerService : IHostedService
    {
        private readonly IContactService _contactService;
        private readonly IConfiguration _configuration;

        public RabbitMQConsumerService(IContactService contactService, IConfiguration configuration)
        {
            _contactService = contactService;
            _configuration = configuration;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest",
            };
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            channel.QueueDeclare(queue: "contactQueue", durable: false, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                // Parse da mensagem
                var contact = ParseMessage(message);

                // Salva no banco de dados
                await SaveContactToDatabase(contact);
            };

            channel.BasicConsume(queue: "contactQueue", autoAck: true, consumer: consumer);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            // Lógica de parada do consumidor
            return Task.CompletedTask;
        }

        private ContactRequestDTO ParseMessage(string message)
        {
            // Simulação de parse, pode ser ajustado conforme sua estrutura de mensagem
            return JsonSerializer.Deserialize<ContactRequestDTO>(message);
        }

        private async Task SaveContactToDatabase(ContactRequestDTO contact)
        {
            var returnContact = await _contactService.AddContactAsync(contact);
            Console.WriteLine($"Contact {contact.Name} saved to database.");
        }
    }
}