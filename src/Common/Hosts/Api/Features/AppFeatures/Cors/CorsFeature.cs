using InsuranceGoSmoke.Common.Contracts.Exceptions.Feature;
using InsuranceGoSmoke.Common.Hosts.Features.AppFeatures.Base;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace InsuranceGoSmoke.Common.Hosts.Api.Features.AppFeatures.Cors
{
    /// <summary>
    /// Функциональность для работы CORS.
    /// </summary>
    internal class CorsFeature : AppFeature
    {
        private readonly static string CorsPolicyName = "KortrosCorsPolicy";

        /// <inheritdoc />
        public override void AddFeature(IServiceCollection services, IHostBuilder hostBuilder, ILoggingBuilder loggingBuilder)
        {
            base.AddFeature(services, hostBuilder, loggingBuilder);

            var corsOptions = OptionSection?.Get<CorsFeatureOptions>()
                            ?? throw new FeatureConfigurationException("В конфигурации не удалось найти настройки CORS.");
            services.AddCors(options =>
            {
                options.AddPolicy(CorsPolicyName,
                    builder => builder
                        .WithOrigins(corsOptions.AllowedOrigins)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
            });
        }


        /// <inheritdoc />
        public override void UseFeature(IApplicationBuilder application, IWebHostEnvironment environment)
        {
            base.UseFeature(application, environment);

            application.UseCors(CorsPolicyName);
        }
    }
}
