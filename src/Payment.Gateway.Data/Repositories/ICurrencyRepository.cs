using System.Collections.Generic;
using System.Threading.Tasks;
using Payment.Gateway.Data.Entities;

namespace Payment.Gateway.Data.Repositories
{
    public interface ICurrencyRepository
    {
        Task<Currency> GetCurrencyByName(string currency);

        Task<Currency> GetCurrencyById(int currencyId);

        Task<IEnumerable<Currency>> GetAllCurrencies();
    }
}