using System.Threading.Tasks;
using Payment.Gateway.Application.Models.Response;

namespace Payment.Gateway.Application
{
    public interface IPaymentManager
    {
        Task<ProcessPaymentTransactionResponse> HandlePaymentAsync(Models.Payment payment);
        Task<PaymentTransactionResponse> GetPaymentTransactionByIdAsync(int paymentTransactionId);
    }
}
