using FluentValidation;

namespace InsuranceGoSmoke.PersonalAccount.Applications.Handlers.Contexts.ActiveUsers.Query.GetUserAvatar
{
    /// <summary>
    /// Валидатор запроса <see cref="GetUserAvatarRequest"/>
    /// </summary>
    public class GetUserAvatarRequestValidator : AbstractValidator<GetUserAvatarRequest>
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        public GetUserAvatarRequestValidator()
        {
            RuleFor(r => r.UserId)
                .GreaterThan(0)
                .WithMessage("Идентификатор пользователя должен быть больше 0");
        }
    }
}
