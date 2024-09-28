namespace InsuranceGoSmoke.Common.Contracts.Abstract
{
    /// <summary>
    /// Интерфейс сообщения.
    /// </summary>
    public interface IMessage
    {
        /// <summary>
        /// Идентификатор сообщения.
        /// </summary>
        Guid CorrelationId { get; set; }
    }
}
