using MassTransit;
using PaymentsWorker.Consumers;
using PaymentsWorker.Contracts;

var builder = Host.CreateApplicationBuilder(args);

#region MassTransit (RabbitMQ)

builder.Services.AddMassTransit(x =>
{
    var orderPlaceExchangeName = Environment.GetEnvironmentVariable("ORDER_PLACED_EXCHANGE_NAME") ?? "order-placed-event";
    var paymentProcessedExchangeName = Environment.GetEnvironmentVariable("PAYMENT_PROCESSED_EXCHANGE_NAME") ?? "payment-processed-event";

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

        cfg.Message<OrderPlacedEvent>(m => m.SetEntityName(orderPlaceExchangeName));
        cfg.Message<PaymentProcessedEvent>(m => m.SetEntityName(paymentProcessedExchangeName));

        cfg.ReceiveEndpoint(ordersPlacedQueue, e => e.ConfigureConsumer<OrderPlacedConsumer>(context));
    });
});

#endregion

var host = builder.Build();
host.Run();
