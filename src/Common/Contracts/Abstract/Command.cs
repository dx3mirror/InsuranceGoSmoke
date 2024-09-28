using MediatR;

namespace InsuranceGoSmoke.Common.Contracts.Abstract
{
    /// <summary>
    /// Базовый класс команды.
    /// </summary>
    public abstract class CommandBase : IMessage
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="correlationId">Идентификатор сообщения.</param>
        protected CommandBase(Guid correlationId) => CorrelationId = correlationId;

        /// <summary>
        /// Идентификатор сообщения.
        /// </summary>
        public Guid CorrelationId { get; set; }
    }

    /// <summary>
    /// Команда.
    /// </summary>
    public abstract class Command : CommandBase, IRequest
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        protected Command() : base(Guid.NewGuid())
        {
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="correlationId">Идентификатор сообщения.</param>
        protected Command(Guid correlationId) : base(correlationId)
        {
        }
    }

    /// <summary>
    /// Команда.
    /// </summary>
    /// <typeparam name="TResponse">Тип ответа.</typeparam>
    public abstract class Command<TResponse> : CommandBase, IRequest<TResponse>
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        protected Command() : base(Guid.NewGuid())
        {
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="correlationId">Идентификатор сообщения.</param>
        protected Command(Guid correlationId) : base(correlationId)
        {
        }
    }
}
