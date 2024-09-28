using Confluent.Kafka;

namespace InsuranceGoSmoke.Common.Consumers.Services.Builders
{
    /// <summary>
    /// Builder отправителей.
    /// </summary>
    public interface IKafkaProducerBuilder
    {
        /// <summary>
        /// Собирает отправителя.
        /// </summary>
        /// <returns>Отправитель.</returns>
        IProducer<Null, string> Build();
    }
}
