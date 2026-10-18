using Fcg.Contracts;
using MassTransit;
using MassTransit.Testing;
using Microsoft.Extensions.DependencyInjection;
using PaymentsWorker.Consumers;

namespace Payments.Tests.Consumers
{
    public class OrderPlacedConsumerTests
    {
        [Fact]
        public async Task Consumer_Should_Process_Order_And_Publish_Payment_Event()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddMassTransitTestHarness(cfg =>
            {
                cfg.AddConsumer<OrderPlacedConsumer>();
            });

            var provider = services.BuildServiceProvider();
            var harness = provider.GetRequiredService<ITestHarness>();

            await harness.Start();

            try
            {
                var publisher = harness.Bus;

                var orderPlacedEvent = new OrderPlacedEvent(
                    Guid.NewGuid().ToString(),
                    123,
                    "user@test.com",
                    Guid.NewGuid().ToString(),
                    100.0m
                );

                // Act
                await publisher.Publish(orderPlacedEvent);

                // Assert
                Assert.True(await harness.Consumed.Any<OrderPlacedEvent>());

                Assert.True(await harness.Published.Any<PaymentProcessedEvent>());
            }
            finally
            {
                await harness.Stop();
            }
        }
    }
}