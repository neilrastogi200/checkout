using System.Collections.Generic;
using System.Threading.Tasks;
using Payment.Gateway.Data.Entities;

namespace Payment.Gateway.Data.Repositories
{
    public interface ITransactionRepository
    {
        Task<IEnumerable<PaymentTransaction>> GetAllPaymentTransactions();
        Task<PaymentTransaction> GetPaymentTransaction(int transactionId);
        int AddPaymentTransaction(PaymentTransaction paymentTransaction);
    }
}