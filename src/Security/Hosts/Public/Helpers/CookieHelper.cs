namespace InsuranceGoSmoke.Security.Public.Hosts.Api.Helpers
{
    /// <summary>
    /// Helper для работы с cookies.
    /// </summary>
    public static class CookieHelper
    {
        /// <summary>
        /// Cookie refresh token'а
        /// </summary>
        public static readonly string RefreshTokenCookieName = "refresh-token";

        /// <summary>
        /// Cookie access token'а
        /// </summary>
        public static readonly string AccessTokenCookieName = "token";

        /// <summary>
        /// Устанавливает refresh token.
        /// </summary>
        /// <param name="refreshToken">Refresh token.</param>
        /// <param name="responseCookies">Ответ с установкой cookie.</param>
        /// <param name="domain">Домен.</param>
        public static void SetRefreshTokenCookie(string refreshToken, IResponseCookies responseCookies, string domain)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true, // защита от XSS
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddDays(7),
                Domain = domain
            };

            // Добавляем cookie в ответ
            responseCookies.Append(RefreshTokenCookieName, refreshToken, cookieOptions);
        }

        /// <summary>
        /// Устанавливает access token.
        /// </summary>
        /// <param name="accessToken">Refresh token.</param>
        /// <param name="responseCookies">Ответ с установкой cookie.</param>
        /// <param name="domain">Домен.</param>
        public static void SetAccessTokenCookie(string accessToken, IResponseCookies responseCookies, string domain)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = false,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddMinutes(5),
                Domain = domain
            };
            // Добавляем cookie в ответ
            responseCookies.Append(AccessTokenCookieName, accessToken, cookieOptions);
        }

        /// <summary>
        /// Очистить cookie.
        /// </summary>
        /// <param name="name">Наименование.</param>
        /// <param name="responseCookies">Cookie.</param>
        public static void ClearCookie(string name, IResponseCookies responseCookies)
        {
            var cookieOptions = new CookieOptions
            {
                Expires = DateTime.UtcNow.AddDays(-1)
            };

            // Добавляем cookie в ответ
            responseCookies.Append(name, string.Empty, cookieOptions);
        }
    }
}
