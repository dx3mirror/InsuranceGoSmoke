using InsuranceGoSmoke.Common.Contracts.Abstract;

namespace InsuranceGoSmoke.Common.Consumers.Services.Consumers
{
    /// <summary>
    /// Именованный обработчик.
    /// </summary>
    /// <typeparam name="TMessage">Тип сообщения.</typeparam>
    public interface IKafkaTopicConsumer<in TMessage> : IKafkaTopicConsumer where TMessage : IEvent
    {
        /// <summary>
        /// Обработка сообщения.
        /// </summary>
        /// <param name="message">Сообщение.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        Task ProcessAsync(TMessage message, CancellationToken cancellationToken);
    }

    /// <summary>
    /// Именованный обработчик.
    /// </summary>
    public interface IKafkaTopicConsumer
    {
        /// <summary>
        /// Наименование очереди.
        /// </summary>
        string TopicName { get; }

        /// <summary>
        /// Запускает работу consumer'а.
        /// </summary>
        /// <param name="cancellationToken">Токен отмены.</param>
        Task StartConsuming(CancellationToken cancellationToken);
    }
}
