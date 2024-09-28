using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace InsuranceGoSmoke.Common.Hosts.Features.AppFeatures.HealthCheck.Base
{
    /// <summary>
    /// Конфигуратор проверки работоспособности.
    /// </summary>
    public interface IHealthCheckConfigurator
    {
        /// <summary>
        /// Настраивает проверку.
        /// </summary>
        /// <param name="services">Коллекция сервисов.</param>
        /// <param name="configuration">Конфигурация.</param>
        /// <param name="optionsSection">Конфигурация настроек.</param>
        /// <param name="checksBuilder">Builder проверки.</param>
        public void Configure(IServiceCollection services, IConfiguration configuration, IConfigurationSection optionsSection, IHealthChecksBuilder checksBuilder);
    }
}
