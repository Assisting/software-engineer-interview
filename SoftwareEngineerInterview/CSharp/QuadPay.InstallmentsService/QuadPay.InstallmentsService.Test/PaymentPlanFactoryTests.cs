using QuadPay.InstallmentsService.Services;
using Shouldly;
using Xunit;

namespace QuadPay.InstallmentsService.Test
{
    public class PaymentPlanFactoryTests
    {
        [Fact]
        public void WhenCreatePaymentPlanWithValidOrderAmount_ShouldReturnValidPaymentPlan()
        {
            // Arrange
            var paymentPlanService = new PaymentPlanService();
            
            // Act
            var paymentPlan = paymentPlanService.CreatePaymentPlan(123.45M);

            // Assert
            paymentPlan.ShouldNotBeNull();
        }
    }
}
