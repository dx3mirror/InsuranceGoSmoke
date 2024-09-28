using IdentityModel.Client;
using InsuranceGoSmoke.Common.Clients.Keycloak.Models.Responses;

namespace InsuranceGoSmoke.Security.ExternalClients.Keycloak
{
    /// <summary>
    /// API клиент для работы с авторизацией через keycloak.
    /// </summary>
    public interface IKeycloakAuthorizationApiClient
    {
        /// <summary>
        /// Отправляет аутентификационный код через SMS.
        /// </summary>
        /// <param name="phoneNumber">Номер телефона.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Результат отправки.</returns>
        Task<KeycloakResponse> TrySendSmsAuthenticationCodeAsync(string phoneNumber, CancellationToken cancellationToken);

        /// <summary>
        /// Авторизация по телефону.
        /// </summary>
        /// <param name="phoneNumber">Номер телефона.</param>
        /// <param name="code">Код.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Токен доступа.</returns>
        Task<TokenResponse> AuthorizationByPhoneAsync(string phoneNumber, string code, CancellationToken cancellationToken);

        /// <summary>
        /// Обновляет токен.
        /// </summary>
        /// <param name="refreshToken">Токен обновления.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Ответ на попытку авторизации.</returns>
        Task<TokenResponse> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken);

        /// <summary>
        /// Разлогин.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        Task LogoutAsync(Guid userId, CancellationToken cancellationToken);
    }
}
