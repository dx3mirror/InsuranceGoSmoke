namespace InsuranceGoSmoke.Common.Contracts.Exceptions.Common
{
    /// <summary>
    /// Читаемое исключение, для отображения пользователям.
    /// </summary>
    public class ReadableException : Exception
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="message">Сообщение об ошибке.</param>
        public ReadableException(string message) : base(message)
        {
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="message">Сообщение об ошибке.</param>
        /// <param name="description">Описание ошибки.</param>
        public ReadableException(string message, string description) : base(message)
        {
            Description = description;
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="message">Сообщение об ошибке.</param>
        /// <param name="innerException">Вложенное исключение.</param>
        public ReadableException(string message, Exception? innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Описание.
        /// </summary>
        public string? Description { get; set; }
    }
}
