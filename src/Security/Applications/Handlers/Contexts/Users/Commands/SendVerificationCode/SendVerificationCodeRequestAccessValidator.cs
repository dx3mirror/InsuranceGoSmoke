using InsuranceGoSmoke.Common.Contracts.Contracts.Access.Enums;
using InsuranceGoSmoke.Common.Contracts.Contracts.Authorization;
using InsuranceGoSmoke.Security.Applications.Handlers.Contexts.Abstract;

namespace InsuranceGoSmoke.Security.Applications.Handlers.Contexts.Users.Commands.SendVerificationCode
{
    /// <summary>
    /// Валидатор доступа для команды <see cref="SendVerificationCodeRequest"/>
    /// </summary>
    public class SendVerificationCodeRequestAccessValidator : CurrentUserOrByRoleAccessValidator<SendVerificationCodeRequest>
    {
        private readonly IAuthorizationData _authorizationData;

        /// <summary>
        /// Создаёт экземпляр <see cref="SendVerificationCodeRequestAccessValidator"/>
        /// </summary>
        /// <param name="authorizationData">Данные авторизации.</param>
        public SendVerificationCodeRequestAccessValidator(IAuthorizationData authorizationData)
        {
            _authorizationData = authorizationData;
        }

        /// <inheritdoc/>
        public override Task ValidateAsync(SendVerificationCodeRequest request, CancellationToken cancellationToken)
        {
            Validate(_authorizationData.UserId, _authorizationData.Roles, request, "Отправка кода подтверждения номера телефона", RoleTypes.Administrator);
            return Task.CompletedTask;
        }
    }
}
