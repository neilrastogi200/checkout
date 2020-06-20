using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Payment_Gateway.Authentication
{
    public class ApiKeyAuthenticationHandler : AuthenticationHandler<ApiKeyAuthenticationOptions>
    {
        private readonly IGetApiKey _getApiKey;
        public const string HeaderName = "X-Api-Key";

        public ApiKeyAuthenticationHandler(IOptionsMonitor<ApiKeyAuthenticationOptions> options, ILoggerFactory logger,
            UrlEncoder encoder, ISystemClock clock, IGetApiKey getApiKey) : base(options, logger, encoder, clock)
        {
            _getApiKey = getApiKey;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.TryGetValue(HeaderName, out var apiKeyHeaderValues))
            {
                Logger.Log(LogLevel.Warning, "No value present for the key");
                return AuthenticateResult.NoResult();
            }

            var providedApiKey = apiKeyHeaderValues.FirstOrDefault();

            if (apiKeyHeaderValues.Count == 0 || string.IsNullOrWhiteSpace(providedApiKey))
            {
                Logger.Log(LogLevel.Warning,"No value present for the key");
                return AuthenticateResult.NoResult();
                
            }

            var existingApiKey = await _getApiKey.Execute(providedApiKey);

            if (existingApiKey == null)
            {
                Logger.Log(LogLevel.Warning, "Invalid API Key provided.");
                return AuthenticateResult.Fail("Invalid API Key provided.");
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, existingApiKey.Key)
            };

            var identity = new ClaimsIdentity(claims, Options.AuthenticationType);
            var identities = new List<ClaimsIdentity> {identity};
            var principal = new ClaimsPrincipal(identities);
            var ticket = new AuthenticationTicket(principal, Options.Scheme);

            return AuthenticateResult.Success(ticket);
        }

    }
}
