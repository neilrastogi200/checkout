using System.Threading.Tasks;
using Payment.Gateway.Application.Models;

namespace Payment.Gateway.Application.Services
{
    public interface ICardDetailsService
    {
        bool IsValid(string expiryMonth, string expiryYear);
        int AddCardDetails(CardDetails card);
        Task<CardDetails> GetCardById(int paymentCardId);
        string MaskCardNumber(string cardNumber);
    }
}
