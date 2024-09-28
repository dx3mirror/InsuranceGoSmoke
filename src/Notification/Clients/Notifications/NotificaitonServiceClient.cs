using InsuranceGoSmoke.Common.Clients;
using InsuranceGoSmoke.Common.Contracts.Options;
using InsuranceGoSmoke.Notification.Clients.Options;
using InsuranceGoSmoke.Notification.Contracts.Notifications.Requests;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;

namespace InsuranceGoSmoke.Notification.Clients.Notifications
{
    /// <inheritdoc/>
    public class NotificaitonServiceClient : BaseApiClient, INotificationServiceClient
    {
        private readonly NotificationClientOptions _options;

        /// <inheritdoc />
        protected override AuthenticationHeaderValue CreateAuthorizationHeader()
            => AuthenticationHeaderValue.Parse($"{TrustedNetworkOptions.Scheme} {TrustedNetworkOptions.System}");

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="httpClient">Клиент.</param>
        /// <param name="options">Настройки.</param>
        public NotificaitonServiceClient(HttpClient httpClient, IOptions<NotificationClientOptions> options) : base(httpClient)
        {
            _options = options.Value;
        }

        /// <inheritdoc/>
        public async Task SendToEmailAsync(EmailNotificationData data, CancellationToken cancellationToken)
        {
            var url = $"{_options.Url}/notification/email/send";

            await PostAsync(url, data, cancellationToken);
        }
    }
}
