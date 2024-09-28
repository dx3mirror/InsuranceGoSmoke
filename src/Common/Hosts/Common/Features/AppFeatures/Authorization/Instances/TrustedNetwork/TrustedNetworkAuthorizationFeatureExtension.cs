using InsuranceGoSmoke.Common.Contracts.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace InsuranceGoSmoke.Common.Hosts.Features.AppFeatures.Authorization.Instances.TrustedNetwork
{
    /// <summary>
    /// Расширение для функциональности авторизации через доверенные сети.
    /// </summary>
    public static class TrustedNetworkAuthorizationFeatureExtension
    {
        /// <summary>
        /// Регистрирует компоненты для аутентификации через доверенные сети.
        /// </summary>
        /// <param name="services">Коллекция сервисов.</param>
        /// <param name="optionSection">Конфигурация.</param>
        /// <returns>Коллекция сервисов.</returns>
        public static IServiceCollection AddTrustedNetworkAuthenticate(this IServiceCollection services, IConfigurationSection optionSection)
        {
            var builder = services.AddAuthentication(TrustedNetworkOptions.Scheme);

            builder.Services.AddScoped<ITrustedNetworkValidator, TrustedNetworkValidator>();

            builder.AddScheme<TrustedNetworkOptions, TrustedNetworkAuthorizationHandler>(
                        TrustedNetworkOptions.Scheme, o =>
                        {
                            optionSection.Bind(TrustedNetworkOptions.Scheme, o);
                        });

            services.AddOptions<TrustedNetworkOptions>()
                    .Configure(o =>
                    {
                        optionSection.Bind(TrustedNetworkOptions.Scheme, o);
                    });

            return services;
        }
    }
}
