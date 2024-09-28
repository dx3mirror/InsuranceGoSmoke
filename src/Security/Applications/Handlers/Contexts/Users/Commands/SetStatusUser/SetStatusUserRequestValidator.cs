using FluentValidation;

namespace InsuranceGoSmoke.Security.Applications.Handlers.Contexts.Users.Commands.SetStatusUser
{
    /// <summary>
    /// Валидатор команды <see cref="SetStatusUserRequest"/>
    /// </summary>
    public class SetStatusUserRequestValidator : AbstractValidator<SetStatusUserRequest>
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        public SetStatusUserRequestValidator()
        {
            RuleFor(r => r.UserId)
                .NotEmpty()
                .WithMessage("Идентификатор пользователя не может пустым");

            RuleFor(r => r.IsEnabled)
                .NotNull()
                .WithMessage("Статус пользователя не может быть неопределенным");
        }
    }
}
