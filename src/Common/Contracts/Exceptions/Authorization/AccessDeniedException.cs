namespace InsuranceGoSmoke.Common.Contracts.Exceptions.Authorization
{
    /// <summary>
    /// Исключение запрета доступа.
    /// </summary>
    public class AccessDeniedException : Exception
    {
        /// <summary>
        /// Создаёт экземпляр <see cref="AccessDeniedException"/>
        /// </summary>
        /// <param name="message">Сообщение.</param>
        public AccessDeniedException(string message) : base(message)
        {
        }
    }
}
