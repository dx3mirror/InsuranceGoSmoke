using FluentValidation;
using InsuranceGoSmoke.Security.Contracts.Common;

namespace InsuranceGoSmoke.Security.Applications.Handlers.Contexts.Users.Commands.SendEmailVerificationCode
{
    /// <summary>
    /// Валидатор команды <see cref="SendEmailVerificationCodeRequest"/>
    /// </summary>
    public class SendEmailVerificationCodeRequestValidator : AbstractValidator<SendEmailVerificationCodeRequest>
    {
        private readonly static int MaxLength = 200;

        /// <summary>
        /// Конструктор.
        /// </summary>
        public SendEmailVerificationCodeRequestValidator()
        {
            RuleFor(r => r.Email)
                .MaximumLength(MaxLength)
                .WithMessage($"Почта не может превышать {MaxLength} символов")
                .NotEmpty()
                .WithMessage("Почта не может пустой")
                .Matches(ValidationConstants.EmailRegex)
                .WithMessage("Недопустимый формат почты. Пример: example@example.ru");
        }
    }
}
