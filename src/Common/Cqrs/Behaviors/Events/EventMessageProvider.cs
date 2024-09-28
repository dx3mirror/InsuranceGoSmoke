using InsuranceGoSmoke.Common.Contracts.Abstract;

namespace InsuranceGoSmoke.Common.Cqrs.Behaviors.Events
{
    /// <inheritdoc/>
    public class EventMessageProvider : IEventMessageProvider
    {
        /// <summary>
        /// События.
        /// </summary>
        private readonly Queue<IEvent> Events = new Queue<IEvent>();

        /// <inheritdoc/>
        public void Add(IEvent @event)
        {
            Events.Enqueue(@event);
        }

        /// <inheritdoc/>
        public IEvent? Get()
        {
            var isSuccess = Events.TryDequeue(out var @event);
            if (isSuccess)
            {
                return @event;
            }
            return null;
        }
    }
}
