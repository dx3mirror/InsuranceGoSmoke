using InsuranceGoSmoke.Common.Applications.Handlers.Abstract;
using InsuranceGoSmoke.Notification.Applications.AppServices.Contexts.Notifications.Services.Abstract;

namespace InsuranceGoSmoke.Notification.Applications.Handlers.Contexts.Notifications.Commands.SendEmail
{
    /// <summary>
    /// Обработчик команды <see cref="SendEmailRequest"/>
    /// </summary>
    public class SendEmailRequestHandler : ICommandHandler<SendEmailRequest>
    {
        private readonly INotificationService _notificationService;

        /// <summary>
        /// Создаёт экземпляр <see cref="SendEmailRequestHandler"/>
        /// </summary>
        /// <param name="notificationService">Сервис работы с уведомлениями.</param>
        public SendEmailRequestHandler(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        /// <inheritdoc/>
        public Task Handle(SendEmailRequest request, CancellationToken cancellationToken)
        {
            return _notificationService.SendEmailAsync(request.Email, request.NotificationType, request.Data, cancellationToken);
        }
    }
}
