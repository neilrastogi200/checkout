using System.Threading.Tasks;
using Payment.Gateway.Application.Models;
using Payment.Gateway.Application.Models.Response;

namespace Payment.Gateway.Application.HttpClient
{
    public interface IApiClient
    {
        Task<BankResponse> ProcessPayment(ProcessPaymentData paymentData);
    }
}