using FluentValidation;

namespace InsuranceGoSmoke.Notification.Applications.Handlers.Contexts.Notifications.Commands.SendEmail
{
    /// <summary>
    /// Валидатор запроса <see cref="SendEmailRequest"/>
    /// </summary>
    public class SendEmailRequestValidator : AbstractValidator<SendEmailRequest>
    {
        private readonly static int MaxLength = 100;

        /// <summary>
        /// Конструктор.
        /// </summary>
        public SendEmailRequestValidator()
        {
            RuleFor(r => r.Email)
                .MaximumLength(MaxLength)
                .WithMessage($"Email не может превышать {MaxLength} символов")
                .NotEmpty()
                .WithMessage($"Email не может пустым");
        }
    }
}
