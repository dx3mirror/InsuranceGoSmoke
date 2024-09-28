using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace InsuranceGoSmoke.Common.Hosts.Features.AppFeatures.HealthCheck.Base
{
    /// <summary>
    /// Настройки для проверки работоспособности
    /// </summary>
    public interface IHealthCheckOptions
    {
        /// <summary>
        /// Наименование.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Какой статус отдать при неудачном хелф-чеке: Degraded, Healthy, Unhealthy
        /// <see cref="HealthStatus"/>
        /// </summary>
        public string FailureStatus { get; set; }

        /// <summary>
        /// Какие теги прикрепить к хелф-чеку
        /// </summary>
        public string[] Tags { get; set; }
    }
}
