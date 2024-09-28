using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace InsuranceGoSmoke.Common.Hosts.Features.AppFeatures.Base
{
    /// <inheritdoc/>
    public abstract class AppFeature : IAppFeature
    {
        /// <inheritdoc/>
        public int Order { get; private set; }

        /// <summary>
        /// Настройки.
        /// </summary>
        public IConfiguration? Configuration { get; private set; }

        /// <summary>
        /// Секция настроек функциональности.
        /// </summary>
        public IConfigurationSection? OptionSection { get; private set; }

        /// <summary>
        /// Дополнительные сборки.
        /// </summary>
        public Assembly[] AdditionalAssemblies { get; private set; } = [];

        /// <inheritdoc/>
        public virtual void Init(AppFeatureInitRequest appFeatureInitRequest)
        {
            Order = appFeatureInitRequest.Order;
            Configuration = appFeatureInitRequest.Configuration;
            OptionSection = appFeatureInitRequest.OptionSection;
            AdditionalAssemblies = appFeatureInitRequest.AdditionalAssemblies;
        }

        /// <inheritdoc/>
        public virtual void AddFeature(IServiceCollection services)
        {
        }

        /// <inheritdoc/>
        public virtual void AddFeature(IServiceCollection services, IHostBuilder hostBuilder, ILoggingBuilder loggingBuilder)
        {
            AddFeature(services);
        }

        /// <inheritdoc/>
        public virtual void AddFeature(IServiceCollection services, IHostApplicationBuilder hostBuilder, ILoggingBuilder loggingBuilder)
        {
            AddFeature(services);
        }

        /// <inheritdoc/>
        public virtual void UseEndpoints(IEndpointRouteBuilder routeBuilder)
        {
        }

        /// <inheritdoc/>
        public virtual void UseFeature(IApplicationBuilder application, IWebHostEnvironment environment)
        {
        }
    }
}
