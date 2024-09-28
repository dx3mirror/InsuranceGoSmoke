using InsuranceGoSmoke.Common.Hosts.Features.AppFeatures.Base;
using InsuranceGoSmoke.Common.Hosts.Features.AppFeatures.HealthCheck.Base;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace InsuranceGoSmoke.Common.Hosts.Features.AppFeatures.HealthCheck
{
    /// <summary>
    /// Функциональность для проверки работоспособности сервиса.
    /// </summary>
    public class HealthCheckFeature : AppFeature
    {
        /// <summary>
        /// Тэги.
        /// </summary>
        public struct Tags
        {
            /// <summary>
            /// Тэг готовности
            /// </summary>
            public const string ReadinessTag = "readiness";

            /// <summary>
            /// Тэг жизнеспособности
            /// </summary>
            public const string LivenessTag = "liveness";

            /// <summary>
            /// По крайней мере один тег
            /// </summary>
            public const string AtLeastOneTag = "atLeastOne";
        }

        /// <summary>
        /// URL проверки.
        /// </summary>
        public static readonly string HealthUrl = "/health/";

        private static readonly JsonSerializerOptions _options = new()
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        /// <inheritdoc/>
        public override void AddFeature(IServiceCollection services)
        {
            base.AddFeature(services);

            if (OptionSection == null)
            {
                return;
            }

            services.AddHttpClient();

            var configuratorTypes = AdditionalAssemblies
                                        .SelectMany(a => a.GetTypes()
                                                          .Where(t => t.GetInterfaces()
                                                                       .Contains(typeof(IHealthCheckConfigurator))));

            IHealthChecksBuilder? healthCheckBuilder = null;

            var options = OptionSection.Get<HealthCheckFeatureOptions>();
            var sections = options?.Sections ?? [];
            foreach (var section in sections)
            {
                var healthCheckOption = section.Get<HealthCheckSectionOptions>();
                if (healthCheckOption == null)
                {
                    continue;
                }

                if (healthCheckOption.Disabled)
                {
                    continue;
                }

                var configurationName = $"{healthCheckOption.Name}HealthCheckConfigurator";
                var configuratorType = configuratorTypes.FirstOrDefault(c => c.Name == configurationName);
                if (configuratorType == null)
                {
                    continue;
                }

                healthCheckBuilder ??= services.AddHealthChecks();
                var configurator = Activator.CreateInstance(configuratorType) as IHealthCheckConfigurator;
                configurator?.Configure(services, Configuration!, section, healthCheckBuilder!);
            }
        }

        /// <inheritdoc/>
        public override void AddFeature(IServiceCollection services, IHostBuilder hostBuilder, ILoggingBuilder loggingBuilder)
        {
            this.AddFeature(services);
        }

        /// <inheritdoc/>
        public override void AddFeature(IServiceCollection services, IHostApplicationBuilder hostBuilder, ILoggingBuilder loggingBuilder)
        { 
            this.AddFeature(services);
        }

        /// <inheritdoc/>
        public override void UseEndpoints(IEndpointRouteBuilder routeBuilder)
        {
            base.UseEndpoints(routeBuilder);

            if (OptionSection == null)
            {
                return;
            }

            var configuratorTypes = AdditionalAssemblies
                                        .SelectMany(a => a.GetTypes()
                                                          .Where(t => t.GetInterfaces()
                                                                       .Contains(typeof(IHealthCheckMapConfigurator))));
            MapHealthCheckService(routeBuilder);

            var options = OptionSection.Get<HealthCheckFeatureOptions>();
            var sections = options?.Sections ?? [];
            foreach (var section in sections)
            {
                var healthCheckOption = section.Get<HealthCheckSectionOptions>();
                if (healthCheckOption == null)
                {
                    continue;
                }

                if (healthCheckOption.Disabled)
                {
                    continue;
                }


                var configurationName = $"{healthCheckOption.Name}HealthCheckConfigurator";
                var configuratorType = configuratorTypes.FirstOrDefault(c => c.Name == configurationName);
                if (configuratorType == null)
                {
                    continue;
                }

                var configurator = Activator.CreateInstance(configuratorType) as IHealthCheckMapConfigurator;
                configurator?.MapHealthCheckService(routeBuilder);
            }
        }

        private static void MapHealthCheckService(IEndpointRouteBuilder routeBuilder)
        {
            MapHealthCheck(routeBuilder, Tags.LivenessTag);
            MapHealthCheck(routeBuilder, Tags.ReadinessTag);
        }

        private static void MapHealthCheck(IEndpointRouteBuilder routeBuilder, string tag)
        {
            var options = new HealthCheckOptions
            {
                ResponseWriter = WriteResponse,
                Predicate = PredicateByTag(tag),
                AllowCachingResponses = false
            };
            routeBuilder.MapHealthChecks(HealthUrl + tag, options);
        }

        /// <summary>
        /// Возвращает метод проверки тэга.
        /// </summary>
        /// <param name="tag">Тэг</param>
        /// <returns>Метод проверки тэга.</returns>
        protected static Func<HealthCheckRegistration, bool> PredicateByTag(string tag)
        {
            return registration =>
            {
                return registration.Tags.Any(s => s == tag);
            };
        }

        /// <summary>
        /// Формирование ответа.
        /// </summary>
        /// <param name="context">Контекст.</param>
        /// <param name="result">Результат.</param>
        protected static Task WriteResponse(HttpContext context, HealthReport result)
        {
            context.Response.ContentType = "application/json; charset=utf-8";
            context.Response.StatusCode = GetHttpStatusConsiderByAtLeastOneTag(result);

            var data = new
            {
                Status = GetHealthStatusConsiderByAtLeastOneTag(result).ToString(),
                Dependencies = result.Entries.Select(e => new
                {
                    Name = e.Key,
                    e.Value.Data,
                    Description = e.Value.Description ?? e.Value.Exception?.Message,
                    Status = e.Value.Status.ToString()
                }).ToArray()
            };
            return context.Response.WriteAsync(JsonSerializer.Serialize(data, _options));
        }

        /// <summary>
        /// Получить HttpStatusCode с учетом тэга AtLeastOne
        /// </summary>
        /// <returns>HttpStatusCode</returns>
        private static int GetHttpStatusConsiderByAtLeastOneTag(HealthReport report)
        {
            return GetHealthStatusConsiderByAtLeastOneTag(report) switch
            {
                HealthStatus.Healthy => StatusCodes.Status200OK,
                HealthStatus.Degraded => StatusCodes.Status200OK,
                _ => StatusCodes.Status503ServiceUnavailable
            };
        }

        /// <summary>
        /// Получить HealthStatus с учетом тэга AtLeastOne
        /// </summary>
        /// <returns>HealthStatus</returns>
        private static HealthStatus GetHealthStatusConsiderByAtLeastOneTag(HealthReport report)
        {
            var entriesWithAtLeastOneTags =
                report.Entries
                        .Where(report => report.Value.Tags.Contains(Tags.AtLeastOneTag))
                        .GroupBy(report => report.Key[..(report.Key.Contains('[') ? report.Key.IndexOf('[') : report.Key.Length)]);

            var atLeastOneNoUnhealthy = entriesWithAtLeastOneTags.All(group =>
                                            group.Any(entry => !entry.Value.Status.Equals(HealthStatus.Unhealthy)));

            if (!entriesWithAtLeastOneTags.Any() || !atLeastOneNoUnhealthy)
            {
                return report.Status;
            }

            return entriesWithAtLeastOneTags.Any(group => group
                .Any(entry => entry.Value.Status.Equals(HealthStatus.Degraded)))
                ? HealthStatus.Degraded
                : HealthStatus.Healthy;
        }
    }
}
