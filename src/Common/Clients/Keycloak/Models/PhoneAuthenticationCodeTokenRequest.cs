using IdentityModel.Client;

namespace InsuranceGoSmoke.Common.Clients.Keycloak.Models
{
    /// <summary>
    /// Запрос токена по телефону и авторизационному коду.
    /// </summary>
    public class PhoneAuthenticationCodeTokenRequest : TokenRequest
    {
        /// <summary>
        /// Номер телефона.
        /// </summary>
        public string PhoneNumber { get; set; } = default!;

        /// <summary>
        /// Код.
        /// </summary>
        public string? Code { get; set; }
    }
}
