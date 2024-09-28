using InsuranceGoSmoke.Security.Contracts.Users.Enums;

namespace InsuranceGoSmoke.Security.Applications.AppServices.Contexts.Users.Models
{
    /// <summary>
    /// Данные пользователя.
    /// </summary>
    public class UserModel
    {
        /// <summary>
        /// Создаёт экземпляр <see cref="UserModel" />
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        public UserModel(Guid userId)
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
        /// Пол.
        /// </summary>
        public SexType? Sex { get; set; }

        /// <summary>
        /// День рождения.
        /// </summary>
        public DateTime? BirthDate { get; set; }

        /// <summary>
        /// Признак, что дата рождения изменена.
        /// </summary>
        public bool? IsBirthDateChanged { get; set; }

        /// <summary>
        /// Email.
        /// </summary>
        public string? Email { get; set; }
    }
}
