using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Payment.Gateway.Data.Entities;

namespace Payment.Gateway.Data.Repositories
{
    public class CurrencyRepository : ICurrencyRepository
    {
        private readonly PaymentContext _paymentContext;

        public CurrencyRepository(PaymentContext paymentContext)
        {
            _paymentContext = paymentContext;
        }

        public async Task<Currency> GetCurrencyByName(string currency)
        {
            return await _paymentContext.Currencies.Where(x => x.Name == currency).FirstOrDefaultAsync();
        }

        public async Task<Currency> GetCurrencyById(int currencyId)
        {
            return await _paymentContext.FindAsync<Currency>(currencyId);
        }

        public async Task<IEnumerable<Currency>> GetAllCurrencies()
        {
            return await _paymentContext.Currencies.ToListAsync();
        }
    }
}
