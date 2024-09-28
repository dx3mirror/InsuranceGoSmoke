using FluentValidation;
using InsuranceGoSmoke.Common.Cqrs.Behaviors.Query;

namespace InsuranceGoSmoke.Common.Cqrs.Behaviors.DataValidator
{
    /// <summary>
    /// Валидатор для запросов на постраничные списки.
    /// </summary>
    /// <typeparam name="T">Тип запроса.</typeparam>
    public abstract class PagedRequestValidator<T> : AbstractValidator<T> where T : IPagedQuery
    {
        /// <summary>
        /// Минимальное значение.
        /// </summary>
        protected virtual int MinValue { get; set; } = 0;

        /// <summary>
        /// Максимальное значение.
        /// </summary>
        protected virtual int MaxValue { get; set; } = 100;

        /// <summary>
        /// Конструктор.
        /// </summary>
        protected PagedRequestValidator()
        {
            RuleFor(r => r.Take)
                .GreaterThan(MinValue)
                .WithMessage($"Значение должно быть больше {MinValue}")
                .LessThanOrEqualTo(MaxValue)
                .WithMessage($"Значение не может быть больше {MaxValue}");

            RuleFor(r => r.Skip)
                .GreaterThanOrEqualTo(MinValue)
                .WithMessage($"Значение должно быть больше либо равно {MinValue}");
        }
    }
}
