using Amazon;
using Amazon.Runtime;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using Fcg.Contracts;
using MassTransit;
using PaymentsWorker.Consumers;
using System.Text.Json;

var builder = Host.CreateApplicationBuilder(args);

var awsAccessKeyId = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID") ?? string.Empty;
var awsSecretAccessKey = Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY") ?? string.Empty;

var credentials = new BasicAWSCredentials(awsAccessKeyId, awsSecretAccessKey);
using var client = new AmazonSecretsManagerClient(RegionEndpoint.USEast1);

var request = new GetSecretValueRequest
{
    SecretId = "fcg-secrets"
};

var response = await client.GetSecretValueAsync(request);

if (!string.IsNullOrEmpty(response.SecretString))
{
    var secretData = JsonSerializer.Deserialize<Dictionary<string, string>>(response.SecretString);
    if (secretData != null)
    {
        builder.Configuration.AddInMemoryCollection(secretData!);
    }
}

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