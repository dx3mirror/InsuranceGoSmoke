using InsuranceGoSmoke.Common.Hosts.Features.AppFeatures.Base;
using InsuranceGoSmoke.Common.Hosts.Features.AppFeatures.Logging.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Settings.Configuration;

namespace InsuranceGoSmoke.Common.Hosts.Api.Features.AppFeatures.Logging
{
    /// <summary>
    /// Функциональность логирования.
    /// </summary>
    internal class LoggingFeature : AppFeature
    {
        /// <inheritdoc />
        public override void AddFeature(IServiceCollection services, IHostBuilder hostBuilder, ILoggingBuilder loggingBuilder)
        {
            base.AddFeature(services, hostBuilder, loggingBuilder);

            hostBuilder.UseSerilog(Configure);
        }

        internal static void Configure(HostBuilderContext host, LoggerConfiguration config)
        {
            var options = new ConfigurationReaderOptions { SectionName = "Features:Logging:Serilog" };
            var confifuration = config.ReadFrom.Configuration(host.Configuration, options);
            confifuration.Enrich.WithEnvironment(host.Configuration, host.HostingEnvironment);
        }
    }
}
