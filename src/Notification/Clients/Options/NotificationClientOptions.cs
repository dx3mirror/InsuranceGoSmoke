using InsuranceGoSmoke.Common.Clients.Options;
using InsuranceGoSmoke.Common.Contracts.Options;

namespace InsuranceGoSmoke.Notification.Clients.Options
{
    /// <summary>
    /// Настройки клиента уведомлений.
    /// </summary>
    [ConfigurationOptions("NotificationClient")]
    public class NotificationClientOptions : IClientOptions
    {
        /// <inheritdoc/>
        public string Url { get; set; } = string.Empty;
    }
}
