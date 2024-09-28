using InsuranceGoSmoke.Common.Contracts.Contracts.Access.Enums;
using InsuranceGoSmoke.Common.Contracts.Contracts.Authorization;
using InsuranceGoSmoke.Common.Cqrs.Behaviors.AccessValidator;

namespace InsuranceGoSmoke.Security.Applications.Handlers.Contexts.Users.Queries.GetUsersPaged
{
    /// <summary>
    /// Валидатор доступа для команды <see cref="GetUsersPagedRequest"/>
    /// </summary>
    public class GetUsersPagedRequestAccessValidator : RoleAccessValidator<GetUsersPagedRequest>
    {
        private readonly IAuthorizationData _authorizationData;

        /// <summary>
        /// Создаёт экземпляр <see cref="GetUsersPagedRequestAccessValidator"/>
        /// </summary>
        /// <param name="authorizationData">Данные авторизации.</param>
        public GetUsersPagedRequestAccessValidator(
            IAuthorizationData authorizationData)
        {
            _authorizationData = authorizationData;
        }

        /// <inheritdoc/>
        public override Task ValidateAsync(GetUsersPagedRequest request, CancellationToken cancellationToken)
        {
            ValidateRole(_authorizationData.Roles, "Просмотр списка пользователей", RoleTypes.Administrator);
            return Task.CompletedTask;
        }
    }
}
