using IdentityModel.Client;

namespace InsuranceGoSmoke.Security.Applications.AppServices.Contexts.Identify.Factories.Strategies
{
    /// <summary>
    /// Интерфейс стратегии идентификации.
    /// </summary>
    public interface IIdentifyStrategy
    {
        /// <summary>
        /// Авторизует.
        /// </summary>
        /// <param name="username">Логин.</param>
        /// <param name="password">Пароль.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Ответ на попытку авторизации.</returns>
        Task<TokenResponse> AuthorizationAsync(string username, string password, CancellationToken cancellationToken);
    }
}
