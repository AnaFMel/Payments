using Fcg.Contracts;
using MassTransit;
using PaymentsWorker.Services;

namespace PaymentsWorker.Consumers
{
    public class OrderPlacedConsumer : IConsumer<OrderPlacedEvent>
    {
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ILogger<OrderPlacedEvent> _logger;

        public OrderPlacedConsumer(IPublishEndpoint publishEndpoint, ILogger<OrderPlacedEvent> logger)
        {
            _publishEndpoint = publishEndpoint;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<OrderPlacedEvent> context)
        {
            var message = context.Message;

            _logger.LogInformation($"User ID: {message.UserId}.\nGame ID:{message.GameId}.\nProcessando o pagamento...");

            var paymentStatus = PaymentService.SimulatePayment();

            _logger.LogInformation($"Pagamento processado com sucesso! Resultado: {paymentStatus}");

            var paymentResult = new PaymentProcessedEvent(Guid.NewGuid(), message.Id, message.UserId, message.UserEmail, message.GameId, paymentStatus);

            await _publishEndpoint.Publish(paymentResult);

            _logger.LogInformation($"Evento 'PaymentProcessedEvent' enviado com sucesso para o tópico.");
        }
    }
}