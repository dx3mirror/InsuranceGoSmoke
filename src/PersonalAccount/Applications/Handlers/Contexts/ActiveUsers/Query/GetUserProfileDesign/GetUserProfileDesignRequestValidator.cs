using FluentValidation;

namespace InsuranceGoSmoke.PersonalAccount.Applications.Handlers.Contexts.ActiveUsers.Query.GetUserProfileDesign
{
    /// <summary>
    /// Валидатор запроса <see cref="GetUserProfileDesignRequest"/>
    /// </summary>
    public class GetUserProfileDesignRequestValidator : AbstractValidator<GetUserProfileDesignRequest>
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        public GetUserProfileDesignRequestValidator()
        {
            RuleFor(r => r.UserId)
                .GreaterThan(0)
                .WithMessage("Идентификатор пользователя должен быть больше 0");
        }
    }
}
