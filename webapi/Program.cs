using MassTransit;
using Microsoft.EntityFrameworkCore;
using webapi.Data;
using webapi.Sagas;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("Database"));

builder.Services.AddMassTransit(options =>
{
    options.SetKebabCaseEndpointNameFormatter();
    
    options.AddConsumers(typeof(Program).Assembly);
    
    options.UsingRabbitMq((context, config) =>
    {
        config.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });
        config.ConfigureEndpoints(context);
    });
    
    options.AddSagaStateMachine<OrderProcessingSaga, OrderProcessingSagaData>()
        .InMemoryRepository();
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();