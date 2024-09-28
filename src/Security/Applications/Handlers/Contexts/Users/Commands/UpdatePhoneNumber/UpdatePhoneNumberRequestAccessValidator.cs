using InsuranceGoSmoke.Common.Contracts.Contracts.Access.Enums;
using InsuranceGoSmoke.Common.Contracts.Contracts.Authorization;
using InsuranceGoSmoke.Common.Cqrs.Behaviors.AccessValidator;
using InsuranceGoSmoke.Security.Applications.Handlers.Contexts.Users.Commands.ChangePhoneNumber;
using InsuranceGoSmoke.Security.Applications.Handlers.Contexts.Users.Commands.UpdateUserField;

namespace InsuranceGoSmoke.Security.Applications.Handlers.Contexts.Users.Commands.UpdatePhoneNumber
{
    /// <summary>
    /// Валидатор доступа для команды <see cref="UpdatePhoneNumberRequest"/>
    /// </summary>
    public class UpdatePhoneNumberRequestAccessValidator : RoleAccessValidator<UpdatePhoneNumberRequest>
    {
        private readonly IAuthorizationData _authorizationData;

        /// <summary>
        /// Создаёт экземпляр <see cref="UpdateUserFieldRequestAccessValidator"/>
        /// </summary>
        /// <param name="authorizationData">Данные авторизации.</param>
        public UpdatePhoneNumberRequestAccessValidator(IAuthorizationData authorizationData)
        {
            _authorizationData = authorizationData;
        }

        /// <inheritdoc/>
        public override Task ValidateAsync(UpdatePhoneNumberRequest request, CancellationToken cancellationToken)
        {
            ValidateRole(_authorizationData.Roles, "Редактирование номера телефона пользователя", RoleTypes.Administrator);
            return Task.CompletedTask;
        }
    }
}
