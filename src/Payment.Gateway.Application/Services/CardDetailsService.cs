using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Payment.Gateway.Application.Encryption;
using Payment.Gateway.Application.Models;
using Payment.Gateway.Data.Repositories;

namespace Payment.Gateway.Application.Services
{
    public class CardDetailsService : ICardDetailsService
    {
        private readonly ICardDetailsRepository _cardDetailsRepository;
        private readonly ILogger<CardDetailsService> _logger;
        private readonly IAesCryptoService _cryptography;
        private const string EncryptionKey = "cardData";

        public CardDetailsService(ICardDetailsRepository cardDetailsRepository, ILogger<CardDetailsService> logger, IAesCryptoService cryptography)
        {
            _cardDetailsRepository = cardDetailsRepository ?? throw new ArgumentNullException(nameof(cardDetailsRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _cryptography = cryptography ?? throw new ArgumentNullException(nameof(cryptography));
        }

        public bool IsValid(int expiryMonth, int expiryYear)
        {
            _logger.LogInformation("CardDetailsService:Entering the isValid method");
            var lastDateOfExpiryMonth = DateTime.DaysInMonth(expiryYear, expiryMonth);
            var cardExpiry = new DateTime(expiryYear, expiryMonth, lastDateOfExpiryMonth);

            return (cardExpiry > DateTime.Now && cardExpiry < DateTime.Now.AddYears(6));
        }

        public int AddCardDetails(CardDetails card)
        {
            var cardDetails = MapCardDetails(card);

           var encryptedCardNumber =  _cryptography.Encrypt(cardDetails.CardNumber, EncryptionKey);

           cardDetails.CardNumber = encryptedCardNumber;

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
            var decryptedCardNum = DecryptCardNumber(cardNumber);
            var lastDigits = decryptedCardNum.Substring(decryptedCardNum.Length - 4, 4);

            var requiredMask = new string('X', decryptedCardNum.Length - lastDigits.Length);

            var maskedString = string.Concat(requiredMask, lastDigits);
            var maskedCardNumberWithSpaces = Regex.Replace(maskedString, ".{4}", "$0 ");

            return maskedCardNumberWithSpaces;
        }

        private string DecryptCardNumber(string cardNumber)
        {
           var decryptedCardNumber = _cryptography.Decrypt(cardNumber, EncryptionKey);

           return decryptedCardNumber;
        }
    }
}
