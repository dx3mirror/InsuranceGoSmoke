namespace InsuranceGoSmoke.Common.Clients.Keycloak.Models
{
    /// <summary>
    /// Константы параметров.
    /// </summary>
    public static class OidcConstants
    {
        /// <summary>
        /// Константы запроса токена по авторизационному коду через телефон.
        /// </summary>
        public static class PhoneAuthenticationCodeTokenRequest
        {
            /// <summary>
            /// Номер телефона.
            /// </summary>
            public const string PhoneNumber = "phone_number";
        }
    }
}
