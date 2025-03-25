using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);

// Adiciona Ocelot e configuração JSON
builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
builder.Services.AddOcelot();

var app = builder.Build();

// Adiciona Middleware de Métricas Prometheus
app.UseRouting();
app.UseHttpMetrics();  // Captura métricas HTTP automaticamente

app.UseEndpoints(endpoints =>
{
    endpoints.MapMetrics();  // Expondo métricas na rota "/metrics"
});

app.UseOcelot().Wait();
app.Run();