namespace InsuranceGoSmoke.Common.Contracts.Contracts.Paged
{
    /// <summary>
    /// Фильтр постраничного списка.
    /// </summary>
    public class PagedFilter
    {
        /// <summary>
        /// Возврщаемое количество записей.
        /// </summary>
        public int Take { get; set; }

        /// <summary>
        /// Пропускаемое количество записей.
        /// </summary>
        public int? Skip { get; set; }

        /// <summary>
        /// Создаёт экземпляр <see cref="PagedFilter"/>.
        /// </summary>
        public PagedFilter()
        {
        }

        /// <summary>
        /// Создаёт экземпляр <see cref="PagedFilter"/>.
        /// </summary>
        /// <param name="take">Возврщаемое количество записей.</param>
        /// <param name="skip">Пропускаемое количество записей.</param>
        public PagedFilter(int take, int? skip) : this()
        {
            Take = take;
            Skip = skip;
        }
    }
}
