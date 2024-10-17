using Microsoft.EntityFrameworkCore;
using System.Reflection;
using TechChallengeFiap.Infrastructure.Repository;
using TechChallengeFiap.Infrastructure.Services;
using TechChallengeFiap.Interfaces;

var builder = WebApplication.CreateBuilder(args);

//Dependence injection to repository
builder.Services.AddScoped<IContactRepository, ContactRepository>();

//Dependence injection to Service
builder.Services.AddScoped<IContactService, ContactService>();

// Add services to the container.
var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile) ;
    c.IncludeXmlComments(xmlPath);
});

var connection = configuration.GetConnectionString("ConnectionString");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(connection);
    options.UseLazyLoadingProxies();
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
