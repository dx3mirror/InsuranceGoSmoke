using FluentValidation;

namespace InsuranceGoSmoke.PersonalAccount.Applications.Handlers.Contexts.ActiveUsers.Query.GetActiveUsersMainInformation
{
    /// <summary>
    /// Валидатор запроса <see cref="GetActiveUserMainInformationRequest"/>
    /// </summary>
    public class GetActiveUserMainInformationRequestValidator : AbstractValidator<GetActiveUserMainInformationRequest>
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        public GetActiveUserMainInformationRequestValidator()
        {
            RuleFor(r => r.UserId)
                .GreaterThan(0)
                .WithMessage("Идентификатор пользователя должен быть больше 0");
        }
    }
}
