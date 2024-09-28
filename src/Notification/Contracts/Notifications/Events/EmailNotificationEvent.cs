using InsuranceGoSmoke.Common.Contracts.Abstract;
using InsuranceGoSmoke.Notification.Contracts.Notifications.Enums;

namespace InsuranceGoSmoke.Security.Contracts.Identify.Events
{
    /// <inheritdoc/>
    public class EmailNotificationEvent : Event
    {
        /// <inheritdoc/>
        public string Email { get; set; }

        /// <summary>
        /// Тип уведомления.
        /// </summary>
        public NotificationType NotificationType { get; set; }

        /// <summary>
        /// Данные.
        /// </summary>
        public IDictionary<string, string> Data { get; set; }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="correlationId">Идентфиикатор сообщения.</param>
        /// <param name="email">Email.</param>
        /// <param name="notificationType">Тип уведомления.</param>
        /// <param name="data">Данные.</param>
        public EmailNotificationEvent(Guid correlationId, string email, NotificationType notificationType, IDictionary<string, string> data) : base(correlationId)
        {
            Email = email;
            NotificationType = notificationType;
            Data = data;
        }
    }
}
