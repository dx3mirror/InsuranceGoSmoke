using InsuranceGoSmoke.Common.Contracts.Abstract;
using InsuranceGoSmoke.Security.Contracts.Users.Responses;

namespace InsuranceGoSmoke.Security.Applications.Handlers.Contexts.Dictionaries.Queries.GetRoles
{
    /// <summary>
    /// Запрос на получение ролей.
    /// </summary>
    public class GetRolesRequest : Query<IReadOnlyCollection<RoleResponse>>
    {
    }
}
