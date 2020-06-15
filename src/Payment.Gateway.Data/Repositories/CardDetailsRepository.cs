using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Payment.Gateway.Data.Entities;

namespace Payment.Gateway.Data.Repositories
{
    public class CardDetailsRepository : ICardDetailsRepository
    {
        private readonly PaymentContext _paymentContext;
        public CardDetailsRepository(PaymentContext paymentContext)
        {
            _paymentContext = paymentContext;
        }

        public int AddCardDetails(CardDetails cardDetails)
        {
            _paymentContext.Add(cardDetails);
            _paymentContext.SaveChanges();

            var id = cardDetails.CardId;

            return id;
        }

        public async Task<CardDetails> GetCardDetails(int cardId)
        {
            return await _paymentContext.FindAsync<CardDetails>(cardId);
        }

        public async Task<CardDetails> GetCardByCardnumber(string cardNumber)
        {
            return await _paymentContext.FindAsync<CardDetails>(cardNumber);
        }
    }
}
