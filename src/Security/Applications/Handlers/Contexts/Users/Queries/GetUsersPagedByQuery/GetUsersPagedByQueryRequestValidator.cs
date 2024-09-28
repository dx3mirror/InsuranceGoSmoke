using FluentValidation;
using InsuranceGoSmoke.Common.Cqrs.Behaviors.DataValidator;
using InsuranceGoSmoke.Common.Infrastructures.DataAccess.Configurators;

namespace InsuranceGoSmoke.Security.Applications.Handlers.Contexts.Users.Queries.GetUsersPagedByQuery
{
    /// <summary>
    /// Валидатор запроса <see cref="GetUsersPagedByQueryRequest"/>
    /// </summary>
    public class GetUsersPagedRequestValidator : PagedRequestValidator<GetUsersPagedByQueryRequest>
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        public GetUsersPagedRequestValidator() : base()
        {
            RuleFor(r => r.Query)
                .Must(n => string.IsNullOrEmpty(n) || n.Length < PropertyLengthConstants.Length200)
                .WithMessage($"Запрос не может быть больше {PropertyLengthConstants.Length200} символов");
        }
    }
}
