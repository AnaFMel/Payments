using MassTransit;
using PaymentsWorker.Contracts;
using PaymentsWorker.Services;

namespace PaymentsWorker.Consumers
{
    public class OrderPlacedConsumer : IConsumer<OrderPlacedEvent>
    {
        public async Task Consume(ConsumeContext<OrderPlacedEvent> context)
        {
            var message = context.Message;

            var paymentStatus = PaymentService.SimulatePayment();

            var paymentResult = new PaymentProcessedEvent
            {
                TransactionId = new Guid(),
                UserId = message.UserId,
                UserEmail = message.UserEmail,
                GameId = message.GameId,
                Status = paymentStatus
            };

            await context.Publish(paymentResult);
        }
    }
}
