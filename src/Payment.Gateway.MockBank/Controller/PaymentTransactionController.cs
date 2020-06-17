using System;
using Microsoft.AspNetCore.Mvc;
using Payment.Gateway.Application.Models;
using Payment.Gateway.MockBank.Models;

namespace Payment.Gateway.MockBank.Controller
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PaymentTransactionController : ControllerBase
    {
        [HttpPost]
        public IActionResult ProcessPaymentTransaction([FromBody]MockBankPaymentRequest request)
        {
            BankResponse bankResponse = new BankResponse();

            switch (request.Amount)
            {
                case 100:
                    bankResponse.Status = PaymentTransactionStatus.Failure;
                    bankResponse.Message = PaymentTransactionSubStatus.PaymentFailedToProcess;
                    bankResponse.BankReferenceIdentifier = Guid.Empty;
                    break;
                    default:
                        bankResponse.Status = PaymentTransactionStatus.Success;
                        bankResponse.Message = PaymentTransactionSubStatus.PaymentSuccessful;
                        bankResponse.BankReferenceIdentifier = Guid.NewGuid();
                        break;
            }

            return Ok(bankResponse);
        }
    }
}
