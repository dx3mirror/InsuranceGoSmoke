namespace InsuranceGoSmoke.Common.Contracts.Contracts.Paged
{
    /// <summary>
    /// Постраничный список.
    /// </summary>
    public class PagedList<T> where T : class
    {
        /// <summary>
        /// Пустая коллекция.
        /// </summary>
        public static readonly PagedList<T> Empty = new(page: [], total: 0);

        /// <summary>
        /// Создаёт экземпляр <see cref="PagedList{T}"/>.
        /// </summary>
        /// <param name="page">Записи на странице.</param>
        /// <param name="total">Всего записей.</param>
        public PagedList(IReadOnlyCollection<T> page, int total)
        {
            Page = page;
            Total = total;
        }

        /// <summary>
        /// Записи на странице.
        /// </summary>
        public IReadOnlyCollection<T> Page { get; }

        /// <summary>
        /// Всего записей.
        /// </summary>
        public int Total { get; }
    }
}
