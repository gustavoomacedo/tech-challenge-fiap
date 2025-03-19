using Microsoft.AspNetCore.Connections;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;
using TechChallengeFiapUpdate.Infrastructure.DTOs;
using TechChallengeFiapUpdate.Models;


namespace TechChallengeFiapUpdate.RabbitMQ
{

    public interface IMessagePublisher
    {
        Task PublishMessageAsync(ContactUpdateRequestDTO contact);
    }
    public class MessagePublisher : IMessagePublisher
    {
        public async Task PublishMessageAsync(ContactUpdateRequestDTO contact)
        {
            var factory = new ConnectionFactory() { 
                HostName = "rabbitmq",
                UserName = "guest",
                Password    = "guest",
            };
            using var connection = factory.CreateConnection();
        
            // Criação do canal (ainda é síncrono)
             using var channel = connection.CreateModel();

            channel.QueueDeclare(queue: "contactUpdateQueue", durable: false, exclusive: false, autoDelete: false, arguments: null);

            var message = JsonConvert.SerializeObject(contact);
            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(exchange: "", routingKey: "contactUpdateQueue", basicProperties: null, body: body);
        }
    }
}
