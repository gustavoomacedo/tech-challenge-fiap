using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Prometheus;
using System;
using System.Reflection;
using TechChallengeFiap.Infrastructure.Repository;
using TechChallengeFiap.Infrastructure.Services;
using TechChallengeFiap.Interfaces;
using TechChallengeFiap.Models;
using TechChallengeFiap.RabbitMQ;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        //Dependence injection to repository
        builder.Services.AddScoped<IContactRepository, ContactRepository>();

        //Dependence injection to Service
        builder.Services.AddScoped<IContactService, ContactService>();

        builder.Services.AddScoped<IMessagePublisher, MessagePublisher>();

        // Add services to the container.
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Cadastro de contatos por DDD", Version = "v1" });

            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile) ;
            c.IncludeXmlComments(xmlPath);
        });

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAllOrigins", builder =>
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader());
        });


        builder.Services.AddDbContext<ApplicationDbContext>(options =>
        {
            // Aqui você usa a string de conexão do seu banco de dados
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
        });

        var app = builder.Build();

        // Aplica as Migrations automaticamente ao iniciar a aplicação
        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            dbContext.Database.Migrate();  // Aplica as migrations pendentes
        }

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        /*INICIO DA CONFIGURAÇÃO - PROMETHEUS*/
        // Custom Metrics to count requests for each endpoint and the method
        var counter = Metrics.CreateCounter("webapimetric", "Counts requests to the WebApiMetrics API endpoints",
            new CounterConfiguration
            {
                LabelNames = new[] { "method", "endpoint" }
            });

        app.Use((context, next) =>
        {
            counter.WithLabels(context.Request.Method, context.Request.Path).Inc();
            return next();
        });

        // Use the prometheus middleware
        app.UseMetricServer();
        app.UseHttpMetrics();

        /*FIM DA CONFIGURAÇÃO - PROMETHEUS*/

        //app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
