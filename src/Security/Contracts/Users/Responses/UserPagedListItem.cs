namespace InsuranceGoSmoke.Security.Contracts.Users.Responses
{
    /// <summary>
    /// Элемент постраничного списка пользователей.
    /// </summary>
    public class UserPagedListItem
    {
        /// <summary>
        /// Создаёт экземпляр <see cref="UserPagedListItem"/>
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        public UserPagedListItem(Guid userId)
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
        public required string PhoneNumber { get; set; }

        /// <summary>
        /// Email.
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// Признак, что пользователь активен.
        /// </summary>
        public required bool IsEnabled { get; set; }

        /// <summary>
        /// Роль.
        /// </summary>
        public RoleResponse? Role { get; set; }
    }
}
