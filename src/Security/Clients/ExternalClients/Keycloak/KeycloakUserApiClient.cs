using InsuranceGoSmoke.Common.Clients.Keycloak;
using InsuranceGoSmoke.Common.Clients.Keycloak.Models;
using InsuranceGoSmoke.Common.Clients.Keycloak.Models.Responses;
using InsuranceGoSmoke.Common.Contracts.Contracts.Access.Enums;

namespace InsuranceGoSmoke.Security.ExternalClients.Keycloak
{
    /// <inheritdoc />
    public class KeycloakUserApiClient : IKeycloakUserApiClient
    {
        private readonly IKeycloakUserApiExternalClient _externalApiClient;

        /// <summary>
        /// Создаёт экземпляр <see cref="KeycloakUserApiClient"/>
        /// </summary>
        /// <param name="externalApiClient">Клиент внешнего API сервиса работы с Keycloak.</param>
        public KeycloakUserApiClient(IKeycloakUserApiExternalClient externalApiClient)
        {
            _externalApiClient = externalApiClient;
        }

        /// <inheritdoc/>
        public Task<KeycloakUserData?> GetUserAsync(Guid userId, CancellationToken cancellationToken)
        {
            return _externalApiClient.GetUserAsync(userId, cancellationToken);
        }

        /// <inheritdoc/>
        public Task EmailVerificationAsync(Guid userId, string email, CancellationToken cancellationToken)
        {
            return _externalApiClient.EmailVerificationAsync(userId, email, cancellationToken);
        }

        /// <inheritdoc/>
        public Task UpdateUserAsync(KeycloakUserData userData, CancellationToken cancellationToken)
        {
            return _externalApiClient.UpdateUserAsync(userData, cancellationToken);
        }

        /// <inheritdoc/>
        public Task UpdateUserFieldAsync(KeycloakUserData userData, CancellationToken cancellationToken)
        {
            return _externalApiClient.UpdateUserFieldAsync(userData, cancellationToken);
        }

        /// <inheritdoc/>
        public Task<KeycloakResponse> VerifyCodeAsync(string phoneNumber, string code, CancellationToken cancellationToken)
        {
            return _externalApiClient.VerifyCodeAsync(phoneNumber, code, cancellationToken);
        }

        /// <inheritdoc/>
        public Task UpdatePhoneNumberAsync(Guid userId, string phoneNumber, CancellationToken cancellationToken)
        {
            return _externalApiClient.UpdatePhoneNumberAsync(userId, phoneNumber, cancellationToken);
        }

        /// <inheritdoc/>
        public Task<KeycloakResponse> SendVerificationCodeAsync(string phoneNumber, CancellationToken cancellationToken)
        {
            return _externalApiClient.TrySendSmsVerificationCodeAsync(phoneNumber, cancellationToken);
        }

        /// <inheritdoc/>
        public Task<IReadOnlyCollection<KeycloakUsersListItemData>> GetUsersByFilterAsync(UserFilter filter, CancellationToken cancellationToken)
        {
            return _externalApiClient.GetUsersByFilterAsync(filter, cancellationToken);
        }

        /// <inheritdoc/>
        public Task<int> GetUsersCountByFilterAsync(UserFilter filter, CancellationToken cancellationToken)
        {
            return _externalApiClient.GetUsersCountByFilterAsync(filter, cancellationToken);
        }

        /// <inheritdoc/>
        public Task SetStatusUserAsync(Guid userId, bool isEnabled, CancellationToken cancellationToken)
        {
            return _externalApiClient.SetStatusUserAsync(userId, isEnabled, cancellationToken);
        }

        /// <inheritdoc/>
        public Task ChangeUserRoleAsync(Guid userId, RoleTypes role, CancellationToken cancellationToken)
        {
            return _externalApiClient.ChangeUserRoleAsync(userId, role.ToString(), cancellationToken);
        }

        /// <inheritdoc/>
        public Task<IReadOnlyCollection<RoleTypes>> GetCachedUserRolesAsync(Guid userId, CancellationToken cancellationToken)
        {
            return _externalApiClient.GetCachedUserRolesAsync(userId, cancellationToken);
        }

        /// <inheritdoc/>
        public Task<int> GetUsersCountByQueryAsync(string? query, CancellationToken cancellationToken)
        {
            return _externalApiClient.GetUsersCountByQueryAsync(query, cancellationToken);
        }

        /// <inheritdoc/>
        public Task<IReadOnlyCollection<KeycloakUsersListItemData>> GetUsersByQueryAsync(string? query, int take, int? skip, CancellationToken cancellationToken)
        {
            return _externalApiClient.GetUsersByQueryAsync(query, take, skip, cancellationToken);
        }
    }
}
