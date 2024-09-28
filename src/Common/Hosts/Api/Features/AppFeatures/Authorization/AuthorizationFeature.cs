using InsuranceGoSmoke.Common.Hosts.Api.Features.AppFeatures.Authorization.Instances.Keycloak;
using InsuranceGoSmoke.Common.Hosts.Features.AppFeatures.Authorization.Instances.TrustedNetwork;
using InsuranceGoSmoke.Common.Hosts.Features.AppFeatures.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using InsuranceGoSmoke.Common.Contracts.Options;
using InsuranceGoSmoke.Common.Clients.Keycloak;
using InsuranceGoSmoke.Common.Contracts.Exceptions.Feature;

namespace InsuranceGoSmoke.Common.Hosts.Api.Features.AppFeatures.Authorization
{
    /// <summary>
    /// Функциональность авторизации.
    /// </summary>
    internal class AuthorizationFeature : AppFeature
    {
        /// <inheritdoc />
        public override void AddFeature(IServiceCollection services, IHostBuilder hostBuilder, ILoggingBuilder loggingBuilder)
        {
            base.AddFeature(services, hostBuilder, loggingBuilder);
            
            RegisterAuthorizationServices(services, OptionSection);

            services.AddAuthorizationBuilder()
                    .SetDefaultPolicy(new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build());
        }

        private static void RegisterAuthorizationServices(IServiceCollection services, IConfigurationSection? optionSection)
        {
            if (optionSection == null)
            {
                return;
            }

            var options = optionSection.Get<AuthorizationFeatureOptions>()
                            ?? throw new FeatureConfigurationException("В конфигурации не удалось найти настройки авторизации.");
            var sections = options.Sections ?? [];
            foreach (var section in sections)
            {
                RegisterAuthorizationService(services, section);
            }
        }

        private static void RegisterAuthorizationService(IServiceCollection services, IConfigurationSection optionSection)
        {
            var trustedNetworkOptions = optionSection.GetSection(TrustedNetworkOptions.Scheme).Get<TrustedNetworkOptions>();
            if (trustedNetworkOptions?.TrustedNetworks != null && trustedNetworkOptions.TrustedNetworks.Length > 0)
            {
                services.AddTrustedNetworkAuthenticate(optionSection);
            }

            var keycloakAuthorizationOptions = optionSection.GetSection(KeycloakAuthorizationOptions.Scheme).Get<KeycloakAuthorizationOptions>();
            if (!string.IsNullOrEmpty(keycloakAuthorizationOptions?.Authority))
            {
                services.AddKeycloakAuthenticate(optionSection);
            }
        }

        /// <inheritdoc />
        public override void UseFeature(IApplicationBuilder application, IWebHostEnvironment environment)
        {
            base.UseFeature(application, environment);

            application.UseAuthentication();
            application.UseAuthorization();
        }
    }
}
