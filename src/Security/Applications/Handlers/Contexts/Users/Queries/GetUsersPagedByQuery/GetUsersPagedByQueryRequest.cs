using InsuranceGoSmoke.Common.Cqrs.Behaviors.Query;
using InsuranceGoSmoke.Security.Applications.Handlers.Contexts.Users.Queries.GetUsersPaged;
using InsuranceGoSmoke.Security.Contracts.Users.Responses;

namespace InsuranceGoSmoke.Security.Applications.Handlers.Contexts.Users.Queries.GetUsersPagedByQuery
{
    /// <summary>
    /// Запрос на получение постраничного списка пользователей.
    /// </summary>
    public class GetUsersPagedByQueryRequest : PagedQuery<UserPagedListItem>
    {
        /// <summary>
        /// Создаёт экземпляр <see cref="GetUsersPagedRequest"/>
        /// </summary>
        /// <param name="query">Фильтр.</param>
        /// <param name="take">Количество записей для получения.</param>
        /// <param name="skip">Количество записей, которое необходимо пропустить.</param>
        public GetUsersPagedByQueryRequest(string? query, int take, int? skip) : base(take, skip)
        {
            Query = query;
        }

        /// <summary>
        /// Запрос.
        /// </summary>
        public string? Query { get; }
    }
}
