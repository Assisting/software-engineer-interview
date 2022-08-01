using System;

namespace Zip.InstallmentsService
{
    /// <summary>
    /// This class is responsible for building the PaymentPlan according to the Zip product definition.
    /// </summary>
    public class PaymentPlanFactory
    {
        /// <summary>
        /// Builds the PaymentPlan instance.
        /// </summary>
        /// <param name="purchaseAmount">The total amount for the purchase that the customer is making.</param>
        /// <returns>The PaymentPlan created with all properties set.</returns>
        public PaymentPlan CreatePaymentPlan(decimal purchaseAmount)
        {
            PaymentPlan plan = new PaymentPlan()
            {
                Id = Guid.NewGuid(),
                PurchaseAmount = purchaseAmount,
                Installments = new Installment[4]
            };

            decimal standardPaymentAmount = Math.Round(purchaseAmount / 4, 2);
            decimal lastPayment = purchaseAmount - (standardPaymentAmount * 3);

            for (int i = 0; i < 4; i++)
            {
                plan.Installments[i] = new Installment()
                {
                    Id = Guid.NewGuid(),
                    DueDate = DateTime.UtcNow.AddDays(i * 14),
                    Amount = standardPaymentAmount
                };
            }

            // Last payment may have an odd amount due to decimal rounding of other payments
            plan.Installments[3].Amount = lastPayment;

            return plan;
        }
    }
}
