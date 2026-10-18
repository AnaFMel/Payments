using Fcg.Contracts;
using PaymentsWorker.Services;

namespace PaymentsWorker.Tests.Services
{
    public class PaymentServiceTests
    {
        [Fact]
        public void SimulatePayment_ShouldReturnValidStatus()
        {
            // Act
            var status = PaymentService.SimulatePayment();

            // Assert
            Assert.True(
                status == PaymentStatus.Approved || status == PaymentStatus.Rejected,
                "O status gerado deve ser Approved ou Rejected."
            );
        }
    }
}