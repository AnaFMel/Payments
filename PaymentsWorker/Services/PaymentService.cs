using PaymentsWorker.Contracts;

namespace PaymentsWorker.Services
{
    public static class PaymentService
    {
        public static PaymentStatus SimulatePayment()
        {
            var randomNumber = Random.Shared.Next(1, 101);

            return (randomNumber % 2 == 0) ? PaymentStatus.Approved : PaymentStatus.Rejected;
        }
    }
}
