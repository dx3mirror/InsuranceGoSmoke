using InsuranceGoSmoke.Common.Contracts.Abstract;
using InsuranceGoSmoke.Security.Contracts.Identify.Responses;

namespace InsuranceGoSmoke.Security.Applications.Handlers.Contexts.Identify.Queries.RefreshToken
{
    /// <summary>
    /// Запрос на обновление токена.
    /// </summary>
    public class RefreshTokenQuery : Query<LoginResponse>
    {
        /// <summary>
        /// Создаёт экземпляр <see cref="RefreshTokenQuery"/>
        /// </summary>
        /// <param name="refreshToken">Токен обновления.</param>
        public RefreshTokenQuery(string? refreshToken)
        {
            RefreshToken = refreshToken;
        }

        /// <summary>
        /// Токен обновления.
        /// </summary>
        public string? RefreshToken { get; }
    }
}
