using IdentityModel.Client;
using InsuranceGoSmoke.Common.Clients.Keycloak.Models.Responses;

namespace InsuranceGoSmoke.Common.Clients.Keycloak
{
    /// <summary>
    /// Клиент работы с авторизацией через Keycloak.
    /// </summary>
    public interface IKeycloakAuthorizationApiExternalClient
    {
        /// <summary>
        /// Отправляет аутентификационный код через SMS.
        /// </summary>
        /// <param name="phoneNumber">Номер телефона.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Результат отправки кода.</returns>
        Task<KeycloakResponse> TrySendSmsAuthenticationCodeAsync(string phoneNumber, CancellationToken cancellationToken);

        /// <summary>
        /// Авторизация.
        /// </summary>
        /// <param name="username">Логин.</param>
        /// <param name="password">Пароль.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Ответ запроса токена.</returns>
        Task<TokenResponse> AuthorizationAsync(string username, string password, CancellationToken cancellationToken);

        /// <summary>
        /// Авторизация по телефону.
        /// </summary>
        /// <param name="phoneNumber">Номер телефона.</param>
        /// <param name="code">Код.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Ответ запроса токена.</returns>
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
