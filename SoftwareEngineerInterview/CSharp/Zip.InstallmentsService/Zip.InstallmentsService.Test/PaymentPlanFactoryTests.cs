using Shouldly;
using Xunit;
using System;
using System.Linq;

namespace Zip.InstallmentsService.Test
{
    public class PaymentPlanFactoryTests
    {
        [Fact]
        public void WhenCreatePaymentPlanWithValidOrderAmount_ShouldReturnValidPaymentPlan()
        {
            // Arrange
            var paymentPlanFactory = new PaymentPlanFactory();

            // Act
            var paymentPlan = paymentPlanFactory.CreatePaymentPlan(123.45M);

            // Assert
            paymentPlan.ShouldNotBeNull();
        }

        [Fact]
        public void HundredDollarPayment_ShouldHaveFourBiweeklyInstallmentsOf25Dollars()
        {
            // Arrange
            var paymentPlanFactory = new PaymentPlanFactory();

            // Act
            var paymentPlan = paymentPlanFactory.CreatePaymentPlan(100.00M);

            // Assert
            paymentPlan.Installments.Length.ShouldBe<int>(4);
            paymentPlan.Installments[0].Amount.ShouldBe<decimal>(25.00M);
            paymentPlan.Installments[1].Amount.ShouldBe<decimal>(25.00M);
            paymentPlan.Installments[2].Amount.ShouldBe<decimal>(25.00M);
            paymentPlan.Installments[3].Amount.ShouldBe<decimal>(25.00M);
        }

        [Fact]
        public void WhenCreatePaymentPlan_PaymentAmountShouldBeTheSame()
        {
            // Arrange
            var paymentPlanFactory = new PaymentPlanFactory();
            decimal potentiallyWeirdNumber = 16.22M;

            // Act
            var paymentPlan = paymentPlanFactory.CreatePaymentPlan(potentiallyWeirdNumber);

            // Assert
            paymentPlan.PurchaseAmount.ShouldBe<decimal>(potentiallyWeirdNumber);
        }

        [Fact]
        public void WhenCreatingPaymentPlan_IdShouldNotBeEmpty()
        {
            // Arrange
            var paymentPlanFactory = new PaymentPlanFactory();

            // Act
            var paymentPlan = paymentPlanFactory.CreatePaymentPlan(123.45M);

            // Assert
            paymentPlan.Id.ShouldNotBe<Guid>(Guid.Empty);
        }

        [Fact]
        public void WhenCreatingNonDivisiblePayment_PaymentsShouldAddUpToTotal()
        {
            // Arrange
            var paymentPlanFactory = new PaymentPlanFactory();

            // Act
            var paymentPlan = paymentPlanFactory.CreatePaymentPlan(123.45M);

            // Assert
            paymentPlan.Installments.Sum(installment => installment.Amount).ShouldBe<decimal>(123.45M);
        }

        [Fact]
        public void WhenCreatingNonDivisiblePayment_LastPaymentAmountShouldDifferFromFirst()
        {
            // Arrange
            var paymentPlanFactory = new PaymentPlanFactory();

            // Act
            var paymentPlan = paymentPlanFactory.CreatePaymentPlan(123.45M);

            // Assert
            paymentPlan.Installments[0].Amount.ShouldNotBe<decimal>(paymentPlan.Installments[3].Amount);
        }

        [Fact]
        public void WhenCreatingAFiveInstallmentPaymentPlan_PaymentsShouldBeTwentyDollars()
        {
            // Arrange
            var paymentPlanFactory = new PaymentPlanFactory();

            // Act
            var paymentPlan = paymentPlanFactory.CreatePaymentPlan(100.00M, 5);

            // Assert
            paymentPlan.Installments.Length.ShouldBe<int>(5);
            paymentPlan.Installments[0].Amount.ShouldBe<decimal>(20.00M);
            paymentPlan.Installments[1].Amount.ShouldBe<decimal>(20.00M);
            paymentPlan.Installments[2].Amount.ShouldBe<decimal>(20.00M);
            paymentPlan.Installments[3].Amount.ShouldBe<decimal>(20.00M);
            paymentPlan.Installments[4].Amount.ShouldBe<decimal>(20.00M);
        }

        [Fact]
        public void WhenCreatingASlightlyLowerPurchaseAmount_PaymentsMustStillMakeSense()
        {
            // Arrange
            var paymentPlanFactory = new PaymentPlanFactory();

            // Act
            var paymentPlan = paymentPlanFactory.CreatePaymentPlan(99.97M, 5);

            // Assert
            paymentPlan.Installments.Sum(installment => installment.Amount).ShouldBe<decimal>(99.97M);
            paymentPlan.Installments.Length.ShouldBe<int>(5);
            paymentPlan.Installments[0].Amount.ShouldBe<decimal>(20.00M);
            paymentPlan.Installments[1].Amount.ShouldBe<decimal>(20.00M);
            paymentPlan.Installments[2].Amount.ShouldBe<decimal>(19.99M);
            paymentPlan.Installments[3].Amount.ShouldBe<decimal>(19.99M);
            paymentPlan.Installments[4].Amount.ShouldBe<decimal>(19.99M);
        }
    }
}
