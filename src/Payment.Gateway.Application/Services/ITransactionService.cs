using System.Threading.Tasks;
using Payment.Gateway.Application.Models;
using Payment.Gateway.Data.Entities;

namespace Payment.Gateway.Application.Services
{
    public interface ITransactionService
    {
        Task<ProcessPaymentTransactionResponse> ProcessPaymentTransactionAsync(ProcessPayment payment);
        Task<PaymentTransaction> GetPaymentTransaction(int paymentTransactionId);
    }
}
