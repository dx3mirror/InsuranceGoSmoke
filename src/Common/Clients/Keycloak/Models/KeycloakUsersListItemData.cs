namespace InsuranceGoSmoke.Common.Clients.Keycloak.Models
{
    /// <summary>
    /// Данные элемента списка пользователей для Keycloak.
    /// </summary>
    public class KeycloakUsersListItemData
    {
        /// <summary>
        /// Создаёт экземпляр <see cref="KeycloakUsersListItemData"/>.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        public KeycloakUsersListItemData(Guid userId)
        {
            UserId = userId;
        }

        /// <summary>
        /// Идентификатор пользователя.
        /// </summary>
        public Guid UserId { get; }

        /// <summary>
        /// Имя.
        /// </summary>
        public string? FirstName { get; set; }

        /// <summary>
        /// Фамилия.
        /// </summary>
        public string? LastName { get; set; }

        /// <summary>
        /// Номер телефона.
        /// </summary>
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// Email.
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// Роли.
        /// </summary>
        public IReadOnlyCollection<KeycloakUserRoleData> Roles { get; set; } = [];

        /// <summary>
        /// Признак, что пользователь активен.
        /// </summary>
        public bool? IsEnabled { get; set; }
    }
}
