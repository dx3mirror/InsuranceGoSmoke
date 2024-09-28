using InsuranceGoSmoke.Common.Contracts.Contracts.Access.Enums;
using InsuranceGoSmoke.Common.Contracts.Contracts.Paged;

namespace InsuranceGoSmoke.Security.Applications.AppServices.Contexts.Users.Models
{
    /// <summary>
    /// Данные для получения постраничного списка пользователей.
    /// </summary>
    public class GetPagedUsersModel : PagedFilter
    {
        /// <summary>
        /// Создаёт экземпляр <see cref="GetPagedUsersModel"/>
        /// </summary>
        /// <param name="take">Возврщаемое количество записей.</param>
        /// <param name="skip">Пропускаемое количество записей.</param>
        public GetPagedUsersModel(int take, int? skip)
        {
            Take = take;
            Skip = skip;
        }

        /// <summary>
        /// Имя.
        /// </summary>
        public string? FirstName { get; init; }

        /// <summary>
        /// Фамиоия.
        /// </summary>
        public string? LastName { get; init; }

        /// <summary>
        /// Номер телефона.
        /// </summary>
        public string? PhoneNumber { get; init; }

        /// <summary>
        /// Роли.
        /// </summary>
        public RoleTypes[] Role { get; init; }

        /// <summary>
        /// Email.
        /// </summary>
        public string? Email { get; init; }

        /// <summary>
        /// Признак, что пользователь активен.
        /// </summary>
        public bool? IsEnabled { get; init; }
    }
}
