using InsuranceGoSmoke.Common.Contracts.Exceptions.Feature;
using InsuranceGoSmoke.Common.Hosts.Features.AppFeatures.Base;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenTelemetry;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace InsuranceGoSmoke.Common.Hosts.Api.Features.AppFeatures.OpenTelemetry
{
    /// <summary>
    /// Функциональность OpenTelemetry.
    /// </summary>
    internal class OpenTelemetryFeature : AppFeature
    {
        /// <inheritdoc />
        public override void AddFeature(IServiceCollection services, IHostBuilder hostBuilder, ILoggingBuilder loggingBuilder)
        {
            base.AddFeature(services, hostBuilder, loggingBuilder);
            if (OptionSection == null)
            {
                return;
            }

            var options = OptionSection.Get<OpenTelemetryFeatureOptions>();
            var serviceName = options?.ApplicationName 
                                ?? throw new FeatureConfigurationException("Не задано наименование приложения для функциональности OpenTelemetry");
            
            services.AddOpenTelemetry()
                    .ConfigureResource(resource => resource.AddService(serviceName))
                    .WithMetrics(metrics =>
                    {
                        metrics.AddAspNetCoreInstrumentation()
                                .AddMeter("System.Net.Http")
                                .AddMeter("Microsoft.AspNetCore.Hosting")
                                .AddMeter("Microsoft.AspNetCore.Server.Kestrel");

                        if (!string.IsNullOrEmpty(options?.ExporterOtlpUrl))
                        {
                            metrics.AddOtlpExporter(GetOptions(options));
                        }
                        else
                        {
                            metrics.AddConsoleExporter();
                        }
                    })
                    .WithTracing(tracing =>
                    {
                        tracing.SetSampler<AlwaysOnSampler>()
                               .AddSource(serviceName)
                               .SetResourceBuilder(
                                   ResourceBuilder.CreateDefault()
                                       .AddService(serviceName: serviceName))
                               .AddAspNetCoreInstrumentation()
                               .AddHttpClientInstrumentation()
                               .AddEntityFrameworkCoreInstrumentation(opt =>
                               {
                                   opt.SetDbStatementForText = true;
                                   opt.SetDbStatementForStoredProcedure = true;
                               })
                               .AddRedisInstrumentation();

                        if (!string.IsNullOrEmpty(options?.ExporterOtlpUrl))
                        {
                            tracing.AddOtlpExporter(GetOptions(options));
                        }
                        else
                        {
                            tracing.AddConsoleExporter();
                        }
                    });

            services.Configure<OpenTelemetryLoggerOptions>(logger => logger.AddOtlpExporter())
                    .ConfigureOpenTelemetryMeterProvider(meter => meter.AddOtlpExporter())
                    .ConfigureOpenTelemetryTracerProvider(tracer => tracer.AddOtlpExporter());

            loggingBuilder.AddOpenTelemetry(configure =>
            {
                configure.IncludeScopes = true;
                configure.IncludeFormattedMessage = true;
                configure.SetResourceBuilder(
                    ResourceBuilder.CreateDefault()
                                    .AddService(serviceName));

                if (!string.IsNullOrEmpty(options?.ExporterOtlpUrl))
                {
                    configure.AddOtlpExporter(GetOptions(options));
                }
                else
                {
                    configure.AddConsoleExporter();
                }
            });
        }

        private static Action<global::OpenTelemetry.Exporter.OtlpExporterOptions> GetOptions(OpenTelemetryFeatureOptions options)
        {
            return o =>
            {
                o.Endpoint = new Uri(options.ExporterOtlpUrl);
            };
        }
    }
}
