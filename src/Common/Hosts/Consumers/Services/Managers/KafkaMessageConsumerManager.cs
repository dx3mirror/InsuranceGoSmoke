using InsuranceGoSmoke.Common.Consumers.Services.Consumers;

namespace InsuranceGoSmoke.Common.Consumers.Services.Managers
{
    /// <inheritdoc/>
    public class KafkaMessageConsumerManager : IKafkaMessageConsumerManager
    {
        private readonly IEnumerable<IKafkaTopicConsumer> _consumers;

        /// <summary>
        /// Создаёт экземпляр <see cref="KafkaMessageConsumerManager"/>
        /// </summary>
        /// <param name="consumers">Обработчики.</param>
        public KafkaMessageConsumerManager(IEnumerable<IKafkaTopicConsumer> consumers)
        {
            _consumers = consumers;
        }

        /// <inheritdoc/>
        public void StartConsumers(CancellationToken cancellationToken)
        {
            foreach (var consumer in _consumers)
            {
                new Thread(() => consumer.StartConsuming(cancellationToken))
                    .Start();
            }
        }
    }
}
