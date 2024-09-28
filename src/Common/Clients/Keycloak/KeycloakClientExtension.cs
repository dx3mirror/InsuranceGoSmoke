using IdentityModel.Client;
using InsuranceGoSmoke.Common.Clients.Keycloak.Generated;
using InsuranceGoSmoke.Common.Contracts.Exceptions.Feature;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace InsuranceGoSmoke.Common.Clients.Keycloak
{
    /// <summary>
    /// Расширение для клиента Keycloak.
    /// </summary>
    public static class KeycloakClientExtension
    {
        /// <summary>
        /// Регистрирует клиент keycloak.
        /// </summary>
        /// <param name="services">Коллекция сервисов.</param>
        /// <param name="configuration">Конфигурация.</param>
        /// <returns>Коллекция сервисов.</returns>
        public static IServiceCollection AddKeycloakExternalApiClient(this IServiceCollection services, IConfiguration configuration)
        {
            var optionsSection = configuration.GetSection("Features:Authorization:Sections")
                                              ?.GetChildren()
                                              ?.FirstOrDefault(c => c.GetSection(KeycloakAuthorizationOptions.Scheme).Exists())
                                                ?? throw new FeatureConfigurationException("Не удалось найти секцию с настройками Keycloak");

            var config = new KeycloakAuthorizationOptions();
            optionsSection.Bind(KeycloakAuthorizationOptions.Scheme, config);
            services.AddAccessTokenManagement(options =>
            {
                options.Client.CacheLifetimeBuffer = 0;
                options.Client.Clients.Add(KeycloakAuthorizationOptions.Scheme, CreateClientCredentialsTokenRequest(config));
            });

            services.AddHttpClient<IKeycloakGeneratedExternalApiClient, KeycloakGeneratedExternalApiClient>(
                              client => client.BaseAddress = new Uri(config.ApiAdminBaseUrl 
                                                                    ?? throw new FeatureConfigurationException("Не удалось найти настройку ApiAdminBaseUrl")))
                    .AddClientAccessTokenHandler(KeycloakAuthorizationOptions.Scheme);

            services.AddScoped<IKeycloakAuthorizationApiExternalClient, KeycloakAuthorizationApiExternalClient>();
            services.AddScoped<IKeycloakUserApiExternalClient, KeycloakUserApiExternalClient>();

            return services;
        }

        /// <summary>
        /// Создаёт запрос на токен.
        /// </summary>
        /// <param name="options">Настройки.</param>
        /// <returns>Запрос токена.</returns>
        public static ClientCredentialsTokenRequest CreateClientCredentialsTokenRequest(KeycloakAuthorizationOptions options)
        {
            return new ClientCredentialsTokenRequest
            {
                Address = $"{options.ApiAdminBaseUrl}/realms/master/protocol/openid-connect/token",
                ClientId = options.ApiClientId,
                ClientSecret = options.ApiClientSecret,
                ClientCredentialStyle = ClientCredentialStyle.PostBody
            };
        }
    }
}
