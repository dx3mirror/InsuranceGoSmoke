using Confluent.Kafka;
using InsuranceGoSmoke.Common.Consumers.Options;
using Microsoft.Extensions.Options;

namespace InsuranceGoSmoke.Common.Consumers.Services.Builders
{
    /// <inheritdoc/>
    public class KafkaProducerBuilder : IKafkaProducerBuilder
    {
        private readonly KafkaOptions _kafkaOptions;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="options">Настройки.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public KafkaProducerBuilder(IOptions<KafkaOptions> options)
        {
            _kafkaOptions = options.Value;
        }

        /// <inheritdoc/>
        public IProducer<Null, string> Build()
        {
            var config = new ClientConfig
            {
                BootstrapServers = _kafkaOptions.BootstrapServers
            };

            var producerBuilder = new ProducerBuilder<Null, string>(config);

            return producerBuilder.Build();
        }
    }
}
