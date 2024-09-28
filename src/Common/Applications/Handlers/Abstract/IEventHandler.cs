using InsuranceGoSmoke.Common.Contracts.Abstract;

namespace InsuranceGoSmoke.Common.Applications.Handlers.Abstract
{
    /// <summary>
    /// Интерфейс обработчика по диагносике.
    /// </summary>
    public interface IEventHandler
    {
        /// <summary>
        /// Обработка.
        /// </summary>
        /// <param name="event">Событие.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        public Task HandleAsync(IEvent @event, CancellationToken cancellationToken);
    }

    /// <summary>
    /// Интерфейс обработчика по диагносике.
    /// </summary>
    public abstract class EventHandler<TEvent> : IEventHandler
        where TEvent : IEvent
    {
        /// <inheritdoc/>
        public Task HandleAsync(IEvent @event, CancellationToken cancellationToken)
        {
            return HandleAsync((TEvent)@event, cancellationToken);
        }

        /// <summary>
        /// Обработка события.
        /// </summary>
        /// <param name="event">Событие.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        public abstract Task HandleAsync(TEvent @event, CancellationToken cancellationToken);
    }
}
