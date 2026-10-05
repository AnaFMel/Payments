using Azure.Identity;
using Fcg.Contracts;
using MassTransit;
using PaymentsWorker.Consumers;

var builder = Host.CreateApplicationBuilder(args);

var vaultUriStr = Environment.GetEnvironmentVariable("KeyVaultUri") ?? string.Empty;
var vaultUri = new Uri(vaultUriStr);

builder.Configuration.AddAzureKeyVault(vaultUri, new DefaultAzureCredential());

#region MassTransit (Azure Service Bus)

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<OrderPlacedConsumer>();

    var paymentsTopicName = Environment.GetEnvironmentVariable("PAYMENTS_TOPIC") ?? string.Empty;
    var orderTopicName = Environment.GetEnvironmentVariable("ORDERS_TOPIC") ?? string.Empty;
    var paymentSubName = Environment.GetEnvironmentVariable("PAYMENT_SUBSCRIPTION") ?? string.Empty;

    x.UsingAzureServiceBus((context, cfg) =>
    {
        var connectionString = builder.Configuration["ServiceBusConnectionString"] ?? string.Empty;

        cfg.Host(connectionString);

        cfg.Message<OrderPlacedEvent>(e => e.SetEntityName(orderTopicName));
        cfg.Message<PaymentProcessedEvent>(e => e.SetEntityName(paymentsTopicName));

        cfg.SubscriptionEndpoint(paymentSubName, orderTopicName, e =>
        {
            e.ConfigureConsumer<OrderPlacedConsumer>(context);
        });

        cfg.UseMessageRetry(r => r.Interval(3, TimeSpan.FromSeconds(5)));
    });
});

#endregion

var host = builder.Build();
host.Run();