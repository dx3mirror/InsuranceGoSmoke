using Confluent.Kafka;

namespace InsuranceGoSmoke.Common.Consumers.Services.Builders
{
    /// <summary>
    /// Сборщик consumer'а.
    /// </summary>
    public interface IKafkaConsumerBuilder
    {
        /// <summary>
        /// Собирает consumer.
        /// </summary>
        /// <returns></returns>
        IConsumer<string, string> Build();
    }
}
