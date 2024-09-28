using InsuranceGoSmoke.Common.Clients.Keycloak;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Claims;

namespace InsuranceGoSmoke.Common.Hosts.Api.Features.AppFeatures.Authorization.Instances.Keycloak
{
    /// <summary>
    /// Трансформер claim'ов.
    /// </summary>
    public class ClaimsTransformer : IClaimsTransformation
    {
        private readonly IServiceProvider _provider;

        /// <summary>
        /// Создаёт экземпляр <see cref="ClaimsTransformer"/>
        /// </summary>
        /// <param name="provider">Провайдер сервисов.</param>
        public ClaimsTransformer(IServiceProvider provider)
        {
            _provider = provider;
        }

        /// <summary>
        /// Изменяет claim'ы
        /// </summary>
        /// <param name="principal">Principal.</param>
        /// <returns>Principal</returns>
        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            var claimsIdentity = principal.Identity as ClaimsIdentity;
            if (claimsIdentity?.IsAuthenticated != true)
            {
                return principal;
            }

            var sub = principal.FindFirst(JwtRegisteredClaimNames.Sub);
            if (!Guid.TryParse(sub?.Value, out var userId))
            {
                return principal;
            }

            var _keycloakUserApiClient = _provider.GetRequiredService<IKeycloakUserApiExternalClient>();
            using var cancelTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(30));
            CancellationToken cancellationToken = cancelTokenSource.Token;
            var roles = await _keycloakUserApiClient.GetCachedUserRolesAsync(userId, cancellationToken);
            foreach (var role in roles)
            {
                claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, role.ToString()));
            }

            return principal;
        }
    }
}
