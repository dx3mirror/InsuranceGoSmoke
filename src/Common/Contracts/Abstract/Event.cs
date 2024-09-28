namespace InsuranceGoSmoke.Common.Contracts.Abstract
{
    /// <inheritdoc/>
    public class Event : IEvent
    {
        /// <inheritdoc/>
        public Guid CorrelationId { get; set; }

        /// <summary>
        /// Конструктор.
        /// </summary>
        public Event()
        {
            CorrelationId = Guid.Empty;
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="correlationId">Идентификатор.</param>
        public Event(Guid correlationId)
        {
            CorrelationId = correlationId;
        }
    }

    /// <summary>
    /// Событие.
    /// </summary>
    public interface IEvent : IMessage
    {
    }
}
