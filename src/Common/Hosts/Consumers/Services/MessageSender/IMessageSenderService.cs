using InsuranceGoSmoke.Common.Contracts.Abstract;

namespace InsuranceGoSmoke.Common.Consumers.Services.MessageSender
{
    /// <summary>
    /// Сервис отправки сообщений.
    /// </summary>
    public interface IMessageSenderService
    {
        /// <summary>
        /// Отправляет событие.
        /// </summary>
        /// <typeparam name="TEvent">Тип сообщения.</typeparam>
        /// <param name="event">Событие.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        Task SendAsync<TEvent>(TEvent @event, CancellationToken cancellationToken) where TEvent : class, IEvent;
    }
}
