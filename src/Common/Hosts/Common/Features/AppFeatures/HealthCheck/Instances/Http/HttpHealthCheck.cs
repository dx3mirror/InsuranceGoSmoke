using InsuranceGoSmoke.Common.Hosts.Features.AppFeatures.HealthCheck.Base;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using System.Globalization;

namespace InsuranceGoSmoke.Common.Hosts.Features.AppFeatures.HealthCheck.Instances.Http
{
    /// <summary>
    /// Проверка на работоспособность API.
    /// </summary>
    /// <param name="_configuration">Конфигурация.</param>
    /// <param name="_httpClientFactory">Фабрика клиентов HTTP.</param>
    /// <param name="_options">Настройки.</param>
    /// <param name="_logger">Логгер.</param>
    public class HttpHealthCheck(IConfiguration _configuration, IHttpClientFactory _httpClientFactory, HttpHealthCheckOptions _options, ILogger<HttpHealthCheck> _logger) : IHealthCheck
    {
        /// <summary>
        /// Проверка на работоспособность.
        /// </summary>
        /// <param name="context">Контекст.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Результат проверки.</returns>
        /// <exception cref="HealthCheckConfigurationException">Исключение, бросаемое когда операция не может быть выполнена.</exception>
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
            CancellationToken cancellationToken = default)
        {
            if (!TimeSpan.TryParse(_options.Timeout, CultureInfo.CurrentCulture, out var timeout))
            {
                throw new HealthCheckConfigurationException($"Не удалось получить Timeout из {_options.Timeout}");
            }

            if (string.IsNullOrWhiteSpace(_options.UrlConfigSection))
            {
                throw new HealthCheckConfigurationException("Название секции с URL для проверки не может быть пустой.");
            }

            var url = _configuration.GetValue<string>(_options.UrlConfigSection);
            if (string.IsNullOrWhiteSpace(url))
            {
                throw new HealthCheckConfigurationException($"Не удалось получить URL из секции '{_options.UrlConfigSection}' для проверки.");
            }

            HttpResponseMessage? response = null;
            try
            {
                using var httpClient = _httpClientFactory.CreateClient();
                httpClient.Timeout = timeout;
                response = await httpClient.GetAsync(url, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Произошла ошибка при проверке работоспособности сервиса по URL: {Url}", url);
            }

            if (response?.IsSuccessStatusCode == true)
            {
                return HealthCheckResult.Healthy();
            }

            return HealthCheckResult.Unhealthy(response?.StatusCode.ToString());
        }
    }
}
