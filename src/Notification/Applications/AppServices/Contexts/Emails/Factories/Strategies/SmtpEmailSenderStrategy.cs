using InsuranceGoSmoke.Common.Contracts.Utilities.Helpers;
using InsuranceGoSmoke.Notification.Applications.AppServices.Contexts.Emails.Models.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace InsuranceGoSmoke.Notification.Applications.AppServices.Contexts.Emails.Factories.Strategies
{
    /// <summary>
    /// Стратегия отправки письма на Email через Smtp.
    /// </summary>
    public class SmtpEmailSenderStrategy : IEmailSenderStrategy
    {
        private readonly EmailSendingOptions _options;
        private readonly ILogger<SmtpEmailSenderStrategy> _logger;

        /// <summary>
        /// Создаёт экземпляр <see cref="SmtpEmailSenderStrategy"/>
        /// </summary>
        /// <param name="options">Настройки.</param>
        /// <param name="logger">Логгер.</param>
        public SmtpEmailSenderStrategy(
            IOptions<EmailSendingOptions> options, 
            ILogger<SmtpEmailSenderStrategy> logger)
        {
            _options = options.Value;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task SendAsync(string email, string title, string message, CancellationToken cancellationToken)
        {
            var port = _options.Port.ToInt();
            if (!port.HasValue)
            {
                _logger.LogError("Не задан порт SMTP сервера.");
                return;
            }

            using SmtpClient client = new (_options.Host, port.Value);
            client.Timeout = 10_000; // 10 секунд
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            if (!string.IsNullOrEmpty(_options.UserName) && !string.IsNullOrEmpty(_options.Password))
            {
                client.Credentials = new NetworkCredential(_options.UserName, _options.Password);
            }

            MailMessage mailMessage = new(_options.SenderEmail!, email)
            {
                Subject = title,
                Body = message,
                IsBodyHtml = true
            };

            try
            {
                await client.SendMailAsync(mailMessage, cancellationToken);
            }
            catch(Exception ex) 
            {
                _logger.LogError(ex, "Произошла ошибка при отправке письма на '{Email}'", email);
            }
        }
    }
}
