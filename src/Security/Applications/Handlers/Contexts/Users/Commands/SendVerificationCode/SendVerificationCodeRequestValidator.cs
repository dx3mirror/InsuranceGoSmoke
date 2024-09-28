using FluentValidation;
using InsuranceGoSmoke.Security.Applications.Handlers.Contexts.Users.Commands.SendVerificationCode;
using InsuranceGoSmoke.Security.Contracts.Common;

namespace InsuranceGoSmoke.Security.Applications.Handlers.Contexts.Users.Commands.ChangePhoneNumber
{
    /// <summary>
    /// Валидатор <see cref="SendVerificationCodeRequest"/>
    /// </summary>
    internal class SendVerificationCodeRequestValidator : AbstractValidator<SendVerificationCodeRequest>
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        public SendVerificationCodeRequestValidator()
        {
            RuleFor(r => r.UserId)
                .NotEmpty()
                .WithMessage("Идентификатор пользователя не может пустым");

            RuleFor(r => r.PhoneNumber)
                .NotEmpty()
                .NotNull()
                .Matches(ValidationConstants.ValidPhoneNumberPattern)
                .WithMessage("Номер телефона не может пустым");
        }
    }
}
