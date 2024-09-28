using FluentValidation;
using Microsoft.AspNetCore.Http;
using System.Net;

namespace InsuranceGoSmoke.Security.Applications.Handlers.Contexts.Identify.Queries.RefreshToken
{
    /// <summary>
    /// Валидатор запроса <see cref="RefreshTokenQuery"/>
    /// </summary>
    public class RefreshTokenQueryValidator : AbstractValidator<RefreshTokenQuery>
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        public RefreshTokenQueryValidator()
        {
            RuleFor(r => r.RefreshToken)
                .NotEmpty()
                .NotNull()
                .WithMessage($"Токен не может пустым")
                .WithErrorCode(StatusCodes.Status401Unauthorized.ToString());
        }
    }
}
