using InsuranceGoSmoke.Common.Consumers.Services.Builders;
using InsuranceGoSmoke.Common.Consumers.Services.Consumers;
using InsuranceGoSmoke.Notification.Clients.Notifications;
using InsuranceGoSmoke.Notification.Contracts.Notifications.Requests;
using InsuranceGoSmoke.Security.Contracts.Identify.Events;
using Microsoft.Extensions.Logging;

namespace InsuranceGoSmoke.Notification.Consumers.Consumers
{
    /// <summary>
    /// Обработчик события <see cref="EmailNotificationEvent"/>
    /// </summary>
    internal class EmailNotificationConsumer : BaseKafkaTopicConsumer<EmailNotificationEvent>
    {
        /// <inheritdoc/>
        private readonly INotificationServiceClient _notificationServiceClient;

        /// <summary>
        /// Создаёт экземпляр <see cref="EmailNotificationConsumer"/>
        /// </summary>
        /// <param name="logger">Логгер.</param>
        /// <param name="kafkaConsumerBuilder">Builder.</param>
        /// <param name="notificationServiceClient">Клиент работы с уведомлениями.</param>
        public EmailNotificationConsumer(ILogger<EmailNotificationConsumer> logger,
            IKafkaConsumerBuilder kafkaConsumerBuilder,
            INotificationServiceClient notificationServiceClient) : base(kafkaConsumerBuilder, logger)
        {
            _notificationServiceClient = notificationServiceClient;
        }

        /// <inheritdoc/>
        public override Task ProcessAsync(EmailNotificationEvent message, CancellationToken cancellationToken)
        {
            var data = new EmailNotificationData()
            {
                Email = message.Email,
                NotificationType = message.NotificationType,
                Data = message.Data.ToDictionary()
            };
            _notificationServiceClient.SendToEmailAsync(data, cancellationToken);
            return Task.CompletedTask;
        }
    }
}
