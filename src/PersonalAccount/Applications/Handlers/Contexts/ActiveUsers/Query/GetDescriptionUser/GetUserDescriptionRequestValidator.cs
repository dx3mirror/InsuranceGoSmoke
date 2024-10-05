using FluentValidation;

namespace InsuranceGoSmoke.PersonalAccount.Applications.Handlers.Contexts.ActiveUsers.Query.GetDescriptionUser
{
    /// <summary>
    /// Валидатор запроса <see cref="GetUserDescriptionRequest"/>
    /// </summary>
    public class GetUserDescriptionRequestValidator : AbstractValidator<GetUserDescriptionRequest>
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        public GetUserDescriptionRequestValidator()
        {
            RuleFor(r => r.UserId)
                .GreaterThan(0)
                .WithMessage("Идентификатор пользователя должен быть больше 0");
        }
    }
}
