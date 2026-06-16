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
                TransactionId = new Guid(), //aqui preciso ver como vai ficar
                UserId = message.UserId,
                GameId = message.GameId,
                Status = paymentStatus
            };

            await context.Publish(paymentResult);
        }
    }
}
