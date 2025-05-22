using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Prometheus;
using System.Reflection;
using TechChallengeFiapDelete.RabbitMQ;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);


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
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Exclusão de contatos por DDD", Version = "v1" });

            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            c.IncludeXmlComments(xmlPath);
        });

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAllOrigins", builder =>
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader());
        });

        var app = builder.Build();

        app.UseSwagger();
        app.UseSwaggerUI();

        /*INICIO DA CONFIGURAÇÃO - PROMETHEUS*/
        // Custom Metrics to count requests for each endpoint and the method
        var counter = Metrics.CreateCounter("deletewebapimetric", "Counts DELETE requests to the WebApiMetrics API endpoints",
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

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
