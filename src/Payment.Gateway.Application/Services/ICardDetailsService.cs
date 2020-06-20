using System.Threading.Tasks;
using Payment.Gateway.Application.Models;

namespace Payment.Gateway.Application.Services
{
    public interface ICardDetailsService
    {
        bool IsValid(int expiryMonth, int expiryYear);
        int AddCardDetails(CardDetails card);
        Task<CardDetails> GetCardByIdAsync(int paymentCardId);
        string MaskCardNumber(string cardNumber);
    }
}
