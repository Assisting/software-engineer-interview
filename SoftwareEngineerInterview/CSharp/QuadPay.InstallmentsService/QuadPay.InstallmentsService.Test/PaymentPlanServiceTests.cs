using System.Linq;
using QuadPay.InstallmentsService.Services;
using Shouldly;
using Xunit;

namespace QuadPay.InstallmentsService.Test
{
    public class PaymentPlanServiceTests
    {
        [Fact]
        public void WhenCreatePaymentPlanWithValidOrderAmount_ShouldReturnValidPaymentPlan()
        {
            // Arrange
            var paymentPlanService = new PaymentPlanService();
            
            // Act
            var paymentPlan = paymentPlanService.CreatePaymentPlan((decimal)123.45);

            // Assert
            paymentPlan.ShouldNotBeNull();
        }
        
        [Theory]
        [InlineData(123.44, 30.86, 30.86, 30.86, 30.86)]
        [InlineData(123.45, 30.87, 30.86, 30.86, 30.86)]
        [InlineData(123.46, 30.87, 30.87, 30.86, 30.86)]
        [InlineData(123.47, 30.87, 30.87, 30.87, 30.86)]
        [InlineData(123.48, 30.87, 30.87, 30.87, 30.87)]
        public void WhenCreatePaymentPlanWithValidOrderAmount_ShouldReturnValidPaymentPlanInstallmentAmounts(
            decimal amount,
            decimal firstInstallmentAmount,
            decimal secondInstallmentAmount,
            decimal thirdInstallmentAmount,
            decimal fourthInstallmentAmount
        )
        {
            // Arrange
            var paymentPlanService = new PaymentPlanService();
            
            // Act
            var paymentPlan = paymentPlanService.CreatePaymentPlan(amount);

            // Assert
            paymentPlan.ShouldNotBeNull();
            
            // Assert installment amounts
            var installments = paymentPlan.Installments.ToArray();
            
            installments.ToArray()[0].Amount.ShouldBe(firstInstallmentAmount);
            installments.ToArray()[1].Amount.ShouldBe(secondInstallmentAmount);
            installments.ToArray()[2].Amount.ShouldBe(thirdInstallmentAmount);
            installments.ToArray()[3].Amount.ShouldBe(fourthInstallmentAmount);
        }
    }
}
