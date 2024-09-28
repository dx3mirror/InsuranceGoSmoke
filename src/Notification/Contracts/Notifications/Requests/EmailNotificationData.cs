using InsuranceGoSmoke.Notification.Contracts.Notifications.Enums;

namespace InsuranceGoSmoke.Notification.Contracts.Notifications.Requests
{
    /// <summary>
    /// Данные для отправки уведомления на почту.
    /// </summary>
    public class EmailNotificationData
    {
        /// <summary>
        /// Email.
        /// </summary>
        public required string Email { get; set; }

        /// <summary>
        /// Тип уведомления.
        /// </summary>
        public required NotificationType NotificationType { get; set; }

        /// <summary>
        /// Данные.
        /// </summary>
        public IReadOnlyDictionary<string, string> Data { get; set; } = new Dictionary<string, string>();
    }
}
