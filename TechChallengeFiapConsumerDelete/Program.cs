using Microsoft.Extensions.Configuration;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System;
using System.Text;
using TechChallengeFiapConsumerDelete.Interfaces;
using TechChallengeFiapConsumerDelete.Infrastructure.DTOs;
using System.Text.Json;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using TechChallengeFiapConsumerDelete.Infrastructure.Services;
using TechChallengeFiapConsumerDelete.Infrastructure.Repository;

namespace TechChallengeFiapConsumerDelete
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Configuração do Host para o consumidor
            var host = Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                {


                    var connection = context.Configuration.GetConnectionString("DefaultConnection");
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

                    services.AddHostedService<Worker>();
                })
                .Build();
            await host.RunAsync();
        }
    }
   
}