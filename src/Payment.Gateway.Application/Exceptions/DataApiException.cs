using System;
using System.Net;

namespace Payment.Gateway.Application.Exceptions
{
    public class DataApiException : Exception
    {
        public DataApiException(string message) : base(message)
        {
        }
        public HttpStatusCode ErrorStatusCode { get; set; } = HttpStatusCode.InternalServerError;
        public string Error { get; set; }
        public DateTime DateTime => DateTime.UtcNow;
    }
}
