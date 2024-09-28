using InsuranceGoSmoke.Notification.Contracts.Attributes;
using System.ComponentModel;

namespace InsuranceGoSmoke.Notification.Contracts.Notifications.Enums
{
    /// <summary>
    /// Тип уведомлений.
    /// </summary>
    public enum NotificationType
    {
        /// <summary>
        /// Неопределенно.
        /// </summary>
        Undefined = 0,

        /// <summary>
        /// Шаблон подтверждения Email.
        /// </summary>
        [Description("Подтверждение Email")]
        [TemplateName("email.confirmation")]
        EmailConfirmation = 1,
    }
}
