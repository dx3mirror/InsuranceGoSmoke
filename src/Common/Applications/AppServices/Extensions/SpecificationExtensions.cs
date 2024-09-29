using InsuranceGoSmoke.Common.Applications.AppServices.Specifications;
using InsuranceGoSmoke.Common.Applications.AppServices.Specifications.Abstracts;
using InsuranceGoSmoke.Common.Applications.AppServices.Visitors;
using System.Linq.Expressions;

namespace InsuranceGoSmoke.Common.Applications.AppServices.Extensions
{
    /// <summary>
    /// Класс расширений для работы с IQueryable с использованием спецификаций.
    /// </summary>
    public static class SpecificationExtensions
    {
        /// <summary>
        /// Применяет спецификацию для фильтрации данных в запросе <see cref="IQueryable{T}"/>.
        /// </summary>
        /// <typeparam name="T">Тип сущности, к которой применяется спецификация.</typeparam>
        /// <param name="query">Запрос <see cref="IQueryable{T}"/>, к которому применяется фильтр.</param>
        /// <param name="specification">Спецификация, определяющая критерий фильтрации.</param>
        /// <returns>Отфильтрованный запрос <see cref="IQueryable{T}"/>.</returns>
        public static IQueryable<T> Where<T>(this IQueryable<T> query, ISpecification<T> specification)
        {
            return query.Where(specification.Criteria);
        }

        /// <summary>
        /// Объединяет два условия спецификации с помощью логического оператора И (AND).
        /// </summary>
        /// <typeparam name="T">Тип объекта, к которому применяется спецификация.</typeparam>
        /// <param name="first">Первая спецификация.</param>
        /// <param name="second">Вторая спецификация.</param>
        /// <returns>Новая спецификация, которая является логическим И (AND) первой и второй спецификаций.</returns>
        public static ISpecification<T> And<T>(this ISpecification<T> first, ISpecification<T> second)
        {
            return new Specification<T>(first.Criteria.AndAlso(second.Criteria));
        }

        /// <summary>
        /// Объединяет два выражения с помощью логического оператора И (AND) и создаёт новое выражение.
        /// </summary>
        /// <typeparam name="T">Тип объекта, к которому применяется выражение.</typeparam>
        /// <param name="first">Первое выражение.</param>
        /// <param name="second">Второе выражение.</param>
        /// <returns>Новое выражение, представляющее логическое И (AND) двух выражений.</returns>
        private static Expression<Func<T, bool>> AndAlso<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            // Создаём новый параметр для объединённого выражения
            var parameter = Expression.Parameter(typeof(T));

            // Заменяем параметр в первом выражении на новый параметр
            var combined = new ReplaceExpressionVisitor(first.Parameters[0], parameter)
                .Visit(first.Body);

            // Заменяем параметр во втором выражении на новый параметр
            var secondBody = new ReplaceExpressionVisitor(second.Parameters[0], parameter)
                .Visit(second.Body);

            // Возвращаем новое выражение с использованием логического И (AND)
            return Expression.Lambda<Func<T, bool>>(Expression.AndAlso(combined, secondBody), parameter);
        }

    }
}
