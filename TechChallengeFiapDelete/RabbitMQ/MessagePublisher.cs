using Microsoft.AspNetCore.Connections;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;
using TechChallengeFiapDelete.Infrastructure.DTOs;
using TechChallengeFiapDelete.Models;


namespace TechChallengeFiapDelete.RabbitMQ
{

    public interface IMessagePublisher
    {
        Task PublishMessageAsync(int id);
    }
    public class MessagePublisher : IMessagePublisher
    {
        public async Task PublishMessageAsync(int id)
        {
            var factory = new ConnectionFactory() { 
                HostName = "localhost",
                UserName = "guest",
                Password    = "guest",
            };
            using var connection = factory.CreateConnection();
        
            // Criação do canal (ainda é síncrono)
             using var channel = connection.CreateModel();

            channel.QueueDeclare(queue: "contactDeleteQueue", durable: false, exclusive: false, autoDelete: false, arguments: null);

            var message = JsonConvert.SerializeObject(id);
            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(exchange: "", routingKey: "contactDeleteQueue", basicProperties: null, body: body);
        }
    }
}
