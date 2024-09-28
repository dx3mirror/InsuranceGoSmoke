using InsuranceGoSmoke.Notification.Contracts.Notifications.Enums;

namespace InsuranceGoSmoke.Notification.Applications.AppServices.Contexts.Notifications.Services.Abstract
{
    /// <summary>
    /// Сервис уведомлений.
    /// </summary>
    public interface INotificationService
    {
        /// <summary>
        /// Отвправляет уведомление на email.
        /// </summary>
        /// <param name="email">Email.</param>
        /// <param name="notificationType">Тип уведомления.</param>
        /// <param name="data">Данные.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        Task SendEmailAsync(string email, NotificationType notificationType, IReadOnlyDictionary<string, string> data, CancellationToken cancellationToken);
    }
}
