using Microsoft.AspNetCore.Connections;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;
using TechChallengeFiapAdd.Infrastructure.DTOs;
using TechChallengeFiapAdd.Models;


namespace TechChallengeFiapAdd.RabbitMQ
{

    public interface IMessagePublisher
    {
        Task PublishMessageAsync(ContactRequestDTO contact);
    }
    public class MessagePublisher : IMessagePublisher
    {
        public async Task PublishMessageAsync(ContactRequestDTO contact)
        {
            var factory = new ConnectionFactory() { 
                HostName = "localhost",
                UserName = "guest",
                Password    = "guest",
            };
            using var connection = factory.CreateConnection();
        
            // Criação do canal (ainda é síncrono)
             using var channel = connection.CreateModel();

            channel.QueueDeclare(queue: "contactQueue", durable: false, exclusive: false, autoDelete: false, arguments: null);

            var message = JsonConvert.SerializeObject(contact);
            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(exchange: "", routingKey: "contactQueue", basicProperties: null, body: body);
        }
    }
}
