using System.Threading.Tasks;
using Payment.Gateway.Application.Models;
using Payment.Gateway.Data.Entities;
using Payment_Gateway.Models;

namespace Payment.Gateway.Application.Services
{
    public interface ITransactionService
    {
        Task<PaymentTransactionResponse> ProcessPaymentTransaction(ProcessPayment payment);
        Task<PaymentTransaction> GetPaymentTransaction(int paymentTransactionId);
    }
}
