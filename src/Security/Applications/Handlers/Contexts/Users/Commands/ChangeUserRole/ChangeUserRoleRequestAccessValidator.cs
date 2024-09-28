using InsuranceGoSmoke.Common.Contracts.Contracts.Access.Enums;
using InsuranceGoSmoke.Common.Contracts.Contracts.Authorization;
using InsuranceGoSmoke.Common.Contracts.Exceptions.Authorization;
using InsuranceGoSmoke.Common.Cqrs.Behaviors.AccessValidator;

namespace InsuranceGoSmoke.Security.Applications.Handlers.Contexts.Users.Commands.ChangeUserRole
{
    /// <summary>
    /// Валидатор доступа команды <see cref="ChangeUserRoleRequest"/>
    /// </summary>
    public class ChangeUserRoleRequestAccessValidator : RoleAccessValidator<ChangeUserRoleRequest>
    {
        private readonly IAuthorizationData _authorizationData;

        /// <summary>
        /// Создаёт экземпляр <see cref="ChangeUserRoleRequestAccessValidator"/>
        /// </summary>
        /// <param name="authorizationData">Данные авторизации.</param>
        public ChangeUserRoleRequestAccessValidator(
            IAuthorizationData authorizationData)
        {
            _authorizationData = authorizationData;
        }

        /// <inheritdoc/>
        public override Task ValidateAsync(ChangeUserRoleRequest request, CancellationToken cancellationToken)
        {
            ValidateRole(_authorizationData.Roles, "Изменение роли пользователя", RoleTypes.Administrator);
            
            if (_authorizationData.UserId == request.UserId)
            {
                throw new AccessDeniedException("Изменять собыственную роль запрещено.");
            }

            return Task.CompletedTask;
        }
    }
}
