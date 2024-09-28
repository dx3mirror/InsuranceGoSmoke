using IdentityModel.Client;
using InsuranceGoSmoke.Common.Clients.Keycloak.Models.Responses;

namespace InsuranceGoSmoke.Security.Applications.AppServices.Contexts.Identify.Factories.Strategies
{
    /// <summary>
    /// Интерфейс стратегии авторизации.
    /// </summary>
    public interface ISendingAuthorizationCodeStrategy
    {
        /// <summary>
        /// Отправляет код авторизации.
        /// </summary>
        /// <param name="destination">Назначение для кода авторизации.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Ответ на попытку авторизации.</returns>
        public Task<KeycloakResponse> TrySendCodeAsync(string destination, CancellationToken cancellationToken);
    }
}
