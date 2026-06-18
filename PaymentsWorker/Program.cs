using MassTransit;
using PaymentsWorker.Consumers;

var builder = Host.CreateApplicationBuilder(args);

#region MassTransit (RabbitMQ)

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<OrderPlacedConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        var host = Environment.GetEnvironmentVariable("RABBITMQ_HOST") ?? "localhost";
        var user = Environment.GetEnvironmentVariable("RABBITMQ_DEFAULT_USER") ?? "guest";
        var password = Environment.GetEnvironmentVariable("RABBITMQ_DEFAULT_PASS") ?? "guest";

        cfg.Host(host, "/", h =>
        {
            h.Username(user);
            h.Password(password);
        });

        var ordersPlacedQueue = Environment.GetEnvironmentVariable("ORDER_PLACED_QUEUE_NAME") ?? "orders-placed-queue";

        cfg.ReceiveEndpoint(ordersPlacedQueue, e => e.ConfigureConsumer<OrderPlacedConsumer>(context));
    });
});

#endregion

var host = builder.Build();
host.Run();
