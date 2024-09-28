using InsuranceGoSmoke.Common.Contracts.Exceptions.Feature;
using InsuranceGoSmoke.Common.Hosts.Features.AppFeatures.Base;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace InsuranceGoSmoke.Common.Hosts.Features.AppFeatures.Redis
{
    /// <summary>
    /// Функциональность для работы с распределенным кэшем.
    /// </summary>
    internal class RedisFeature : AppFeature
    {
        /// <inheritdoc />
        public override void AddFeature(IServiceCollection services, IHostBuilder hostBuilder, ILoggingBuilder loggingBuilder)
        {
            base.AddFeature(services, hostBuilder, loggingBuilder);
            if (OptionSection == null)
            {
                return;
            }

            var options = OptionSection.Get<RedisFeatureOptions>() 
                            ?? throw new FeatureConfigurationException("Настройки подключения для Redis не найдены.");
            services.AddSingleton<IConnectionMultiplexer>(sp =>
            {
                if (string.IsNullOrEmpty(options.ConnectionString))
                {
                    throw new FeatureConfigurationException("Строка подключения для Redis пуста.");
                }

                return ConnectionMultiplexer.Connect(options.ConnectionString!);
            });

            services.AddStackExchangeRedisCache(o =>
            {
                o.Configuration = OptionSection["ConnectionString"];
                o.InstanceName = options.ApplicationName;
            });
        }
    }
}
