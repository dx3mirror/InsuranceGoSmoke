using InsuranceGoSmoke.Security.Contracts.Identify.Responses;

namespace InsuranceGoSmoke.Security.Applications.AppServices.Contexts.Identify.Services.Abstract
{
    /// <summary>
    /// Сервис работы с авторизацией.
    /// </summary>
    public interface IAuthorizationService
    {
        /// <summary>
        /// Авторизует.
        /// </summary>
        /// <param name="userName">Логин.</param>
        /// <param name="password">Пароль.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Ответ на попытку авторизации.</returns>
        Task<LoginResponse> AuthorizationAsync(string userName, string password, CancellationToken cancellationToken);

        /// <summary>
        /// Разлогин.
        /// </summary>
        /// <param name="cancellationToken">Токен отмены.</param>
        Task LogoutAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Обновляет токен.
        /// </summary>
        /// <param name="refreshToken">Токен обновления.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Ответ на попытку авторизации.</returns>
        Task<LoginResponse> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken);

        /// <summary>
        /// Отправляет аутентификационный код для дальнейшей авторизации.
        /// </summary>
        /// <param name="destination">Назначение для кода.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        Task SendAuthentificationCodeAsync(string destination, CancellationToken cancellationToken);
    }
}
