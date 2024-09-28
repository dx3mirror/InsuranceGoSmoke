using InsuranceGoSmoke.Common.Contracts.Abstract;
using InsuranceGoSmoke.Security.Contracts.Identify.Responses;

namespace InsuranceGoSmoke.Security.Applications.Handlers.Contexts.Identify.Queries.GetAuthorizationConfig
{
    /// <summary>
    /// Запрос на получение конфигурации авторизации.
    /// </summary>
    public class GetAuthorizationConfigRequest : Query<IdentifyConfiguration>
    {
    }
}
