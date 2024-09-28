using InsuranceGoSmoke.Common.Contracts.Contracts.Access.Enums;
using InsuranceGoSmoke.Common.Contracts.Contracts.Authorization;
using InsuranceGoSmoke.Common.Cqrs.Behaviors.AccessValidator;

namespace InsuranceGoSmoke.Security.Applications.Handlers.Contexts.Users.Queries.GetUsersPagedByQuery
{
    /// <summary>
    /// Валидатор доступа для команды <see cref="GetUsersPagedByQueryRequest"/>
    /// </summary>
    public class GetUsersPagedByQueryRequestAccessValidator : RoleAccessValidator<GetUsersPagedByQueryRequest>
    {
        private readonly IAuthorizationData _authorizationData;

        /// <summary>
        /// Создаёт экземпляр <see cref="GetUsersPagedByQueryRequestAccessValidator"/>
        /// </summary>
        /// <param name="authorizationData">Данные авторизации.</param>
        public GetUsersPagedByQueryRequestAccessValidator(
            IAuthorizationData authorizationData)
        {
            _authorizationData = authorizationData;
        }

        /// <inheritdoc/>
        public override Task ValidateAsync(GetUsersPagedByQueryRequest request, CancellationToken cancellationToken)
        {
            ValidateRole(_authorizationData.Roles, "Просмотр списка пользователей", RoleTypes.Administrator);
            return Task.CompletedTask;
        }
    }
}
