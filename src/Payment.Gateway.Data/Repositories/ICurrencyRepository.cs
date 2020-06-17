using System.Collections.Generic;
using System.Threading.Tasks;
using Payment.Gateway.Data.Entities;

namespace Payment.Gateway.Data.Repositories
{
    public interface ICurrencyRepository
    {
        Task<Currency> GetCurrencyByNameAsync(string currency);

        Task<Currency> GetCurrencyByIdAsync(int currencyId);

        Task<IEnumerable<Currency>> GetAllCurrenciesAsync();
    }
}