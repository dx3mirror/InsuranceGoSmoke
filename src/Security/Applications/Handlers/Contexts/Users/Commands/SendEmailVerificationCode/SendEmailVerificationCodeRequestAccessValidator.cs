using InsuranceGoSmoke.Common.Contracts.Contracts.Access.Enums;
using InsuranceGoSmoke.Common.Contracts.Contracts.Authorization;
using InsuranceGoSmoke.Security.Applications.Handlers.Contexts.Abstract;
using InsuranceGoSmoke.Security.Applications.Handlers.Contexts.Users.Commands.SendVerificationCode;

namespace InsuranceGoSmoke.Security.Applications.Handlers.Contexts.Users.Commands.SendEmailVerificationCode
{
    /// <summary>
    /// Валидатор доступа для команды <see cref="SendEmailVerificationCodeRequest"/>
    /// </summary>
    public class SendEmailVerificationCodeRequestAccessValidator : CurrentUserOrByRoleAccessValidator<SendEmailVerificationCodeRequest>
    {
        private readonly IAuthorizationData _authorizationData;

        /// <summary>
        /// Создаёт экземпляр <see cref="SendVerificationCodeRequestAccessValidator"/>
        /// </summary>
        /// <param name="authorizationData">Данные авторизации.</param>
        public SendEmailVerificationCodeRequestAccessValidator(IAuthorizationData authorizationData)
        {
            _authorizationData = authorizationData;
        }

        /// <inheritdoc/>
        public override Task ValidateAsync(SendEmailVerificationCodeRequest request, CancellationToken cancellationToken)
        {
            Validate(_authorizationData.UserId, _authorizationData.Roles, request, "Отправка кода подтверждения Email", RoleTypes.Administrator);
            return Task.CompletedTask;
        }
    }
}
