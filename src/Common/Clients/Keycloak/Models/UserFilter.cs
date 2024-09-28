using InsuranceGoSmoke.Common.Contracts.Contracts.Access.Enums;

namespace InsuranceGoSmoke.Common.Clients.Keycloak.Models
{
    /// <summary>
    /// Фильтр пользователей.
    /// </summary>
    public class UserFilter
    {
        /// <summary>
        /// Логин.
        /// </summary>
        public string UserName { get; init; } = string.Empty;

        /// <summary>
        /// Email.
        /// </summary>
        public string? Email { get; init; }

        /// <summary>
        /// Признак, что пользователь активен.
        /// </summary>
        public bool? IsEnabled { get; init; } = true;

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
        public RoleTypes[] Role { get; init; } = [];

        /// <summary>
        /// Смещение.
        /// </summary>
        public int Offset { get; init; } = 0;

        /// <summary>
        /// Максимально возвращаемое количество.
        /// </summary>
        public int Max { get; init; } = 100;
    }
}
