using System.Linq.Expressions;

namespace InsuranceGoSmoke.Common.Applications.AppServices.Specifications.Abstracts
{
    /// <summary>
    /// Интерфейс спецификации, который определяет критерий для фильтрации сущностей.
    /// </summary>
    /// <typeparam name="T">Тип сущности, к которой применяется спецификация.</typeparam>
    public interface ISpecification<T>
    {
        /// <summary>
        /// Лямбда-выражение, которое представляет критерий фильтрации для сущности типа <typeparamref name="T"/>.
        /// </summary>
        Expression<Func<T, bool>> Criteria { get; }
    }
}
