using InsuranceGoSmoke.Common.Contracts.Contracts.Access.Enums;
using InsuranceGoSmoke.Common.Contracts.Contracts.Authorization;
using InsuranceGoSmoke.Common.Cqrs.Behaviors.AccessValidator;

namespace InsuranceGoSmoke.Security.Applications.Handlers.Contexts.Users.Commands.SetStatusUser
{
    /// <summary>
    /// Валидатор доступа для команды <see cref="SetStatusUserRequest"/>
    /// </summary>
    public class SetStatusUserRequestAccessValidator : RoleAccessValidator<SetStatusUserRequest>
    {
        private readonly IAuthorizationData _authorizationData;

        /// <summary>
        /// Создаёт экземпляр <see cref="SetStatusUserRequestAccessValidator"/>
        /// </summary>
        /// <param name="authorizationData">Данные авторизации.</param>
        public SetStatusUserRequestAccessValidator(
            IAuthorizationData authorizationData)
        {
            _authorizationData = authorizationData;
        }

        /// <inheritdoc/>
        public override Task ValidateAsync(SetStatusUserRequest request, CancellationToken cancellationToken)
        {
            ValidateRole(_authorizationData.Roles, "Заблокировать/Разблокировать пользователя", RoleTypes.Administrator);
            return Task.CompletedTask;
        }
    }
}
