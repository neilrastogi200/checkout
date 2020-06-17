using System.Threading.Tasks;
using Payment.Gateway.Application.Models;
using Payment_Gateway.Models;

namespace Payment_Gateway
{
    public interface IPaymentManager
    {
        Task<ProcessPaymentTransactionResponse> HandlePaymentAsync(PaymentRequest paymentRequest);
        Task<PaymentTransactionResponse> GetPaymentTransactionByIdAsync(int paymentTransactionId);
    }
}
