using Microsoft.AspNetCore.Authentication;

namespace InsuranceGoSmoke.Common.Clients.Keycloak
{
    /// <summary>
    /// Настройки авторизации через Keycloak.
    /// </summary>
    public class KeycloakAuthorizationOptions : AuthenticationSchemeOptions
    {
        /// <summary>
        /// Название схемы.
        /// </summary>
        public static readonly string Scheme = "Keycloak";

        /// <summary>
        /// Realm.
        /// </summary>
        public string Realm { get; set; } = string.Empty;

        /// <summary>
        /// URL аутентификации.
        /// </summary>
        public string LoginUrl { get; set; } = string.Empty;

        /// <summary>
        /// URL.
        /// </summary>
        public string Authority { get; set; } = string.Empty;

        /// <summary>
        /// Идентификатор клиента.
        /// </summary>
        public string ClientId { get; set; } = string.Empty;

        /// <summary>
        /// GUID клиента.
        /// </summary>
        public string ClientUuid { get; set; } = string.Empty;

        /// <summary>
        /// Секрет клиента.
        /// </summary>
        public string ClientSecret { get; set; } = string.Empty;

        /// <summary>
        /// Признак, что требуется https.
        /// </summary>
        public bool RequireHttpsMetadata { get; set; }

        /// <summary>
        /// Адрес с метаданными.
        /// </summary>
        public string MetadataAddress { get; set; } = string.Empty;

        /// <summary>
        /// URL админки API.
        /// </summary>
        public string ApiAdminBaseUrl { get; set; } = string.Empty;

        /// <summary>
        /// Идентификатор клиент администратора API.
        /// </summary>
        public string ApiClientId { get; set; } = string.Empty;

        /// <summary>
        /// Секрет клиента администратора API.
        /// </summary>
        public string ApiClientSecret { get; set; } = string.Empty;
    }
}
