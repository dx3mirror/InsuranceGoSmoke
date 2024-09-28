using FluentValidation;
using InsuranceGoSmoke.Security.Contracts.Common;

namespace InsuranceGoSmoke.Security.Applications.Handlers.Contexts.Users.Commands.ChangePhoneNumber
{
    /// <summary>
    /// Валидатор <see cref="UpdatePhoneNumberRequest"/>
    /// </summary>
    internal class UpdatePhoneNumberRequestValidator : AbstractValidator<UpdatePhoneNumberRequest>
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        public UpdatePhoneNumberRequestValidator()
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
