using InsuranceGoSmoke.Common.Contracts.Options;

namespace InsuranceGoSmoke.Common.Consumers.Options
{
    /// <summary>
    /// Настройки для Kafka.
    /// </summary>
    [ConfigurationOptions("Kafka")]
    public class KafkaOptions
    {
        /// <summary>
        /// Сервера.
        /// </summary>
        public string BootstrapServers { get; set; } = string.Empty;

        /// <summary>
        /// Идентификатор группы.
        /// </summary>
        public string ConsumerGroupId { get; set; } = string.Empty;
    }
}
