namespace InsuranceGoSmoke.Common.Clients
{
    /// <summary>
    /// Интерфейс для вызова внешнего сервиса с поддержкой политики повторных попыток.
    /// </summary>
    public interface IExternalServiceCaller
    {
        /// <summary>
        /// Выполняет вызов внешнего сервиса с поддержкой политики повторных попыток.
        /// </summary>
        /// <typeparam name="TResponse">Тип ожидаемого ответа от внешнего сервиса.</typeparam>
        /// <param name="action">Функция, выполняющая запрос к внешнему сервису.</param>
        /// <param name="cancellationToken">Токен отмены для прекращения выполнения.</param>
        /// <returns>Ответ от внешнего сервиса, если успешен; иначе, значение по умолчанию для типа <typeparamref name="TResponse"/>.</returns>
        Task<TResponse?> CallExternalServiceAsync<TResponse>(Func<Task<TResponse>> action, CancellationToken cancellationToken);
    }
}
