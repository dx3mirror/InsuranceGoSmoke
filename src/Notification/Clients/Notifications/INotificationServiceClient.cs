using InsuranceGoSmoke.Notification.Contracts.Notifications.Requests;

namespace InsuranceGoSmoke.Notification.Clients.Notifications
{
    /// <summary>
    /// Клиент для работы с сервисом уведомлений.
    /// </summary>
    public interface INotificationServiceClient
    {
        /// <summary>
        /// Отправляет уведомление на email.
        /// </summary>
        /// <param name="data">Данные.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        Task SendToEmailAsync(EmailNotificationData data, CancellationToken cancellationToken);
    }
}
