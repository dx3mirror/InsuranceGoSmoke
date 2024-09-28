using InsuranceGoSmoke.Common.Clients.Keycloak.Models.Enums;

namespace InsuranceGoSmoke.Common.Clients.Keycloak.Models
{
    /// <summary>
    /// Данные пользователя для Keycloak.
    /// </summary>
    public class KeycloakUserData
    {
        /// <summary>
        /// Создаёт экземпляр <see cref="KeycloakUserData"/>.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        public KeycloakUserData(Guid userId)
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
        /// Пол.
        /// </summary>
        public KeycloakSexType? Sex { get; set; }

        /// <summary>
        /// Email.
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// Признак, что почта подтверждена
        /// </summary>
        public bool IsEmailVerified { get; set; }

        /// <summary>
        /// Признак, что телефон подтвержден
        /// </summary>
        public bool IsPhoneNumberVerified { get; set; }

        /// <summary>
        /// День рождения.
        /// </summary>
        public DateTime? BirthDate { get; set; }

        /// <summary>
        /// Признак, что дата рождения изменена.
        /// </summary>
        public bool? IsBirthDateChanged { get; set; }

        /// <summary>
        /// Дата создания.
        /// </summary>
        public DateTime? CreateDate { get; set; }

        /// <summary>
        /// Роли.
        /// </summary>
        public IReadOnlyCollection<KeycloakUserRoleData> Roles { get; set; } = [];

        /// <summary>
        /// Признак, что пользователь активен.
        /// </summary>
        public bool? IsEnabled { get; set; }
        
        /// <summary>
        /// Признак, что первый логин за всё существование.
        /// </summary>
        public bool IsFirstLogin { get; set; }
    }
}
