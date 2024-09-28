using MediatR;

namespace InsuranceGoSmoke.Common.Contracts.Abstract
{
    /// <summary>
    /// Интерфейс запроса.
    /// </summary>
    /// <typeparam name="TResponse">Тип ответа.</typeparam>
    public class Query<TResponse> : IMessage, IRequest<TResponse>
    {
        /// <summary>
        /// Идентификатор сообщения.
        /// </summary>
        public Guid CorrelationId { get; set; }

        /// <summary>
        /// Конструктор.
        /// </summary>
        protected Query() : this(Guid.NewGuid())
        {
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="correlationId">Идентификатор сообщения.</param>
        protected Query(Guid correlationId) => CorrelationId = correlationId;
    }
}
