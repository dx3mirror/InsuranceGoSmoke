using InsuranceGoSmoke.Security.Contracts.Identify.Responses;

namespace InsuranceGoSmoke.Security.Applications.AppServices.Contexts.Identify.Services.Abstract
{
    /// <summary>
    /// Сервис конфигурации.
    /// </summary>
    public interface IConfigurationService
    {
        /// <summary>
        /// Возвращает конфигурацию авторизации.
        /// </summary>
        /// <returns>Конфигурация авторизации.</returns>
        IdentifyConfiguration GetConfig();
    }
}
