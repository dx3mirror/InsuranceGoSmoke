using InsuranceGoSmoke.Common.Contracts.Exceptions.Common;
using InsuranceGoSmoke.Common.Contracts.Utilities.Helpers;
using InsuranceGoSmoke.Notification.Applications.AppServices.Contexts.Templates.Services.Abstract;
using InsuranceGoSmoke.Notification.Contracts.Attributes;
using InsuranceGoSmoke.Notification.Contracts.Notifications.Enums;
using InsuranceGoSmoke.Static.Clients;
using Mustache;

namespace InsuranceGoSmoke.Notification.Applications.AppServices.Contexts.Templates.Services
{
    /// <inheritdoc/>
    public class TemplateService : ITemplateService
    {
        private readonly IStaticTemplateClient _staticTemplateClient;

        /// <summary>
        /// Создаёт экземпляр <see cref="TemplateService"/>.
        /// </summary>
        /// <param name="staticTemplateClient">Клиент для работы с шаблонами.</param>
        public TemplateService(IStaticTemplateClient staticTemplateClient)
        {
            _staticTemplateClient = staticTemplateClient;
        }

        /// <inheritdoc/>
        public async Task<string> GetMessageByTemplateAsync(NotificationType templateType, IReadOnlyDictionary<string, string> data, CancellationToken cancellationToken)
        {
            var template = await DownloadTemplateAsync(templateType, cancellationToken);
            string message = Template.Compile(template).Render(data);
            return message;
        }

        private async Task<string> DownloadTemplateAsync(NotificationType notificationType, CancellationToken cancellationToken)
        {
            var attribute = EnumHelper.GetEnumAttribuite<NotificationType, TemplateNameAttribute>(notificationType);
            var templateName = attribute?.TemplateName;
            if (string.IsNullOrEmpty(templateName))
            {
                throw new ArgumentException($"Не удалось получить название шаблона для уведомления: {notificationType}");
            }
            return await _staticTemplateClient.DownloadTemplateAsync(templateName, cancellationToken);
        }
    }
}
