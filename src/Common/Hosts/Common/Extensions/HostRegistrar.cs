using InsuranceGoSmoke.Common.Hosts.Features.AppFeatures.Base;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace InsuranceGoSmoke.Common.Hosts.Extensions
{
    /// <summary>
    /// Регистрация для хостов.
    /// </summary>
    public static class HostRegistrar
    {
        /// <summary>
        /// Использование локализации.
        /// </summary>
        /// <param name="builder">Builder приложения</param>
        /// <returns>Builder приложения</returns>
        public static IApplicationBuilder UseLocalization(this IApplicationBuilder builder)
        {
            builder.UseRequestLocalization("ru-RU");

            return builder;
        }

        /// <summary>
        /// Регистрация функциональностей.
        /// </summary>
        /// <param name="services">Коллекция сервисов.</param>
        /// <param name="loggingBuilder">Builder логирования.</param>
        /// <param name="hostBuilder">Builder хоста.</param>
        /// <param name="configuration">Конфигурация.</param>
        /// <param name="featureAssemblies">Сборки, где находятся функциональности.</param>
        /// <returns>Коллекция сервисов.</returns>
        public static IServiceCollection AddFeatures(this IServiceCollection services,
            IHostBuilder hostBuilder, ILoggingBuilder loggingBuilder, IConfiguration configuration, Assembly[] featureAssemblies)
        {
            IReadOnlyCollection<IAppFeature> features = AppFeatureFactory.GetAppFeatures(configuration, featureAssemblies);
            foreach (var feature in features.OrderBy(f => f.Order))
            {
                feature.AddFeature(services, hostBuilder, loggingBuilder);
                services.AddSingleton(feature);
            }
            return services;
        }

        /// <summary>
        /// Выполняет внедрение фич в пайплайн вызова.
        /// </summary>
        public static IApplicationBuilder UseFeatures(this IApplicationBuilder app, IWebHostEnvironment environment)
        {
            var features = app.ApplicationServices.GetServices<IAppFeature>();
            foreach (var feature in features.OrderBy(f => f.Order))
            {
                feature.UseFeature(app, environment);
            }
            app.UseEndpoints(eb =>
            {
                foreach (var feature in features.OrderBy(f => f.Order))
                {
                    feature.UseEndpoints(eb);
                }
            });

            return app;
        }
    }
}
