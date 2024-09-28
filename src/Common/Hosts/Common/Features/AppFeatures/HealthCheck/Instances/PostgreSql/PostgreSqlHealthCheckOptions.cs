using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace InsuranceGoSmoke.Common.Hosts.Api.Features.AppFeatures.HealthCheck.Instances.PostgreSql
{
    /// <summary>
    /// Настройки проверки функционирования PostgreSql.
    /// </summary>
    internal class PostgreSqlHealthCheckOptions
    {
        /// <summary>
        /// Наименование.
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// Конфигурационная секция ConnectionsString к БД.
        /// </summary>
        public required string ConnectionStringConfigSection { get; set; }

        /// <summary>
        /// Тайм-аут подключения
        /// </summary>
        public string Timeout { get; set; } = TimeSpan.FromSeconds(3).ToString();

        /// <summary>
        /// Какой статус отдать при неудачной проверке
        /// <see cref="HealthStatus"/>
        /// </summary>
        public string FailureStatus { get; set; } = HealthStatus.Degraded.ToString();

        /// <summary>
        /// Тэги.
        /// </summary>
        public string[] Tags { get; set; } = [];
    }
}
