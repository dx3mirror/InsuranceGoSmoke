using InsuranceGoSmoke.Common.Hosts.Features.AppFeatures.Authorization.Instances.TrustedNetwork.Models;
using Microsoft.AspNetCore.Http;

namespace InsuranceGoSmoke.Common.Hosts.Features.AppFeatures.Authorization.Instances.TrustedNetwork
{
    /// <summary>
    /// Валидация сети на доверие.
    /// </summary>
    public interface ITrustedNetworkValidator
    {
        /// <summary>
        /// Проверяет запрос на то, что он пришел из доверенной сети.
        /// </summary>
        /// <param name="request">Запрос.</param>
        /// <returns>Результат валидации.</returns>
        TrustedNetworkValidationResult Validate(HttpRequest request);
    }
}
