using InsuranceGoSmoke.Common.Contracts;
using InsuranceGoSmoke.Notification.Clients.Notifications;
using InsuranceGoSmoke.Notification.Clients.Options;
using Microsoft.Extensions.DependencyInjection;

namespace InsuranceGoSmoke.Notification.Clients
{
    /// <summary>
    /// Регистрация клиентов.
    /// </summary>
    public static class Registrar
    {
        /// <inheritdoc/>
        public static IServiceCollection AddNotificationServiceClient(this IServiceCollection services)
        {
            services
                .AddConfigurationOptions<NotificationClientOptions>()
                .AddHttpClient<INotificationServiceClient, NotificaitonServiceClient>();

            return services;
        }
    }
}
