﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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

            switch (request.TransactionAmount)
            {
                case 100:
                    bankResponse.Status = PaymentTransactionStatus.Failure;
                    bankResponse.BankReferenceIdentifier = Guid.Empty;
                    break;
                    default:
                        bankResponse.Status = PaymentTransactionStatus.Success;
                        bankResponse.BankReferenceIdentifier = Guid.NewGuid();
                        break;
            }

            return Ok(bankResponse);
        }
    }
}
