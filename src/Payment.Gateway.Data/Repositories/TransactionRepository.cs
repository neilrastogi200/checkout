using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Payment.Gateway.Data.Entities;

namespace Payment.Gateway.Data.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly PaymentContext _paymentContext;

        public TransactionRepository(PaymentContext paymentContext)
        {
            _paymentContext = paymentContext;
        }

        public async Task<IEnumerable<PaymentTransaction>> GetAllPaymentTransactions()
        {
            return await _paymentContext.PaymentTransactions.ToListAsync();
        }

        public async Task<PaymentTransaction> GetPaymentTransactionAsync(int transactionId)
        {
            return await _paymentContext.PaymentTransactions.Where(x => x.PaymentTransactionId == transactionId)
                .SingleOrDefaultAsync();
        }

        public int AddPaymentTransaction(PaymentTransaction paymentTransaction)
        {
            _paymentContext.Add(paymentTransaction);
            _paymentContext.SaveChanges();

            int paymentTransactionId = paymentTransaction.PaymentTransactionId;

            return paymentTransactionId;
        }

        public void UpdateTransaction(PaymentTransaction paymentTransaction)
        {
            _paymentContext.Update(paymentTransaction);
            _paymentContext.SaveChanges();
        }
    }
}
