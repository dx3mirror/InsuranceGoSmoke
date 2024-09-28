using InsuranceGoSmoke.Common.Contracts.Contracts.Access.Enums;
using InsuranceGoSmoke.Common.Contracts.Contracts.Paged;

namespace InsuranceGoSmoke.Security.Contracts.Users.Requests
{
    /// <summary>
    /// Запрос на получение постраничного списка пользователей.
    /// </summary>
    public class UsersPagedFilter : PagedFilter
    {
        /// <summary>
        /// Имя.
        /// </summary>
        public string? FirstName { get; set; }

        /// <summary>
        /// Фамиоия.
        /// </summary>
        public string? LastName { get; set; }

        /// <summary>
        /// Номер телефона.
        /// </summary>
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// Роли.
        /// </summary>
        public RoleTypes[] Role { get; set; } = [];

        /// <summary>
        /// Email.
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// Признак, что пользователь активен.
        /// </summary>
        public bool? IsEnabled { get; set; }
    }
}
