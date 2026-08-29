using Fcg.Contracts;
using MassTransit;
using PaymentsWorker.Services;

namespace PaymentsWorker.Consumers
{
    public class OrderPlacedConsumer : IConsumer<OrderPlacedEvent>
    {
        private readonly ILogger<OrderPlacedEvent> _logger;
        private readonly string _queueCatalogName;
        private readonly string _queueNotificationsName;

        public OrderPlacedConsumer(ILogger<OrderPlacedEvent> logger, IConfiguration configuration)
        {
            _logger = logger;
            _queueCatalogName = configuration["PAYMENTS_CATALOG_QUEUE_NAME"] ?? "payments-1-queue";
            _queueNotificationsName = configuration["PAYMENTS_NOTIFICATION_QUEUE_NAME"] ?? "payments-2-queue";
        }

        public async Task Consume(ConsumeContext<OrderPlacedEvent> context)
        {
            var message = context.Message;

            _logger.LogInformation($"User ID: {message.UserId}.\nGame ID:{message.GameId}.\nProcessando o pagamento...");

            var paymentStatus = PaymentService.SimulatePayment();

            _logger.LogInformation($"Pagamento processado com sucesso! Resultado: {paymentStatus}");

            var paymentResult = new PaymentProcessedEvent
            {
                TransactionId = Guid.NewGuid(),
                OrderId = message.Id,
                UserId = message.UserId,
                UserEmail = message.UserEmail,
                GameId = message.GameId,
                Status = paymentStatus
            };

            var sendEndpointCatalog = await context.GetSendEndpoint(new Uri($"queue:{_queueCatalogName}"));
            await sendEndpointCatalog.Send(paymentResult);

            _logger.LogInformation($"Evento 'PaymentProcessedEvent' enviado com sucesso para a fila {_queueCatalogName}!");


            var sendEndpointNotification = await context.GetSendEndpoint(new Uri($"queue:{_queueNotificationsName}"));
            await sendEndpointNotification.Send(paymentResult);

            _logger.LogInformation($"Evento 'PaymentProcessedEvent' enviado com sucesso para a fila {_queueNotificationsName}!");
        }
    }
}