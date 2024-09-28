using InsuranceGoSmoke.Common.Hosts.Features.AppFeatures.HealthCheck.Base;
using InsuranceGoSmoke.Common.Hosts.Features.AppFeatures.HealthCheck.Instances.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace InsuranceGoSmoke.Common.Hosts.Api.Features.AppFeatures.HealthCheck.Instances.Api
{
    /// <summary>
    /// Проверка на работоспособность для API.
    /// </summary>
    internal class HttpHealthCheckConfigurator : IHealthCheckConfigurator, IHealthCheckMapConfigurator
    {
        private const string OptionsSectionName = "Options";

        /// <inheritdoc/>
        public void Configure(IServiceCollection services, IConfiguration configuration, IConfigurationSection optionsSection, IHealthChecksBuilder checksBuilder)
        {
            var options = optionsSection.GetSection(OptionsSectionName).Get<HttpHealthCheckOptions>();
            if (options == null)
            {
                return;
            }

            services.AddSingleton(options);
            if (!Enum.TryParse<HealthStatus>(options.FailureStatus, out var failureStatus))
            {
                throw new HealthCheckConfigurationException($"Не удалось получить статус из {options.FailureStatus}");
            }

            checksBuilder.AddCheck<HttpHealthCheck>(options.Name, failureStatus, options.Tags);
        }

        /// <inheritdoc/>
        public void MapHealthCheckService(IEndpointRouteBuilder routeBuilder)
        {
        }
    }
}
