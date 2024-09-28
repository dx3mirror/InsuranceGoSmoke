using FluentValidation;
using InsuranceGoSmoke.Common.Applications.AppServices.Contexts.Common.Services.DateTimeProvider;
using InsuranceGoSmoke.Common.Contracts.Utilities.Helpers;

namespace InsuranceGoSmoke.Security.Applications.Handlers.Contexts.Users.Commands.UpdateUser
{
    /// <summary>
    /// Валидатор команды <see cref="UpdateUserRequest"/>
    /// </summary>
    public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequest>
    {
        private readonly static int MinLength = 2;
        private readonly static int MaxLength = 200;
        private readonly static string EmailRegex = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

        /// <summary>
        /// Конструктор.
        /// </summary>
        public UpdateUserRequestValidator(IDateTimeProvider provider)
        {
            RuleFor(r => r.UserId)
                .NotEmpty()
                .WithMessage("Идентификатор пользователя не может пустым");

            RuleFor(r => r.FirstName)
                .MinimumLength(MinLength)
                .WithMessage($"Имя не может быть меньше {MinLength} символов")
                .MaximumLength(MaxLength)
                .WithMessage($"Имя не может превышать {MaxLength} символов");

            RuleFor(r => r.LastName)
                .MinimumLength(MinLength)
                .WithMessage($"Фамилия не может быть меньше {MinLength} символов")
                .MaximumLength(MaxLength)
                .WithMessage($"Фамилия не может превышать {MaxLength} символов");

            RuleFor(r => r.BirthDate)
                .Must(b => !b.HasValue || b.Value < provider.UtcNow)
                .WithMessage("Дата дня рождения не может быть в будущем");

            RuleFor(r => r.Email)
                .MaximumLength(MaxLength)
                .WithMessage($"Почта не может превышать {MaxLength} символов")
                .Matches(EmailRegex)
                .WithMessage("Недопустимый формат почты. Пример: example@example.ru");
        }
    }
}
