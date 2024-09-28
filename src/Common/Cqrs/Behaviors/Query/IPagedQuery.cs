namespace InsuranceGoSmoke.Common.Cqrs.Behaviors.Query
{
    /// <summary>
    /// Запрос с поддержкой пагинации.
    /// </summary>
    public interface IPagedQuery
    {
        /// <summary>
        /// Количество записей для получения.
        /// </summary>
        int Take { get; set; }

        /// <summary>
        /// Количество записей, которое необходимо пропустить.
        /// </summary>
        int? Skip { get; set; }
    }
}
