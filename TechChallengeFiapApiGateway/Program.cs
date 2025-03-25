using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);

// Adiciona Ocelot e configura��o JSON
builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
builder.Services.AddOcelot();

var app = builder.Build();

// Adiciona Middleware de M�tricas Prometheus
app.UseRouting();
app.UseHttpMetrics();  // Captura m�tricas HTTP automaticamente

app.UseEndpoints(endpoints =>
{
    endpoints.MapMetrics();  // Expondo m�tricas na rota "/metrics"
});

app.UseOcelot().Wait();
app.Run();