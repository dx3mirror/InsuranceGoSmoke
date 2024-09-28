using InsuranceGoSmoke.Common.Contracts.Contracts.Access.Enums;
using InsuranceGoSmoke.Common.Contracts.Contracts.Authorization;
using InsuranceGoSmoke.Security.Applications.Handlers.Contexts.Abstract;

namespace InsuranceGoSmoke.Security.Applications.Handlers.Contexts.Users.Queries.GetUser
{
    /// <summary>
    /// Валидатор доступа для команды <see cref="GetUserRequest"/>
    /// </summary>
    public class GetUserRequestAccessValidator : CurrentUserOrByRoleAccessValidator<GetUserRequest>
    {
        private readonly IAuthorizationData _authorizationData;

        /// <summary>
        /// Создаёт экземпляр <see cref="GetUserRequestAccessValidator"/>
        /// </summary>
        /// <param name="authorizationData">Данные авторизации.</param>
        public GetUserRequestAccessValidator(IAuthorizationData authorizationData)
        {
            _authorizationData = authorizationData;
        }

        /// <inheritdoc/>
        public override Task ValidateAsync(GetUserRequest request, CancellationToken cancellationToken)
        {
            Validate(_authorizationData.UserId, _authorizationData.Roles, request, "Просмотр данных пользователя", RoleTypes.Administrator);
            return Task.CompletedTask;
        }
    }
}
