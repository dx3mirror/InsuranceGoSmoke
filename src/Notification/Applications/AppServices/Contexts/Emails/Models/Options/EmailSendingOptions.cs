using InsuranceGoSmoke.Common.Contracts.Options;

namespace InsuranceGoSmoke.Notification.Applications.AppServices.Contexts.Emails.Models.Options
{
    /// <summary>
    /// Настройки отправки писем на email.
    /// </summary>
    [ConfigurationOptions("EmailSending")]
    public class EmailSendingOptions
    {
        /// <summary>
        /// Стратегия отправки.
        /// </summary>
        /// <remarks>Exchange или Smtp</remarks>
        public string? Strategy { get; set; }

        /// <summary>
        /// Хост.
        /// </summary>
        public string? Host { get; set; }

        /// <summary>
        /// Порт.
        /// </summary>
        public string? Port { get; set; }

        /// <summary>
        /// Email отправителя.
        /// </summary>
        public string? SenderEmail { get; set; }
        
        /// <summary>
        /// Логин.
        /// </summary>
        public string? UserName { get; set; }

        /// <summary>
        /// Пароль.
        /// </summary>
        public string? Password { get; set; }

        /// <summary>
        /// Домен.
        /// </summary>
        public string? Domain { get; set; }

        /// <summary>
        /// Признак, что отправка писем работает.
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// Признак, что трассировка включена.
        /// </summary>
        public bool IsTracingEnabled { get; set; }
    }
}
