using FluentValidation;

namespace InsuranceGoSmoke.Security.Applications.Handlers.Contexts.Users.Queries.GetUser
{
    /// <summary>
    /// Валидатор запроса <see cref="GetUserRequest"/>
    /// </summary>
    public class GetUserRequestValidator : AbstractValidator<GetUserRequest>
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        public GetUserRequestValidator()
        {
            RuleFor(r => r.UserId)
                .NotEmpty()
                .WithMessage("Идентификатор пользователя не может пустым");
        }
    }
}
