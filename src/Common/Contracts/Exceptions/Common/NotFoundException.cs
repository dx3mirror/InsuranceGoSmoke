namespace InsuranceGoSmoke.Common.Contracts.Exceptions.Common
{
    /// <summary>
    /// Исключение, вызываемое когда не найден объект.
    /// </summary>
    public class NotFoundException : Exception
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="message">Сообщение об ошибке.</param>
        public NotFoundException(string message) : base(message)
        {
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="message">Сообщение об ошибке.</param>
        /// <param name="innerException">Вложенное исключение.</param>
        public NotFoundException(string message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
