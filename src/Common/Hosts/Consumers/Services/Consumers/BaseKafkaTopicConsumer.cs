using Confluent.Kafka;
using InsuranceGoSmoke.Common.Consumers.Exceptions;
using InsuranceGoSmoke.Common.Consumers.Services.Builders;
using InsuranceGoSmoke.Common.Contracts.Abstract;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace InsuranceGoSmoke.Common.Consumers.Services.Consumers
{
    /// <summary>
    /// Обработчик событий.
    /// </summary>
    /// <typeparam name="TEvent">Событий.</typeparam>
    public abstract class BaseKafkaTopicConsumer<TEvent> : IKafkaTopicConsumer<TEvent> where TEvent : IEvent
    {
        /// <inheritdoc/>
        public virtual string TopicName { get; } = typeof(TEvent).FullName!.ToLower();

        private readonly ILogger _logger;
        private readonly IKafkaConsumerBuilder _kafkaConsumerBuilder;

        /// <summary>
        /// Создаёт экземпляр.
        /// </summary>
        /// <param name="kafkaConsumerBuilder">Builder.</param>
        /// <param name="logger">Логгер.</param>
        protected BaseKafkaTopicConsumer(IKafkaConsumerBuilder kafkaConsumerBuilder, ILogger logger)
        {
            _kafkaConsumerBuilder = kafkaConsumerBuilder;
            _logger = logger;
        }

        /// <inheritdoc/>
        public abstract Task ProcessAsync(TEvent message, CancellationToken cancellationToken);

        /// <inheritdoc/>
        public async Task StartConsuming(CancellationToken cancellationToken)
        {
            using var consumer = _kafkaConsumerBuilder.Build();
            consumer.Subscribe(TopicName);

            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    try
                    {
                        var consumeResult = consumer.Consume(cancellationToken)
                                                ?? throw new KafkaConsumerException($"Ответ при чтении сообщения из топика '{TopicName}' пустой.");
                        
                        var message = JsonSerializer.Deserialize<TEvent>(consumeResult.Message.Value)
                                                ?? throw new KafkaConsumerException($"Cообщение из топика '{TopicName}' пустое.");
                        await ProcessAsync(message, cancellationToken);
                    }
                    catch (ConsumeException ex)
                    {
                        _logger.LogError(ex, "Не удалось получить сообщение из топика '{TopicName}'.", TopicName);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "При получении сообщения из топика '{TopicName}' произошла ошибка.", TopicName);
                    }
                }
            }
            catch (OperationCanceledException ex)
            {
                _logger.LogError(ex, "Произошло исключение отмены операции");
            }
            finally
            {
                consumer.Close();
            }
        }
    }
}
