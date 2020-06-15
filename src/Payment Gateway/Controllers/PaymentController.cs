using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Payment_Gateway.Models;
using Payment_Gateway.Services;

namespace Payment_Gateway.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentManager _paymentManager;

        public PaymentController(IPaymentManager paymentManager)
        {
            _paymentManager = paymentManager;
        }

        [HttpPost]
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

            var result = await _paymentManager.HandlePayment(paymentRequest);

            return Ok("Successfully processed payment");
        }

        [HttpGet]
        public async Task<IActionResult> GetPaymentTransaction(int transactionId)
        {
            if (transactionId == 0)
            {
                return BadRequest("TransactionId is invalid. It needs to be greater than 0.");
            }

            var paymentTransaction = await _paymentManager.GetPaymentTransactionById(transactionId);

            if (paymentTransaction != null)
            {
                return Ok(paymentTransaction);
            }

            return NotFound("The transaction does not exist");
        }
    }
}
