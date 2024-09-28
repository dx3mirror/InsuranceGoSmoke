using FluentValidation;
using InsuranceGoSmoke.Common.Cqrs.Behaviors.DataValidator;
using InsuranceGoSmoke.Common.Infrastructures.DataAccess.Configurators;

namespace InsuranceGoSmoke.Security.Applications.Handlers.Contexts.Users.Queries.GetUsersPaged
{
    /// <summary>
    /// Валидатор запроса <see cref="GetUsersPagedRequest"/>
    /// </summary>
    public class GetUsersPagedRequestValidator : PagedRequestValidator<GetUsersPagedRequest>
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        public GetUsersPagedRequestValidator() : base()
        {
            RuleFor(r => r.FirstName)
                .Must(n => string.IsNullOrEmpty(n) || n.Length < PropertyLengthConstants.Length200)
                .WithMessage($"Имя не может быть больше {PropertyLengthConstants.Length200} символов");

            RuleFor(r => r.LastName)
                .Must(n => string.IsNullOrEmpty(n) || n.Length < PropertyLengthConstants.Length200)
                .WithMessage($"Фамилия не может быть больше {PropertyLengthConstants.Length200} символов");

            RuleFor(r => r.Email)
                .Must(e => string.IsNullOrEmpty(e) || e.Length < PropertyLengthConstants.Length200)
                .WithMessage($"Email не может быть больше {PropertyLengthConstants.Length200} символов");

            RuleFor(r => r.PhoneNumber)
                .Must(p => string.IsNullOrEmpty(p) || p.Length < PropertyLengthConstants.Length200)
                .WithMessage($"Номер телефона не может быть больше {PropertyLengthConstants.Length200} символов");
        }
    }
}
