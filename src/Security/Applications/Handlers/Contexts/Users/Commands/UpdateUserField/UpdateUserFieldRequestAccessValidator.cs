using InsuranceGoSmoke.Common.Contracts.Contracts.Access.Enums;
using InsuranceGoSmoke.Common.Contracts.Contracts.Authorization;
using InsuranceGoSmoke.Security.Applications.Handlers.Contexts.Abstract;

namespace InsuranceGoSmoke.Security.Applications.Handlers.Contexts.Users.Commands.UpdateUserField
{
    /// <summary>
    /// Валидатор доступа для команды <see cref="UpdateUserFieldRequest"/>
    /// </summary>
    public class UpdateUserFieldRequestAccessValidator : CurrentUserOrByRoleAccessValidator<UpdateUserFieldRequest>
    {
        private readonly IAuthorizationData _authorizationData;

        /// <summary>
        /// Создаёт экземпляр <see cref="UpdateUserFieldRequestAccessValidator"/>
        /// </summary>
        /// <param name="authorizationData">Данные авторизации.</param>
        public UpdateUserFieldRequestAccessValidator(IAuthorizationData authorizationData)
        {
            _authorizationData = authorizationData;
        }

        /// <inheritdoc/>
        public override Task ValidateAsync(UpdateUserFieldRequest request, CancellationToken cancellationToken)
        {
            Validate(_authorizationData.UserId, _authorizationData.Roles, request, "Редактирование данных пользователя", RoleTypes.Administrator);
            return Task.CompletedTask;
        }
    }
}
