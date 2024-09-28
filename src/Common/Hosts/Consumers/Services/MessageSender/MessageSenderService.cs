using Confluent.Kafka;
using InsuranceGoSmoke.Common.Consumers.Options;
using InsuranceGoSmoke.Common.Consumers.Services.Builders;
using InsuranceGoSmoke.Common.Contracts.Abstract;
using System.Text.Json;

namespace InsuranceGoSmoke.Common.Consumers.Services.MessageSender
{
    /// <inheritdoc/>
    public class MessageSenderService : IMessageSenderService, IDisposable
    {
        private readonly Lazy<IProducer<Null, string>> _producer;

        /// <summary>
        /// Создаёт экземпляр <see cref="KafkaOptions"/>
        /// </summary>
        /// <param name="kafkaProducerBuilder">Builder отправителя.</param>
        public MessageSenderService(IKafkaProducerBuilder kafkaProducerBuilder)
        {
            _producer = new Lazy<IProducer<Null, string>>(kafkaProducerBuilder.Build);
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Освобождение ресурсов.
        /// </summary>
        /// <param name="disposing">Признак освобождения ресурсов.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing && _producer.IsValueCreated)
            {
                _producer.Value.Dispose();
            }
        }

        /// <inheritdoc/>
        public async Task SendAsync<TEvent>(TEvent @event, CancellationToken cancellationToken) where TEvent : class, IEvent
        {
            var json = JsonSerializer.Serialize(@event);
            var topic = @event.GetType().FullName!.ToLower();

            var message = new Message<Null, string> { Value = json };

            await _producer.Value.ProduceAsync(topic, message, cancellationToken);
            _producer.Value.Flush(cancellationToken);
        }
    }
}
