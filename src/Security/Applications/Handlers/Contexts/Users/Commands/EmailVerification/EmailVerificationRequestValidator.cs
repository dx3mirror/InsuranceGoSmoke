using FluentValidation;

namespace InsuranceGoSmoke.Security.Applications.Handlers.Contexts.Users.Commands.EmailVerification
{
    /// <summary>
    /// Валидатор команды <see cref="EmailVerificationRequest"/>
    /// </summary>
    public class EmailVerificationRequestValidator : AbstractValidator<EmailVerificationRequest>
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        public EmailVerificationRequestValidator()
        {
            RuleFor(r => r.Code)
                .NotEmpty()
                .NotNull()
                .WithMessage("Код не может пустым");
        }
    }
}