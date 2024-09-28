using InsuranceGoSmoke.Common.Contracts.Abstract;
using InsuranceGoSmoke.Notification.Contracts.Notifications.Enums;
using InsuranceGoSmoke.Notification.Contracts.Notifications.Requests;

namespace InsuranceGoSmoke.Notification.Applications.Handlers.Contexts.Notifications.Commands.SendEmail
{
    /// <summary>
    /// Запрос на отправку уведомления на email.
    /// </summary>
    public class SendEmailRequest : Command
    {
        /// <summary>
        /// Создаёт экземпляр <see cref="SendEmailRequest"/>
        /// </summary>
        public SendEmailRequest(EmailNotificationData request)
        {
            Email = request.Email;
            NotificationType = request.NotificationType;
            Data = request.Data;
        }

        /// <summary>
        /// Email.
        /// </summary>
        public string Email { get; }

        /// <summary>
        /// Тип уведомления.
        /// </summary>
        public NotificationType NotificationType { get; }

        /// <summary>
        /// Данные.
        /// </summary>
        public IReadOnlyDictionary<string, string> Data { get; }
    }
}
