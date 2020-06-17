using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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

            var result = await _paymentManager.HandlePaymentAsync(paymentRequest);

            return Ok(result);
        }

        [HttpGet]
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
    }
}
