using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Payment.Gateway.Application.Models;
using Payment.Gateway.Data.Repositories;

namespace Payment.Gateway.Application.Services
{
    public class CardDetailsService : ICardDetailsService
    {
        private readonly ICardDetailsRepository _cardDetailsRepository;
        private readonly ILogger<CardDetailsService> _logger;

        public CardDetailsService(ICardDetailsRepository cardDetailsRepository, ILogger<CardDetailsService> logger)
        {
            _cardDetailsRepository = cardDetailsRepository ?? throw new ArgumentNullException(nameof(cardDetailsRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public bool IsValid(string expiryMonth, string expiryYear)
        {
            _logger.LogInformation("CardDetailsService:Entering the isValid method");
            var lastDateOfExpiryMonth = DateTime.DaysInMonth(Convert.ToInt32(expiryYear), Convert.ToInt32(expiryMonth));
            var cardExpiry = new DateTime(Convert.ToInt32(expiryYear), Convert.ToInt32(expiryMonth), lastDateOfExpiryMonth);

            return (cardExpiry > DateTime.Now && cardExpiry < DateTime.Now.AddYears(6));
        }

        public int AddCardDetails(CardDetails card)
        {
            var cardDetails = MapCardDetails(card);

            var cardId = _cardDetailsRepository.AddCardDetails(cardDetails);

            return cardId;
        }

        private Data.Entities.CardDetails MapCardDetails(CardDetails card)
        {
            var cardDetails = new Data.Entities.CardDetails()
            {
                CardExpiryMonth = card.CardExpiryMonth,
                CardExpiryYear = card.CardExpiryYear,
                CardHolderName = card.CardHolderName,
                Cvv = card.Cvv,
                CardNumber = card.CardNumber
            };
            return cardDetails;
        }

        public async Task<CardDetails> GetCardByIdAsync(int paymentCardId)
        {
           var cardPaymentDetails = await _cardDetailsRepository.GetCardDetailsAsync(paymentCardId);

           var cardDetails = MapCardDetailsFromDto(cardPaymentDetails);

           return cardDetails;
        }

        private CardDetails MapCardDetailsFromDto(Data.Entities.CardDetails cardPaymentDetails)
        {
            var cardDetails = new CardDetails()
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

            var requiredMask = new string('X', cardNumber.Length - lastDigits.Length);

            var maskedString = string.Concat(requiredMask, lastDigits);
            var maskedCardNumberWithSpaces = Regex.Replace(maskedString, ".{4}", "$0");

            return maskedCardNumberWithSpaces;
        }
    }
}
