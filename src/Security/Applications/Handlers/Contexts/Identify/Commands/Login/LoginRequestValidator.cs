using FluentValidation;

namespace InsuranceGoSmoke.Security.Applications.Handlers.Contexts.Identify.Commands.Login
{
    /// <summary>
    /// Валидатор запроса <see cref="LoginRequest"/>
    /// </summary>
    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        private readonly static int MaxLength = 200;

        /// <summary>
        /// Конструктор.
        /// </summary>
        public LoginRequestValidator()
        {
            RuleFor(r => r.UserName)
                .MaximumLength(MaxLength)
                .WithMessage($"Логин не может превышать {MaxLength} символов")
                .NotEmpty()
                .WithMessage($"Логин не может пустым");

            RuleFor(r => r.Password)
                .MaximumLength(MaxLength)
                .WithMessage($"Пароль не может превышать {MaxLength} символов")
                .NotEmpty()
                .WithMessage($"Пароль не может пустым");
        }
    }
}
