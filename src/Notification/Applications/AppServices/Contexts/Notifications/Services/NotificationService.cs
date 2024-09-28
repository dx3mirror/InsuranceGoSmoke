using InsuranceGoSmoke.Common.Contracts.Utilities.Helpers;
using InsuranceGoSmoke.Notification.Applications.AppServices.Contexts.Emails.Services.Abstract;
using InsuranceGoSmoke.Notification.Applications.AppServices.Contexts.Notifications.Services.Abstract;
using InsuranceGoSmoke.Notification.Applications.AppServices.Contexts.Templates.Services.Abstract;
using InsuranceGoSmoke.Notification.Contracts.Notifications.Enums;

namespace InsuranceGoSmoke.Notifications.Applications.AppServices.Contexts.Notifications.Services
{
    /// <inheritdoc/>
    public class NotificationService : INotificationService
    {
        private readonly IEmailSendingService _emailSendingService;
        private readonly ITemplateService _templateService;

        /// <summary>
        /// Создаёт экземпляр <see cref="NotificationService"/>
        /// </summary>
        /// <param name="emailSendingService">Сервис отправки писем на Email.</param>
        /// <param name="templateService">Сервис работы с шаблонами.</param>
        public NotificationService(
            IEmailSendingService emailSendingService, 
            ITemplateService templateService)
        {
            _emailSendingService = emailSendingService;
            _templateService = templateService;
        }

        /// <inheritdoc/>
        public async Task SendEmailAsync(string email, NotificationType notificationType, IReadOnlyDictionary<string, string> data, CancellationToken cancellationToken)
        {
            var title = EnumHelper.GetEnumDescription(notificationType);
            if (string.IsNullOrEmpty(title))
            {
                throw new ArgumentException("Не удалось получить заголовок письма для уведомления: " + notificationType);
            }

            var message = await _templateService.GetMessageByTemplateAsync(notificationType, data, cancellationToken);
            await _emailSendingService.SendAsync(email, title, message, cancellationToken);
        }
    }
}
