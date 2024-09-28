using Confluent.Kafka;
using InsuranceGoSmoke.Common.Consumers.Options;
using Microsoft.Extensions.Options;

namespace InsuranceGoSmoke.Common.Consumers.Services.Builders
{
    /// <inheritdoc/>
    public class KafkaConsumerBuilder : IKafkaConsumerBuilder
    {
        private readonly KafkaOptions _kafkaOptions;

        /// <summary>
        /// Создаёт экземпляр <see cref="KafkaConsumerBuilder"/>
        /// </summary>
        /// <param name="kafkaOptions">Настройки.</param>
        public KafkaConsumerBuilder(IOptions<KafkaOptions> kafkaOptions)
        {
            _kafkaOptions = kafkaOptions.Value;
        }

        /// <inheritdoc/>
        public IConsumer<string, string> Build()
        {
            var config = new ClientConfig
            {
                BootstrapServers = _kafkaOptions.BootstrapServers,
                AllowAutoCreateTopics = true
            };

            var consumerConfig = new ConsumerConfig(config)
            {
                GroupId = _kafkaOptions.ConsumerGroupId,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                AllowAutoCreateTopics = true
            };

            var consumerBuilder = new ConsumerBuilder<string, string>(consumerConfig);

            return consumerBuilder.Build();
        }
    }
}
