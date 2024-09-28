namespace InsuranceGoSmoke.Security.Contracts.Identify.Responses
{
    /// <summary>
    /// Ответ на попытку авторизации.
    /// </summary>
    public class LoginResponse
    {
        /// <summary>
        /// Создаёт сущность <see cref="LoginResponse" />
        /// </summary>
        /// <param name="accessToken">Токен доступа.</param>
        /// <param name="refreshToken">Токен обновления.</param>
        public LoginResponse(string accessToken, string refreshToken)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
        }

        /// <summary>
        /// Токен доступа.
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// Токен обновления.
        /// </summary>
        public string RefreshToken { get; set; }
    }
}
