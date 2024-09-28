using InsuranceGoSmoke.Common.Contracts.Abstract;
using InsuranceGoSmoke.Common.Contracts.Contracts.Paged;

namespace InsuranceGoSmoke.Common.Cqrs.Behaviors.Query
{
    /// <summary>
    /// Запрос с поддержкой пагинации.
    /// </summary>
    /// <typeparam name="T">Тип ответа.</typeparam>
    public class PagedQuery<T> : Query<PagedList<T>>, IPagedQuery
        where T : class
    {
        /// <summary>
        /// Создаёт экземпляр.
        /// </summary>
        /// <param name="take">Количество записей для получения.</param>
        /// <param name="skip">Количество записей, которое необходимо пропустить.</param>
        public PagedQuery(int take, int? skip)
        {
            Take = take;
            Skip = skip;
        }

        /// <inheritdoc/>
        public int Take { get; set; }

        /// <inheritdoc/>
        public int? Skip { get; set; }
    }
}
