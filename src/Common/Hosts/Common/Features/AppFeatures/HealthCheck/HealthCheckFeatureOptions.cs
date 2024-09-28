using Microsoft.Extensions.Configuration;

namespace InsuranceGoSmoke.Common.Hosts.Features.AppFeatures.HealthCheck
{
    /// <summary>
    /// Настройки функциональности проверки на работоспособность сервиса.
    /// </summary>
    public class HealthCheckFeatureOptions
    {
        /// <summary>
        /// Секции с настройками.
        /// </summary>
        public IReadOnlyCollection<IConfigurationSection> Sections { get; set; } = [];
    }
}
