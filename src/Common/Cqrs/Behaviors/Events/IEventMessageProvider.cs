using InsuranceGoSmoke.Common.Contracts.Abstract;

namespace InsuranceGoSmoke.Common.Cqrs.Behaviors.Events
{
    /// <summary>
    /// Провайдер событий.
    /// </summary>
    public interface IEventMessageProvider
    {
        /// <summary>
        /// Добавить событие.
        /// </summary>
        /// <param name="event">Событие.</param>
        public void Add(IEvent @event);

        /// <summary>
        /// Возвращает событие из очереди.
        /// </summary>
        /// <returns>Событие.</returns>
        public IEvent? Get();
    }
}
