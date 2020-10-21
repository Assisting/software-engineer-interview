using System;
using System.Collections.Generic;
using System.Linq;
using QuadPay.InstallmentsService.Interfaces;
using QuadPay.InstallmentsService.Models;

namespace QuadPay.InstallmentsService.Services
{
    /// <summary>
    /// This class is responsible for building the PaymentPlan according to the QuadPay product definition.
    /// </summary>
    public class PaymentPlanService : IPaymentPlanService
    {
        private const int NumberOfPayments = 4;
        private const int WeekPayInterval = 2;
        
        /// <summary>
        /// Builds the PaymentPlan instance.
        /// </summary>
        /// <param name="purchaseAmount">The total amount for the purchase that the customer is making.</param>
        /// <returns>The PaymentPlan created with all properties set.</returns>
        public PaymentPlan CreatePaymentPlan(decimal purchaseAmount)
        {
            var baseInstallmentAmount = Math.Truncate(purchaseAmount / NumberOfPayments * 100) / 100;
            var remainder = Math.Round(purchaseAmount % baseInstallmentAmount / 100 * 100, 2);
            var installments = GenerateBaseInstallments(baseInstallmentAmount, NumberOfPayments, WeekPayInterval);

            if (remainder > 0)
            {
                DistributeRemainder(installments, remainder);
            }
            
            return new PaymentPlan
            {
                Id = Guid.NewGuid(),
                PurchaseAmount = purchaseAmount,
                Installments = installments
            };
        }
        
        private IList<Installment> GenerateBaseInstallments(decimal baseInstallmentAmount, int numberOfPayments, int weekPayInterval)
        {
            var installments = new List<Installment>();
		
            for (var i = 0; i < numberOfPayments; i++)
            {
                var paymentDate = DateTime.UtcNow;
                var amountDue = baseInstallmentAmount;
			
                if (i > 0) {
                    var daysOut = (weekPayInterval * 7) * i;
                    paymentDate = paymentDate.AddDays(daysOut);
                }
			
                installments.Add(new Installment
                {
                    DueDate = paymentDate,
                    Amount = amountDue
                });
            }

            return installments;
        }
        
        private void DistributeRemainder(IList<Installment> installments, decimal remainder)
        {
            var iterations = (int)(remainder * 100);
		
            for (var i = 0; i < iterations; i++)
            {
                installments.ToArray()[i].Amount += (decimal)0.01;
            }
        }
    }
}
