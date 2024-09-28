namespace InsuranceGoSmoke.Common.Hosts.Models.Errors
{
    /// <summary>
    /// Ошибки API.
    /// </summary>
    public class ApiError
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="traceId">Идентификатор трассировки.</param>
        /// <param name="message">Сообщение об ошибке.</param>
        public ApiError(string traceId, string message)
        {
            TraceId = traceId;
            Message = message;
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="message">Сообщение об ошибке.</param>
        public ApiError(string message)
        {
            Message = message;
        }

        /// <summary>
        /// Идентификатор трассировки.
        /// </summary>
        public string? TraceId { get; set; }

        /// <summary>
        /// Сообщение об ошибке.
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Описание.
        /// </summary>
        public string? Description { get; set; }
    }
}
