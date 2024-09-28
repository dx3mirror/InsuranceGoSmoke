using InsuranceGoSmoke.Common.Hosts.Api.Features.AppFeatures.HealthCheck.Instances.PostgreSql;
using InsuranceGoSmoke.Common.Hosts.Features.AppFeatures.HealthCheck.Base;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Globalization;

namespace InsuranceGoSmoke.Common.Hosts.Features.AppFeatures.HealthCheck.Instances.PostgreSql
{
    /// <summary>
    /// Конфигуратор проверок работоспособности PostgreSql базы.
    /// </summary>
    internal class PostgreSqlHealthCheckConfigurator : IHealthCheckConfigurator
    {
        private const string OptionsSectionName = "Options";

        /// <inheritdoc/>
        public void Configure(IServiceCollection services, IConfiguration configuration, IConfigurationSection optionsSection, IHealthChecksBuilder checksBuilder)
        {
            var options = optionsSection.GetSection(OptionsSectionName).Get<PostgreSqlHealthCheckOptions>();
            if (options == null)
            {
                return;
            }

            services.AddSingleton(options);
            if (!Enum.TryParse<HealthStatus>(options.FailureStatus, out var failureStatus))
            {
                throw new HealthCheckConfigurationException($"Не удалось получить статус из {options.FailureStatus}");
            }

            if (!TimeSpan.TryParse(options.Timeout, CultureInfo.CurrentCulture, out var timeout))
            {
                throw new HealthCheckConfigurationException($"Не удалось получить Timeout из {options.Timeout}");
            }

            var connectionString = configuration.GetConnectionString(options.ConnectionStringConfigSection);
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new HealthCheckConfigurationException($"Не удалось получить connectionString из секции '{options.ConnectionStringConfigSection}' для проверки БД.");
            }
            checksBuilder.AddNpgSql(connectionString, name: options.Name, failureStatus: failureStatus, tags: options.Tags, timeout: timeout);
        }
    }
}
