using InsuranceGoSmoke.Notification.Clients.Notifications;
using InsuranceGoSmoke.Notification.Contracts.Notifications.Enums;
using InsuranceGoSmoke.Notification.Contracts.Notifications.Requests;
using InsuranceGoSmoke.Security.Applications.AppServices.Contexts.Users.Models;
using InsuranceGoSmoke.Security.Applications.Handlers.Contexts.Users.Commands.UpdateUserField;
using InsuranceGoSmoke.Security.Contracts.Identify.Options;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text.Json;

namespace InsuranceGoSmoke.Security.Applications.Handlers.Contexts.Users.Events.EmailConfirmNotification
{
    /// <summary>
    /// Обработчик события <see cref="UpdateUserFieldRequest"/>
    /// </summary>
    public class EmailConfirmNotificationEventHandler
        : Common.Applications.Handlers.Abstract.EventHandler<EmailConfirmNotificationEvent>
    {
        private readonly INotificationServiceClient _notificationServiceClient;
        private readonly IdentifyOptions _options;
        private readonly IDistributedCache _distributedCache;

        /// <summary>
        /// Создаёт экземпляр <see cref="EmailConfirmNotificationEventHandler"/>.
        /// </summary>
        public EmailConfirmNotificationEventHandler(
            INotificationServiceClient notificationServiceClient,
            IOptions<IdentifyOptions> options,
            IDistributedCache distributedCache)
        {
            _notificationServiceClient = notificationServiceClient;
            _options = options.Value;
            _distributedCache = distributedCache;
        }

        /// <inheritdoc/>>
        public override async Task HandleAsync(EmailConfirmNotificationEvent @event, CancellationToken cancellationToken)
        {
            var expirationTime = TimeSpan.FromHours(_options.EmailVerificationTimeInHours);
            var code = GenerateAndSaveEmailVerification(@event.UserId, @event.Email, expirationTime);

            var parameters = new Dictionary<string, string>()
            {
                { "Link", string.Format(_options.EmailVerificationLink, code) },
                { "Email", @event.Email }
            };
            var data = new EmailNotificationData { Email = @event.Email, NotificationType = NotificationType.EmailConfirmation, Data = parameters };
            await _notificationServiceClient.SendToEmailAsync(data, cancellationToken);
        }

        private string GenerateAndSaveEmailVerification(Guid userId, string email, TimeSpan? expirationTime)
        {
            var code = GenerateEmailVerificationCode();
            expirationTime ??= TimeSpan.FromDays(1);
            var cacheOptions = new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = expirationTime };
            _distributedCache.SetString(string.Format(UserEmailModel.EmailVerificationCodeCacheKey, code),
                JsonSerializer.Serialize(new UserEmailModel(userId, email)), cacheOptions);
            return code;
        }

        private static string GenerateEmailVerificationCode(int length = 32)
        {
            // Генерация случайного кода
            byte[] randomNumber = new byte[length];
            RandomNumberGenerator.Fill(randomNumber);

            // Преобразование в Base64 строку
            return Convert.ToBase64String(randomNumber)
                          .Replace("+", string.Empty)
                          .Replace("/", string.Empty)
                          .Replace("=", string.Empty);
        }
    }
}
