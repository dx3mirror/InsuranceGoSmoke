using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

namespace InsuranceGoSmoke.Common.Hosts.Features.AppFeatures.Base
{
    /// <summary>
    /// Интерфейс функциональности.
    /// </summary>
    public interface IAppFeature
    {
        /// <summary>
        /// Порядок применения функциональности.
        /// </summary>
        int Order { get; }

        /// <summary>
        /// Инициализирует.
        /// </summary>
        /// <param name="appFeatureInitRequest"></param>
        void Init(AppFeatureInitRequest appFeatureInitRequest);

        /// <summary>
        /// Добавляет функциональность.
        /// </summary>
        /// <param name="services">Коллекция сервисов.</param>
        void AddFeature(IServiceCollection services);

        /// <summary>
        /// Добавляет функциональность.
        /// </summary>
        /// <param name="services">Коллекция сервисов.</param>
        /// <param name="hostBuilder">Builder хоста.</param>
        /// <param name="loggingBuilder">Builder логирования.</param>
        void AddFeature(IServiceCollection services, IHostBuilder hostBuilder, ILoggingBuilder loggingBuilder);

        /// <summary>
        /// Добавляет функциональность.
        /// </summary>
        /// <param name="services">Коллекция сервисов.</param>
        /// <param name="hostBuilder">Builder хоста.</param>
        /// <param name="loggingBuilder">Builder логирования.</param>
        void AddFeature(IServiceCollection services, IHostApplicationBuilder hostBuilder, ILoggingBuilder loggingBuilder);

        /// <summary>
        /// Добавляет endpoint'ы.
        /// </summary>
        /// <param name="routeBuilder">Билдер endpoint'ов</param>
        void UseEndpoints(IEndpointRouteBuilder routeBuilder);

        /// <summary>
        /// Добавляет использование функциональности.
        /// </summary>
        /// <param name="application">Билдер приложения.</param>
        /// <param name="environment">Окружение.</param>
        void UseFeature(IApplicationBuilder application, IWebHostEnvironment environment);
    }
}
