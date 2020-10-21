using System.Threading.Tasks;
using QuadPay.InstallmentsService.Interfaces;
using QuadPay.InstallmentsService.Models;

namespace QuadPay.InstallmentsService.Services
{
    /// <summary>
    /// This class is responsible for building the PaymentPlan according to the QuadPay product definition.
    /// </summary>
    public class PaymentPlanService : IPaymentPlanService
    {
        /// <summary>
        /// Builds the PaymentPlan instance.
        /// </summary>
        /// <param name="purchaseAmount">The total amount for the purchase that the customer is making.</param>
        /// <returns>The PaymentPlan created with all properties set.</returns>
        public async Task<PaymentPlan> CreatePaymentPlan(decimal purchaseAmount)
        {
            // TODO
            return new PaymentPlan();
        }
    }
}