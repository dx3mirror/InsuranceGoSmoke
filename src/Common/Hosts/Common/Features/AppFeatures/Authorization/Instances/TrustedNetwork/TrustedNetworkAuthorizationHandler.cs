using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using InsuranceGoSmoke.Common.Hosts.Features.AppFeatures.Authorization.Models;
using InsuranceGoSmoke.Common.Hosts.Features.AppFeatures.Authorization.Instances.TrustedNetwork.Models;
using InsuranceGoSmoke.Common.Contracts.Options;
using System.Text.Json;

namespace InsuranceGoSmoke.Common.Hosts.Features.AppFeatures.Authorization.Instances.TrustedNetwork
{
    /// <summary>
    /// Обработчик валидации доверенных сетей.
    /// </summary>
    public class TrustedNetworkAuthorizationHandler : AuthenticationHandler<TrustedNetworkOptions>
    {
        private readonly ITrustedNetworkValidator _validator;

        /// <summary>
        /// Инициализирует экземпляр <see cref="TrustedNetworkAuthorizationHandler"/>.
        /// </summary>
        public TrustedNetworkAuthorizationHandler(
            IOptionsMonitor<TrustedNetworkOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ITrustedNetworkValidator validator)
            : base(options, logger, encoder)
        {
            _validator = validator;
        }

        /// <inheritdoc />
        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var identity = Context.User.Identity;
            _ = identity ?? throw new AuthorizationException(nameof(identity));

            var schemeName = TrustedNetworkOptions.Scheme;

            if (identity.IsAuthenticated
                && identity.AuthenticationType != null
                && identity.AuthenticationType.Equals(schemeName, StringComparison.OrdinalIgnoreCase))
            {
                return Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(Context.User, schemeName)));
            }

            var isExistAuthenticateHeader = TryGetRoleFromAuthenticateHeader(schemeName, out _, out var failResult);
            if (!isExistAuthenticateHeader)
            {
                return failResult!;
            }

            var result = _validator.Validate(Request);
            if (!result)
            {
                Logger.LogError("Попытка аутентификации отклонена. Оригинальный Ip: {OriginalIp}: {Errors}",
                    result.OriginalIp, string.Join(Environment.NewLine, result.Errors));
                return Task.FromResult(AuthenticateResult.Fail(
                    "Аутентификация отклонена. Сеть не входит в перечень доверенных."));
            }

            var principal = new ClaimsPrincipal(CreateIdentity(schemeName));
            var trustedTicker = new AuthenticationTicket(principal, schemeName);

            return Task.FromResult(AuthenticateResult.Success(trustedTicker));
        }

        private bool TryGetRoleFromAuthenticateHeader(string schemeName, out string role, out Task<AuthenticateResult>? fromResult)
        {
            role = "";
            fromResult = null;

            if (!Request.Headers.TryGetValue(HeaderNames.Authorization, out var authorization) &&
                !Request.Headers.TryGetValue("AuthorizationHeader", out authorization))
            {
                fromResult = Task.FromResult(AuthenticateResult.Fail($"Не задан заголовок '{HeaderNames.Authorization}'"));
                return false;
            }

            var authorizationParts = authorization
                                        .ToString()
                                        .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                                        .Select(p => p.Trim());

            var scheme = authorizationParts.ElementAtOrDefault(index: 0);
            if (!schemeName.Equals(scheme, StringComparison.OrdinalIgnoreCase))
            {
                fromResult = Task.FromResult(AuthenticateResult.Fail("Задана неподдерживаемая схема аутентификации"));
                return false;
            }

            role = authorizationParts.ElementAtOrDefault(index: 1) ?? string.Empty;
            if (string.IsNullOrEmpty(role))
            {
                fromResult =
                    Task.FromResult(AuthenticateResult.Fail("Не задана роль пользователя, запросившего аутентификацию"));
                return false;
            }

            return true;
        }

        private static ClaimsIdentity CreateIdentity(string scheme)
        {
            var claims = new List<Claim>
            {
                new (ClaimTypes.Role, TrustedNetworkRoles.Service)
            };

            return new ClaimsIdentity(claims, scheme);
        }
    }
}
