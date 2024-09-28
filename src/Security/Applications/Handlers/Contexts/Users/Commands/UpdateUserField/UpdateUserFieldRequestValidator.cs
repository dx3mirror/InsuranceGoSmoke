using FluentValidation;
using InsuranceGoSmoke.Common.Applications.AppServices.Contexts.Common.Services.DateTimeProvider;
using InsuranceGoSmoke.Common.Clients.Keycloak.Models;
using InsuranceGoSmoke.Common.Contracts.Utilities.Helpers;
using InsuranceGoSmoke.Security.Contracts.Common;
using System.Text.RegularExpressions;

namespace InsuranceGoSmoke.Security.Applications.Handlers.Contexts.Users.Commands.UpdateUserField
{
    /// <summary>
    /// Валидатор команды <see cref="UpdateUserFieldRequest"/>
    /// </summary>
    public class UpdateUserFieldRequestValidator : AbstractValidator<UpdateUserFieldRequest>
    {
        private readonly static int MinLength = 2;
        private readonly static int MaxLength = 200;

        /// <summary>
        /// Конструктор.
        /// </summary>
        public UpdateUserFieldRequestValidator(IDateTimeProvider provider)
        {
            RuleFor(r => r.UserId)
                .NotEmpty()
                .WithMessage("Идентификатор пользователя не может пустым");

            RuleFor(r => r.Field)
                .MinimumLength(MinLength)
                .WithMessage($"Поле не может быть меньше {MinLength} символов")
                .MaximumLength(MaxLength)
                .WithMessage($"Поле не может превышать {MaxLength} символов");

            RuleFor(r => r)
                .Must(r => r.Field.FirstCharToUpper() != nameof(KeycloakUserData.FirstName) || IsLess(r.Value.GetString(), MaxLength))
                .WithMessage($"Имя не может превышать {MaxLength} символов")
                .Must(r => r.Field.FirstCharToUpper() != nameof(KeycloakUserData.FirstName) || IsGreater(r.Value.GetString(), MinLength))
                .WithMessage($"Имя не может быть меньше {MinLength} символов")
                .Must(r => r.Field.FirstCharToUpper() != nameof(KeycloakUserData.LastName) || IsLess(r.Value.GetString(), MaxLength))
                .WithMessage($"Фамилия не может превышать {MaxLength} символов")
                .Must(r => r.Field.FirstCharToUpper() != nameof(KeycloakUserData.LastName) || IsGreater(r.Value.GetString(), MinLength))
                .WithMessage($"Фамилия не может быть меньше {MinLength} символов")
                .Must(r => r.Field.FirstCharToUpper() != nameof(KeycloakUserData.BirthDate) || IsPast(DateTimeHelper.ToDateTime(r.Value.GetString()), provider.UtcNow))
                .WithMessage("Дата дня рождения не может быть в будущем")
                .Must(r => r.Field.FirstCharToUpper() != nameof(KeycloakUserData.Email) || IsEmail(r.Value.GetString()))
                .WithMessage("Недопустимый формат почты. Пример: example@example.ru")
                .WithName("Пользователь");
        }

        private static bool IsLess(string? value, int maxLength)
        {
            if (value is null)
            {
                return true;
            }

            return value.Length <= maxLength;
        }

        private static bool IsGreater(string? value, int minLength)
        {
            if (value is null)
            {
                return false;
            }

            return value.Length > minLength;
        }

        private static bool IsPast(DateTime? value, DateTime date)
        {
            if (value is null)
            {
                return true;
            }

            return value < date;
        }

        private static bool IsEmail(string? value)
        {
            if (value is null)
            {
                return true;
            }

            return Regex.IsMatch(value, ValidationConstants.EmailRegex);
        }
    }
}
