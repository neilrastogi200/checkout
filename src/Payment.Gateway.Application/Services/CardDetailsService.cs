using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Payment.Gateway.Application.Models;
using Payment.Gateway.Data.Repositories;

namespace Payment.Gateway.Application.Services
{
    public class CardDetailsService : ICardDetailsService
    {
        private readonly ICardDetailsRepository _cardDetailsRepository;

        public CardDetailsService(ICardDetailsRepository cardDetailsRepository)
        {
            _cardDetailsRepository = cardDetailsRepository ?? throw new ArgumentNullException(nameof(cardDetailsRepository));
        }

        public bool IsValid(string expiryMonth, string expiryYear)
        {
            var lastDateOfExpiryMonth = DateTime.DaysInMonth(Convert.ToInt32(expiryYear), Convert.ToInt32(expiryMonth));
            var cardExpiry = new DateTime(Convert.ToInt32(expiryYear), Convert.ToInt32(expiryMonth), lastDateOfExpiryMonth);

            return (cardExpiry > DateTime.Now && cardExpiry < DateTime.Now.AddYears(6));
        }

        public int AddCardDetails(CardDetails card)
        {
            Payment.Gateway.Data.Entities.CardDetails cardDetails = new Payment.Gateway.Data.Entities.CardDetails()
            {
                CardExpiryMonth = card.CardExpiryMonth,
                CardExpiryYear = card.CardExpiryYear,
                CardHolderName = card.CardHolderName,
                Cvv = card.Cvv,
                CardNumber = card.CardNumber
            };

            var cardId = _cardDetailsRepository.AddCardDetails(cardDetails);

            return cardId;
        }

        public async Task<CardDetails> GetCardById(int paymentCardId)
        {
           var cardPaymentDetails = await _cardDetailsRepository.GetCardDetails(paymentCardId);

           CardDetails cardDetails = new CardDetails()
           {
               CardExpiryMonth = cardPaymentDetails.CardExpiryMonth,
               CardExpiryYear = cardPaymentDetails.CardExpiryYear,
               CardHolderName = cardPaymentDetails.CardHolderName,
               Cvv = cardPaymentDetails.Cvv,
               CardNumber = cardPaymentDetails.CardNumber
           };

           return cardDetails;
        }

        public string MaskCardNumber(string cardNumber)
        {
            var lastDigits = cardNumber.Substring(cardNumber.Length - 4, 4);

            var requiredMask = new String('X', cardNumber.Length - lastDigits.Length);

            var maskedString = string.Concat(requiredMask, lastDigits);
            var maskedCardNumberWithSpaces = Regex.Replace(maskedString, ".{4}", "$0 ");

            return maskedCardNumberWithSpaces;
        }
    }
}
