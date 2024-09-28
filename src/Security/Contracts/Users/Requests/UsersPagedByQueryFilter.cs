using InsuranceGoSmoke.Common.Contracts.Contracts.Paged;

namespace InsuranceGoSmoke.Security.Contracts.Users.Requests
{
    /// <summary>
    /// Фильтр пользователей по запросу.
    /// </summary>
    public class UsersPagedByQueryFilter : PagedFilter
    {
        /// <summary>
        /// Запрос.
        /// </summary>
        public string? Query { get; set; }
    }
}
