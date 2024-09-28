using IdentityModel.Client;
using InsuranceGoSmoke.Common.Clients.Keycloak;
using InsuranceGoSmoke.Common.Clients.Keycloak.Models.Responses;

namespace InsuranceGoSmoke.Security.ExternalClients.Keycloak
{
    /// <inheritdoc/>
    public class KeycloakAuthorizationApiClient : IKeycloakAuthorizationApiClient
    {
        private readonly IKeycloakAuthorizationApiExternalClient _externalApiClient;

        /// <summary>
        /// Создаёт экземпляр <see cref="KeycloakAuthorizationApiClient"/>
        /// </summary>
        /// <param name="externalApiClient">Клиент внешнего API сервиса работы с Keycloak.</param>
        public KeycloakAuthorizationApiClient(IKeycloakAuthorizationApiExternalClient externalApiClient)
        {
            _externalApiClient = externalApiClient;
        }

        /// <inheritdoc/>
        public Task<KeycloakResponse> TrySendSmsAuthenticationCodeAsync(string phoneNumber, CancellationToken cancellationToken)
        {
            return _externalApiClient.TrySendSmsAuthenticationCodeAsync(phoneNumber, cancellationToken);
        }

        /// <inheritdoc/>
        public Task<TokenResponse> AuthorizationByPhoneAsync(string phoneNumber, string code, CancellationToken cancellationToken)
        {
            return _externalApiClient.AuthorizationByPhoneAsync(phoneNumber, code, cancellationToken);
        }

        /// <inheritdoc/>
        public Task<TokenResponse> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken)
        {
            return _externalApiClient.RefreshTokenAsync(refreshToken, cancellationToken);
        }

        /// <inheritdoc/>
        public Task LogoutAsync(Guid userId, CancellationToken cancellationToken)
        {
            return _externalApiClient.LogoutAsync(userId, cancellationToken);
        }
    }
}
