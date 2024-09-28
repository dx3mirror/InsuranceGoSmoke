using InsuranceGoSmoke.Notification.Applications.AppServices.Contexts.Emails.Services.Abstract;
using Microsoft.Extensions.Options;
using InsuranceGoSmoke.Notification.Applications.AppServices.Contexts.Emails.Models.Options;
using Microsoft.Extensions.Logging;
using InsuranceGoSmoke.Notification.Applications.AppServices.Contexts.Emails.Factories;

namespace InsuranceGoSmoke.Notification.Applications.AppServices.Contexts.Emails.Services
{
    /// <inheritdoc/>
    public class EmailSendingService : IEmailSendingService
    {
        private readonly IEmailSenderStrategiesFactory _emailSenderStrategiesFactory;
        private readonly ILogger<EmailSendingService> _logger;
        private readonly EmailSendingOptions _options;

        /// <summary>
        /// Создаёт экземпляр <see cref="EmailSendingService"/>
        /// </summary>
        /// <param name="emailSenderStrategiesFactory">Фабика стратегий отправки писем.</param>
        /// <param name="options">Настройки.</param>
        /// <param name="logger">Логгер.</param>
        public EmailSendingService(
            IEmailSenderStrategiesFactory emailSenderStrategiesFactory,
            IOptions<EmailSendingOptions> options, 
            ILogger<EmailSendingService> logger)
        {
            _options = options.Value;
            _emailSenderStrategiesFactory = emailSenderStrategiesFactory;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task SendAsync(string email, string title, string message, CancellationToken cancellationToken)
        {
            if (!_options.IsEnabled)
            {
                _logger.LogError("Сообщения для уведомления '{Email}' успешно сформировано. {Title}: {Message}", email, title, message);
                return;
            }

            var strategy = _emailSenderStrategiesFactory.GetStrategy();
            await strategy.SendAsync(email, title, message, cancellationToken);
        }
    }
}
