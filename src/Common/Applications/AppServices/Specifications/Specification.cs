using InsuranceGoSmoke.Common.Applications.AppServices.Specifications.Abstracts;
using System.Linq.Expressions;

namespace InsuranceGoSmoke.Common.Applications.AppServices.Specifications
{
    /// <summary>
    /// Базовая реализация спецификации, содержащая критерий фильтрации.
    /// </summary>
    /// <typeparam name="T">Тип сущности, к которой применяется спецификация.</typeparam>
    public class Specification<T> : ISpecification<T>
    {
        /// <summary>
        /// Лямбда-выражение, представляющее критерий фильтрации для сущности типа <typeparamref name="T"/>.
        /// </summary>
        public Expression<Func<T, bool>> Criteria { get; }

        /// <summary>
        /// Создает экземпляр спецификации с переданным критерием.
        /// </summary>
        /// <param name="criteria">Лямбда-выражение, представляющее критерий фильтрации.</param>
        public Specification(Expression<Func<T, bool>> criteria)
        {
            Criteria = criteria;
        }
    }

}
