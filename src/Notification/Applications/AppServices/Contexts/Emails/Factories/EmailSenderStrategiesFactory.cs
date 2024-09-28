using InsuranceGoSmoke.Notification.Applications.AppServices.Contexts.Emails.Factories.Strategies;
using InsuranceGoSmoke.Notification.Applications.AppServices.Contexts.Emails.Models.Options;
using Microsoft.Extensions.Options;

namespace InsuranceGoSmoke.Notification.Applications.AppServices.Contexts.Emails.Factories
{
    /// <inheritdoc/>
    public class EmailSenderStrategiesFactory : IEmailSenderStrategiesFactory
    {
        private static readonly string ExchangeType = "Exchange";

        private readonly EmailSendingOptions _options;
        private readonly Lazy<ExchangeEmailSenderStrategy> _exchangeEmailSenderStrategy;
        private readonly Lazy<SmtpEmailSenderStrategy> _smtpEmailSenderStrategy;

        /// <summary>
        /// Создаёт экземпляр <see cref="EmailSenderStrategiesFactory"/>
        /// </summary>
        /// <param name="options">Настройки.</param>
        /// <param name="exchangeEmailSenderStrategy">Стратегия отправки через exchange.</param>
        /// <param name="smtpEmailSenderStrategy">Стратегия отправки через smtp.</param>
        public EmailSenderStrategiesFactory(
            IOptions<EmailSendingOptions> options,
            Lazy<ExchangeEmailSenderStrategy> exchangeEmailSenderStrategy,
            Lazy<SmtpEmailSenderStrategy> smtpEmailSenderStrategy)
        {
            _options = options.Value;
            _exchangeEmailSenderStrategy = exchangeEmailSenderStrategy;
            _smtpEmailSenderStrategy = smtpEmailSenderStrategy;
        }

        /// <inheritdoc/>
        public IEmailSenderStrategy GetStrategy()
        {
            if (_options.Strategy == ExchangeType)
            {
                return _exchangeEmailSenderStrategy.Value;
            }

            return _smtpEmailSenderStrategy.Value;
        }
    }
}
