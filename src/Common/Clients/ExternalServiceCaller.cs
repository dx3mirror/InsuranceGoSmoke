using Polly.Retry;
using Polly;
using Microsoft.Extensions.Logging;

namespace InsuranceGoSmoke.Common.Clients
{
    /// <summary>
    /// Класс для выполнения вызовов к внешним сервисам с поддержкой политики повторных попыток.
    /// </summary>
    public class ExternalServiceCaller : IExternalServiceCaller
    {
        private readonly HttpClient _httpClient;
        private readonly AsyncRetryPolicy _retryPolicy;
        private readonly ILogger<ExternalServiceCaller> _logger;

        /// <summary>
        /// Инициализирует новый экземпляр <see cref="ExternalServiceCaller"/>.
        /// </summary>
        /// <param name="httpClient">Экземпляр <see cref="HttpClient"/> для выполнения HTTP-запросов.</param>
        /// <param name="logger">Экземпляр <see cref="ILogger"/> для логирования операций.</param>
        /// <exception cref="ArgumentNullException">Если <paramref name="httpClient"/> или <paramref name="logger"/> равны null.</exception>
        public ExternalServiceCaller(HttpClient httpClient, ILogger<ExternalServiceCaller> logger)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _retryPolicy = Policy
                .Handle<HttpRequestException>()
                .Or<TaskCanceledException>()
                .WaitAndRetryAsync(
                    retryCount: 3,
                    sleepDurationProvider: attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)),
                    onRetry: (exception, timespan, retryCount, context) =>
                    {
                        _logger.LogWarning($"Попытка {retryCount}: Повтор через {timespan.TotalSeconds} секунд из-за ошибки: {exception.Message}");
                    });
        }

        /// <summary>
        /// Выполняет вызов внешнего сервиса с поддержкой политики повторных попыток.
        /// </summary>
        /// <typeparam name="TResponse">Тип ожидаемого ответа от внешнего сервиса.</typeparam>
        /// <param name="action">Функция, выполняющая запрос к внешнему сервису.</param>
        /// <param name="cancellationToken">Токен отмены для прекращения выполнения.</param>
        /// <returns>Ответ от внешнего сервиса, если успешен; иначе, значение по умолчанию для типа <typeparamref name="TResponse"/>.</returns>
        public async Task<TResponse?> CallExternalServiceAsync<TResponse>(Func<Task<TResponse>> action, CancellationToken cancellationToken)
        {
            if (action == null) throw new ArgumentNullException(nameof(action));

            try
            {
                return await _retryPolicy.ExecuteAsync(async ct =>
                {
                    _logger.LogInformation("Вызов внешнего сервиса...");
                    var result = await action();

                    _logger.LogInformation("Вызов внешнего сервиса выполнен успешно.");
                    return result;
                }, cancellationToken);
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                _logger.LogError(ex, "Не удалось выполнить вызов внешнего сервиса.");
                return default;
            }
        }
    }
}


