using System.Threading.Tasks;
using Payment.Gateway.Application.Models;
using Payment_Gateway.Models;

namespace Payment_Gateway
{
    public interface IPaymentManager
    {
        Task<ProcessPaymentTransactionResponse> HandlePayment(PaymentRequest paymentRequest);
        Task<PaymentTransactionResponse> GetPaymentTransactionById(int paymentTransactionId);
    }
}
