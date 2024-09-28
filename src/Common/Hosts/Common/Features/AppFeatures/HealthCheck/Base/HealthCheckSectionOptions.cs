using Microsoft.Extensions.Configuration;

namespace InsuranceGoSmoke.Common.Hosts.Features.AppFeatures.HealthCheck.Base
{
    /// <summary>
    /// Настройки секции проверки работоспособности.
    /// </summary>
    public class HealthCheckSectionOptions
    {
        /// <summary>
        /// Наименование.
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// Признак, что проверка функциональности выключена.
        /// </summary>
        public bool Disabled { get; set; }
    }
}
