using FluentValidation;

namespace InsuranceGoSmoke.PersonalAccount.Applications.Handlers.Contexts.ActiveUsers.Query.GetRulePrivacyUser
{
    /// <summary>
    /// Валидатор запроса <see cref="GetUserPrivacySettingsRequest"/>
    /// </summary>
    public class GetUserPrivacySettingsRequestValidator : AbstractValidator<GetUserPrivacySettingsRequest>
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        public GetUserPrivacySettingsRequestValidator()
        {
            RuleFor(r => r.UserId)
                .GreaterThan(0)
                .WithMessage("Идентификатор пользователя должен быть больше 0");
        }
    }
}
