using ApiKeyAuth.Models;
using ApiKeyAuth.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace ApiKeyAuth.Authentication
{
    public class ApiKeyAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly UserService _userService;

        public ApiKeyAuthHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            UserService userService)
            : base(options, logger, encoder, clock)
        {
            _userService = userService;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var apiKey = GetApiKey();
            if (string.IsNullOrWhiteSpace(apiKey))
                return Task.FromResult(AuthenticateResult.Fail("No API key provided"));

            var client = _userService.GetUserByToken(apiKey);
            if (client is null)
                return Task.FromResult(AuthenticateResult.Fail("Invalid API key"));

            var principal = CreatePrincipal(client);
            AuthenticationTicket ticket = new(principal, "ApiKey");

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }

        protected override Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            Response.Headers.WWWAuthenticate = new StringValues("MY-APP-AUTH-TOKEN");
            Response.StatusCode = StatusCodes.Status401Unauthorized;

            return Task.CompletedTask;
        }

        private string? GetApiKey()
        {
            if (!Context.Request.Headers.TryGetValue("MY-APP-AUTH-TOKEN", out StringValues keyValue))
                return null;

            if (!keyValue.Any())
                return null;

            return keyValue.ElementAt(0);
        }

        private static ClaimsPrincipal CreatePrincipal(AppUser client)
        {
            ClaimsIdentity identity = new("ApiKey");
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, client.Id.ToString()));
            identity.AddClaim(new Claim(ClaimTypes.Name, client.Name));

            return new ClaimsPrincipal(identity);
        }
    }
}