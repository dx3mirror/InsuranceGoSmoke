using InsuranceGoSmoke.Common.Contracts.Exceptions.Common;
using InsuranceGoSmoke.Static.Clients.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace InsuranceGoSmoke.Static.Clients
{
    /// <inheritdoc/>
    public class StaticTemplateClient : IStaticTemplateClient
    {
        private readonly HttpClient _httpClient;
        private readonly StaticTemplateOptions _options;
        private readonly ILogger<StaticTemplateClient> _logger;

        /// <summary>
        /// Создаёт экземпляр <see cref="StaticTemplateClient"/>
        /// </summary>
        /// <param name="options">Настройки.</param>
        /// <param name="logger">Логгер.</param>
        /// <param name="httpClient">HTTP клиент.</param>
        public StaticTemplateClient(IOptions<StaticTemplateOptions> options,
            ILogger<StaticTemplateClient> logger,
            HttpClient httpClient)
        {
            _options = options.Value;
            _logger = logger;
            _httpClient = httpClient;
        }

        /// <inheritdoc/>
        public async Task<string> DownloadTemplateAsync(string templateName, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(templateName))
            {
                throw new ArgumentException($"'{nameof(templateName)}' не может быть пустым.", nameof(templateName));
            }

            var url = $"{_options.Url}/Templates/Notifications/{templateName}.html";

            try
            {
                var response = await _httpClient.GetAsync(url, cancellationToken);
                var content = await response.Content.ReadAsStringAsync(cancellationToken);

                if (!response.IsSuccessStatusCode)
                {
                    throw new FileLoadException(
                        $"Код ответа '{response.StatusCode}' не указывает на успешное скачивание шаблона. Метод: '{url}'. Ответ: '{content}'");
                }

                return content;
            }
            catch (IOException ex)
            {
                _logger.LogError(ex, "Произошла ошибка при скачивании шаблона: {Url}", url);
                throw new ReadableException("При скачивании шаблона произошла ошибки.", ex);
            }
        }
    }
}
