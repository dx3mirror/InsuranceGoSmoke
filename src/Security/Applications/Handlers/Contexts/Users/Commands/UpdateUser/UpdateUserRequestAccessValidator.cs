using InsuranceGoSmoke.Common.Contracts.Contracts.Access.Enums;
using InsuranceGoSmoke.Common.Contracts.Contracts.Authorization;
using InsuranceGoSmoke.Security.Applications.Handlers.Contexts.Abstract;

namespace InsuranceGoSmoke.Security.Applications.Handlers.Contexts.Users.Commands.UpdateUser
{
    /// <summary>
    /// Валидатор доступа для команды <see cref="UpdateUserRequest"/>
    /// </summary>
    public class UpdateUserRequestAccessValidator : CurrentUserOrByRoleAccessValidator<UpdateUserRequest>
    {
        private readonly IAuthorizationData _authorizationData;

        /// <summary>
        /// Создаёт экземпляр <see cref="UpdateUserRequestAccessValidator"/>
        /// </summary>
        /// <param name="authorizationData">Данные авторизации.</param>
        public UpdateUserRequestAccessValidator(IAuthorizationData authorizationData)
        {
            _authorizationData = authorizationData;
        }

        /// <inheritdoc/>
        public override Task ValidateAsync(UpdateUserRequest request, CancellationToken cancellationToken)
        {
            Validate(_authorizationData.UserId, _authorizationData.Roles, request, "Редактирование данных пользователя", RoleTypes.Administrator);
            return Task.CompletedTask;
        }
    }
}
