using Amazon.SimpleNotificationService;
using Amazon.SQS;
using MassTransit;
using PaymentsWorker.Consumers;

var builder = Host.CreateApplicationBuilder(args);

#region MassTransit (AWS SQS / LocalStack)
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<OrderPlacedConsumer>();

    x.UsingAmazonSqs((context, cfg) =>
    {
        cfg.Host("us-east-1", h =>
        {
            h.AccessKey("test");
            h.SecretKey("test");

            var awsEndpoint = Environment.GetEnvironmentVariable("AWS_ENDPOINT") ?? "http://localhost:4566";

            h.Config(new AmazonSQSConfig { ServiceURL = awsEndpoint });
            h.Config(new AmazonSimpleNotificationServiceConfig { ServiceURL = awsEndpoint });
        });

        var ordersPlacedQueue = Environment.GetEnvironmentVariable("ORDER_PLACED_QUEUE_NAME") ?? "orders-placed-queue";

        cfg.ReceiveEndpoint(ordersPlacedQueue, e =>
        {
            e.ConfigureConsumer<OrderPlacedConsumer>(context);
        });
    });
});
#endregion

var host = builder.Build();
host.Run();