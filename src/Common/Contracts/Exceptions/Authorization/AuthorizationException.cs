namespace InsuranceGoSmoke.Common.Contracts.Exceptions.Authorization
{
    /// <summary>
    /// Исключение, выбрасываемое во время проблем с авторизацией.
    /// </summary>
    public class AuthorizationException : Exception
    {
        /// <summary>
        /// Создает экземпляр <see cref="AuthorizationException"/>
        /// </summary>
        /// <param name="message">Сообщение об ошибке.</param>
        public AuthorizationException(string? message) : base(message)
        {
        }
    }
}
