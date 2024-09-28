using FluentValidation;

namespace InsuranceGoSmoke.Security.Applications.Handlers.Contexts.Users.Commands.ChangeUserRole
{
    /// <summary>
    /// Валидатор команды <see cref="ChangeUserRoleRequest"/>
    /// </summary>
    public class ChangeUserRoleRequestValidator : AbstractValidator<ChangeUserRoleRequest>
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        public ChangeUserRoleRequestValidator()
        {
            RuleFor(r => r.UserId)
                .NotEmpty()
                .WithMessage("Идентификатор пользователя не может пустым");

            RuleFor(r => r.Role)
                .IsInEnum()
                .WithMessage("Неопределенный тип роли");
        }
    }
}
