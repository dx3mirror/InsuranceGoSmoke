using InsuranceGoSmoke.Notification.Contracts.Notifications.Enums;

namespace InsuranceGoSmoke.Notification.Applications.AppServices.Contexts.Templates.Services.Abstract
{
    /// <summary>
    /// Сервис для работы с шаблонами.
    /// </summary>
    public interface ITemplateService
    {
        /// <summary>
        /// Возвращает сгенерированное сообщение на основе шаблона.
        /// </summary>
        /// <param name="templateType">Тип шаблона.</param>
        /// <param name="data">Данные.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Сообщение.</returns>
        Task<string> GetMessageByTemplateAsync(NotificationType templateType, IReadOnlyDictionary<string, string> data, CancellationToken cancellationToken);
    }
}
