using System.Threading.Tasks;
using QuadPay.InstallmentsService.Models;

namespace QuadPay.InstallmentsService.Interfaces
{
    public interface IPaymentPlanService
    {
        /// <summary>
        /// Builds the PaymentPlan instance.
        /// </summary>
        /// <param name="purchaseAmount">The total amount for the purchase that the customer is making.</param>
        /// <returns>The PaymentPlan created with all properties set.</returns>
        Task<PaymentPlan> CreatePaymentPlan(decimal purchaseAmount);
    }
}