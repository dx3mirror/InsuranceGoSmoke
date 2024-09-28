using InsuranceGoSmoke.Common.Hosts.Features.AppFeatures.Base;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace InsuranceGoSmoke.Common.Hosts.Features.AppFeatures.Routing
{
    /// <summary>
    /// Функциональность роутинга.
    /// </summary>
    internal class RoutingFeature : AppFeature
    {
        /// <inheritdoc />
        public override void AddFeature(IServiceCollection services, IHostBuilder hostBuilder, ILoggingBuilder loggingBuilder)
        {
            base.AddFeature(services, hostBuilder, loggingBuilder);

            services.AddRouting();
        }

        /// <inheritdoc />
        public override void UseFeature(IApplicationBuilder application, IWebHostEnvironment environment)
        {
            base.UseFeature(application, environment);

            application.UseRouting();
        }
    }
}
