using System.Threading.Tasks;
using Payment.Gateway.Data.Entities;

namespace Payment.Gateway.Data.Repositories
{
    public interface ICardDetailsRepository
    {
        int AddCardDetails(CardDetails cardDetails);
        Task<CardDetails> GetCardDetails(int cardId);
        Task<CardDetails> GetCardByCardnumber(string cardNumber);
    }
}