using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Payment.Gateway.Application;
using Payment.Gateway.Application.Models.Response;
using Payment_Gateway.Models;

namespace Payment_Gateway.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentManager _paymentManager;
        private readonly ILogger<PaymentController> _logger;

        public PaymentController(IPaymentManager paymentManager, ILogger<PaymentController> logger)
        {
            _paymentManager = paymentManager;
            _logger = logger;
        }

        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ProcessPaymentTransactionResponse),200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> ProcessPayment(PaymentRequest paymentRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (paymentRequest.Amount == 0)
            {
                return BadRequest("The amount field needs to be greater than zero");
            }

            var payment = MapPaymentRequestToPayment(paymentRequest);

            var result = await _paymentManager.HandlePaymentAsync(payment);

            return Ok(result);
        }

        
        [HttpGet]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetPaymentTransaction(int id)
        {
            if (id == 0)
            {
                return BadRequest("TransactionId is invalid. It needs to be greater than 0.");
            }

            var paymentTransaction = await _paymentManager.GetPaymentTransactionByIdAsync(id);

            if (paymentTransaction != null)
            {
                return Ok(paymentTransaction);
            }

            _logger.LogWarning($"The transaction does not exist{id}");
            return NotFound("The transaction does not exist");
        }

        private Payment.Gateway.Application.Models.Payment MapPaymentRequestToPayment(PaymentRequest paymentRequest)
        {
            var paymentData = new Payment.Gateway.Application.Models.Payment()
            {
                Amount = paymentRequest.Amount,
                Card = new Payment.Gateway.Application.Models.CardDetails()
                {
                    CardExpiryYear = Convert.ToInt32(paymentRequest.Card.CardExpiryYear),
                    CardExpiryMonth = Convert.ToInt32(paymentRequest.Card.CardExpiryMonth),
                    CardHolderName = paymentRequest.Card.CardHolderName,
                    Cvv = paymentRequest.Card.Cvv,
                    CardNumber = paymentRequest.Card.CardNumber
                },
                Currency = paymentRequest.Currency,
                MerchantId = paymentRequest.MerchantId
            };

            return paymentData;
        }
    }
}
