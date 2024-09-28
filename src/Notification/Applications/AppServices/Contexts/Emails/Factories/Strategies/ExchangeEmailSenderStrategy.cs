using InsuranceGoSmoke.Notification.Applications.AppServices.Contexts.Emails.Models.Options;
using Microsoft.Exchange.WebServices.Data;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace InsuranceGoSmoke.Notification.Applications.AppServices.Contexts.Emails.Factories.Strategies
{
    /// <summary>
    /// Стратегия отправки письма на Email через Exchange.
    /// </summary>
    public class ExchangeEmailSenderStrategy : IEmailSenderStrategy
    {
        private readonly EmailSendingOptions _options;
        private readonly ILogger<SmtpEmailSenderStrategy> _logger;
        private readonly ITraceListener _traceListener;

        /// <summary>
        /// Создаёт экземпляр <see cref="ExchangeEmailSenderStrategy"/>
        /// </summary>
        /// <param name="options">Настройки.</param>
        /// <param name="logger">Логгер.</param>
        /// <param name="traceListener">Трассировщик.</param>
        public ExchangeEmailSenderStrategy(IOptions<EmailSendingOptions> options, 
            ILogger<SmtpEmailSenderStrategy> logger, 
            ITraceListener traceListener)
        {
            _options = options.Value;
            _logger = logger;
            _traceListener = traceListener;
        }

        /// <inheritdoc/>
        public System.Threading.Tasks.Task SendAsync(string email, string title, string message, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(_options.Host))
            {
                _logger.LogError("Не задан хост сервера.");
                return System.Threading.Tasks.Task.CompletedTask;
            }

            if (string.IsNullOrEmpty(_options.UserName))
            {
                _logger.LogError("Не задан логин.");
                return System.Threading.Tasks.Task.CompletedTask;
            }

            if (string.IsNullOrEmpty(_options.Password))
            {
                _logger.LogError("Не задан пароль.");
                return System.Threading.Tasks.Task.CompletedTask;
            }

            var service = new ExchangeService(ExchangeVersion.Exchange2013_SP1)
            {
                Credentials = new WebCredentials(_options.UserName, _options.Password, _options.Domain),
                Url = new Uri(_options.Host)
            };

            if (_options.IsTracingEnabled) 
            {
                service.TraceFlags = TraceFlags.All;
                service.TraceEnabled = true;
                service.TraceListener = _traceListener;
            }

            EmailMessage emailMessage = new(service)
            {
                Subject = title,
                Body = message,
                ToRecipients = { email }
            };
            
            emailMessage.Send();
            return System.Threading.Tasks.Task.CompletedTask;
        }
    }
}
