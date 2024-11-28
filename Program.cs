using Microsoft.EntityFrameworkCore;
using MySQLService.Repositories;
using CQRS_Rabbit_MySQL.Services;
using CQRS_Un_Proyecto.Infrastructure.Model;
using MySQLService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<GestionSaludContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
    new MySqlServerVersion(new Version(8, 0, 32))));

builder.Services.AddScoped<ConsultaRepository>();
builder.Services.AddScoped<EstadoConsultaRepository>();
builder.Services.AddScoped<PacienteRepository>();
builder.Services.AddScoped<MedicoRepository>();
builder.Services.AddScoped<RecetaRepository>();
builder.Services.AddScoped<HistorialEstadoConsultaRepository>();
builder.Services.AddSingleton<RabbitMqService>();
builder.Services.AddSingleton<EstadoConsultaRabbitMqService>();
builder.Services.AddSingleton<PacienteRabbitMqService>();
builder.Services.AddSingleton<MedicoRabbitMqService>();
builder.Services.AddSingleton<RecetaRabbitMqService>();
builder.Services.AddSingleton<HistorialEstadoConsultaRabbitMqService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<RabbitMqService>();

var app = builder.Build();

app.MapGet("/", () => "RabbitMQ Service Running!");
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
