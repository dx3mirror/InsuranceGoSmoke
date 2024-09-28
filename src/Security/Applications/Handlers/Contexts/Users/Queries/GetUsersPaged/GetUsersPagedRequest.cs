using InsuranceGoSmoke.Common.Contracts.Contracts.Access.Enums;
using InsuranceGoSmoke.Common.Cqrs.Behaviors.Query;
using InsuranceGoSmoke.Security.Contracts.Users.Requests;
using InsuranceGoSmoke.Security.Contracts.Users.Responses;

namespace InsuranceGoSmoke.Security.Applications.Handlers.Contexts.Users.Queries.GetUsersPaged
{
    /// <summary>
    /// Запрос на получение постраничного списка пользователей.
    /// </summary>
    public class GetUsersPagedRequest : PagedQuery<UserPagedListItem>
    {
        /// <summary>
        /// Создаёт экземпляр <see cref="GetUsersPagedRequest"/>
        /// </summary>
        /// <param name="filter">Фильтр.</param>
        public GetUsersPagedRequest(UsersPagedFilter filter) : base(filter.Take, filter.Skip)
        {
            FirstName = filter.FirstName;
            LastName = filter.LastName;
            PhoneNumber = filter.PhoneNumber;
            Role = filter.Role ?? [];
            Email = filter.Email;
            IsEnabled = filter.IsEnabled;
        }

        /// <summary>
        /// Имя.
        /// </summary>
        public string? FirstName { get; }

        /// <summary>
        /// Фамиоия.
        /// </summary>
        public string? LastName { get; }

        /// <summary>
        /// Номер телефона.
        /// </summary>
        public string? PhoneNumber { get; }

        /// <summary>
        /// Роли.
        /// </summary>
        public RoleTypes[] Role { get; }

        /// <summary>
        /// Email.
        /// </summary>
        public string? Email { get; }

        /// <summary>
        /// Признак, что пользователь активен.
        /// </summary>
        public bool? IsEnabled { get; }
    }
}
