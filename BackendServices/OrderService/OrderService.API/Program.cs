using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using OrderService.API.Consumer;
using OrderService.Infrastructure;
using OrderStateMachine;
using OrderStateMachine.Database.Entities;
using OrderStateMachine.StateMachine;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
ServicesRegistration.RegisterServices(builder.Services, builder.Configuration);
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.TryAddSingleton(KebabCaseEndpointNameFormatter.Instance);
builder.Services
    .AddMassTransit(_Config =>
    {
        //State machine consumer
        _Config.AddConsumer<OrderStartConsumer>();
        _Config.AddConsumer<OrderAcceptedConsumer>();
        //_Config.AddConsumer<OrderRejectedConsumer>();
        //_Config.AddConsumer<OrderCompletedConsumer>();
        _Config.AddConsumer<OrderCancelledConsumer>();

        //State machine
        _Config.AddSagaStateMachine<OrderMachine, OrderState>()
            .EntityFrameworkRepository(r =>
            {
                r.ConcurrencyMode = ConcurrencyMode.Pessimistic; // Use Pessimistic concurrency
                r.AddDbContext<DbContext, OrderStateDBContext>((provider, options) =>
                {
                    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
                });
            });
        //Config azure service bus
        _Config.UsingAzureServiceBus((context, cfg) =>
        {
            var connectionString = builder.Configuration["ServiceBus:ConnectionString"];
            cfg.Host(connectionString);
            cfg.ConfigureEndpoints(context);
        });
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
