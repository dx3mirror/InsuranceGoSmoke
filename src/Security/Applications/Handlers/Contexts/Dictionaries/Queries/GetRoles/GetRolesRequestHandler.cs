using InsuranceGoSmoke.Common.Applications.Handlers.Abstract;
using InsuranceGoSmoke.Common.Contracts.Contracts.Access.Enums;
using InsuranceGoSmoke.Common.Contracts.Utilities.Helpers.Access;
using InsuranceGoSmoke.Security.Contracts.Users.Responses;

namespace InsuranceGoSmoke.Security.Applications.Handlers.Contexts.Dictionaries.Queries.GetRoles
{
    /// <summary>
    /// Обработчик команды <see cref="GetRolesRequest"/>
    /// </summary>
    public class GetRolesRequestHandler : IQueryHandler<GetRolesRequest, IReadOnlyCollection<RoleResponse>>
    {
        /// <inheritdoc/>
        public Task<IReadOnlyCollection<RoleResponse>> Handle(GetRolesRequest request, CancellationToken cancellationToken)
        {
            var roles = Enum.GetValues<RoleTypes>()
                            .Where(r => r != RoleTypes.Undefined)
                            .Select(r => new RoleResponse(r, RoleTypeHelper.GetRoleName(r)))
                            .ToArray();
            return Task.FromResult<IReadOnlyCollection<RoleResponse>>(roles!);
        }
    }
}
