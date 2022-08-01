using System;

namespace Zip.InstallmentsService
{
    public interface IPaymentPlanFactory
    {
        PaymentPlan CreatePaymentPlan(decimal purchaseAmount, int installments = 4);
    }

    /// <summary>
    /// This class is responsible for building the PaymentPlan according to the Zip product definition.
    /// </summary>
    public class PaymentPlanFactory : IPaymentPlanFactory
    {
        /// <summary>
        /// Builds the PaymentPlan instance.
        /// </summary>
        /// <param name="purchaseAmount">The total amount for the purchase that the customer is making.</param>
        /// <returns>The PaymentPlan created with all properties set.</returns>
        public PaymentPlan CreatePaymentPlan(decimal purchaseAmount, int installments = 4)
        {
            PaymentPlan plan = new PaymentPlan()
            {
                Id = Guid.NewGuid(),
                PurchaseAmount = purchaseAmount,
                Installments = new Installment[installments]
            };

            decimal standardPaymentAmount = Math.Round(purchaseAmount / installments, 2, MidpointRounding.ToNegativeInfinity);
            decimal remainderAmount = purchaseAmount - (standardPaymentAmount * installments);
            for (int i = 0; i < installments; i++)
            {
                plan.Installments[i] = new Installment()
                {
                    Id = Guid.NewGuid(),
                    DueDate = DateTime.UtcNow.AddDays(i * 14),
                    Amount = standardPaymentAmount
                };

                if (remainderAmount != 0M)
                {
                    plan.Installments[i].Amount += 0.01M;
                    remainderAmount -= 0.01M;
                }
            }

            return plan;
        }
    }
}
