using System.Threading.Tasks;
using Payment_Gateway.Models;

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
