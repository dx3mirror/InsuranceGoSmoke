using AspNetCoreRateLimit;
using AspNetCoreRateLimit.Redis;
using InsuranceGoSmoke.Common.Contracts.Exceptions.Feature;
using InsuranceGoSmoke.Common.Hosts.Features.AppFeatures.Base;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Net;

namespace InsuranceGoSmoke.Common.Hosts.Api.Features.AppFeatures.Throttle
{
    /// <summary>
    /// Функциональность throttling'а.
    /// </summary>
    public class ThrottleFeature : AppFeature
    {
        /// <inheritdoc/>
        public override void AddFeature(IServiceCollection services, IHostBuilder hostBuilder, ILoggingBuilder loggingBuilder)
        {
            base.AddFeature(services, hostBuilder, loggingBuilder);
            if (OptionSection == null)
            {
                return;
            }

            // Храним счетчики и IP правила в памяти.
            services.AddMemoryCache();

            // Общие правила
            var throttleOptions = OptionSection?.Get<ThrottleFeatureOptions>()
                                    ?? throw new FeatureConfigurationException("В конфигурации не удалось найти настройки троттлинга.");
            if (throttleOptions.GeneralRules.Count == 0)
            {
                throttleOptions = GetDefaultSettings();
            }
            services.Configure<IpRateLimitOptions>(options => GetIpRateLimitOptions(options, throttleOptions));

            // IP правила
            services.Configure<IpRateLimitPolicies>(options => GetGetIpRateLimitPolicies(options, throttleOptions));

            services.AddRedisRateLimiting();
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
        }

        private static ThrottleFeatureOptions GetDefaultSettings()
        {
            return new ThrottleFeatureOptions()
            {
                EnableEndpointRateLimiting = true,  // ограничение по endpoint'у, а не глобально
                StackBlockedRequests = true,        // ограничение отсчитывается от последнего успешного, заблокированные не учитываются
                GeneralRules = [
                    new()
                    {
                        Endpoint = "*",
                        Period = "1s",
                        Limit = 10
                    },
                    new()
                    {
                        Endpoint = "*",
                        Period = "15m",
                        Limit = 100
                    },
                    new()
                    {
                        Endpoint = "*",
                        Period = "12h",
                        Limit = 1000
                    },
                    new()
                    {
                        Endpoint = "*",
                        Period = "7d",
                        Limit = 10000
                    }
                ],
                IpRules = []
            };
        }

        private static void GetIpRateLimitOptions(IpRateLimitOptions options, ThrottleFeatureOptions throttleOptions)
        {
            options.EnableEndpointRateLimiting = throttleOptions.EnableEndpointRateLimiting;  
            options.StackBlockedRequests = throttleOptions.StackBlockedRequests;        
            options.RealIpHeader = "X-Real-IP";                           // заголовок из которого берется IP.
            options.HttpStatusCode = (int)HttpStatusCode.TooManyRequests; // код ошибки при превышении
            options.QuotaExceededMessage = "Превышено количество вызовов API.";
            options.GeneralRules = throttleOptions.GeneralRules;
        }

        private static void GetGetIpRateLimitPolicies(IpRateLimitPolicies policies, ThrottleFeatureOptions throttleOptions)
        {
            policies.IpRules = throttleOptions.IpRules;
        }

        /// <inheritdoc />
        public override void UseFeature(IApplicationBuilder application, IWebHostEnvironment environment)
        {
            base.UseFeature(application, environment);

            application.UseIpRateLimiting();
        }
    }
}
