using InsuranceGoSmoke.Common.Hosts.Features.AppFeatures.Logging.Enrichers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Configuration;

namespace InsuranceGoSmoke.Common.Hosts.Features.AppFeatures.Logging.Extensions
{
    /// <summary>
    /// Расширения для логирования.
    /// </summary>
    public static class LoggerExtensions
    {
        /// <summary>
        /// Логирование данных окружения.
        /// </summary>
        /// <param name="enrichmentConfiguration">Конфигурация обогащения.</param>
        /// <param name="configuration">Конфигурация.</param>
        /// <param name="environment">Окружение.</param>
        /// <returns>Конфигурация логирования.</returns>
        public static LoggerConfiguration WithEnvironment(this LoggerEnrichmentConfiguration enrichmentConfiguration,
            IConfiguration configuration, IHostEnvironment? environment)
        {
            ArgumentNullException.ThrowIfNull(enrichmentConfiguration);
            ArgumentNullException.ThrowIfNull(configuration);

            return enrichmentConfiguration.With(new EnvironmentEnricher(configuration, environment));
        }
    }
}
