using Fcg.Contracts;
using MassTransit;
using PaymentsWorker.Services;

namespace PaymentsWorker.Consumers
{
    public class OrderPlacedConsumer : IConsumer<OrderPlacedEvent>
    {
        private readonly ILogger<OrderPlacedEvent> _logger;

        public OrderPlacedConsumer(ILogger<OrderPlacedEvent> logger) => _logger = logger;

        public async Task Consume(ConsumeContext<OrderPlacedEvent> context)
        {
            var message = context.Message;

            _logger.LogInformation($"User ID: {message.UserId}.\nGame ID:{message.GameId}.\nProcessando o pagamento...");

            var paymentStatus = PaymentService.SimulatePayment();

            _logger.LogInformation($"Pagamento processado com sucesso! Resultado: {paymentStatus}");

            var paymentResult = new PaymentProcessedEvent
            {
                TransactionId = Guid.NewGuid(),
                UserId = message.UserId,
                UserEmail = message.UserEmail,
                GameId = message.GameId,
                Status = paymentStatus
            };

            await context.Publish(paymentResult);
        }
    }
}
