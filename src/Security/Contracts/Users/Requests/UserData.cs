using InsuranceGoSmoke.Security.Contracts.Users.Enums;

namespace InsuranceGoSmoke.Security.Contracts.Users.Requests
{
    /// <summary>
    /// Данные пользователя.
    /// </summary>
    public class UserData
    {
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
        public DateTime BirthDate { get; set; }

        /// <summary>
        /// Email.
        /// </summary>
        public string? Email { get; set; }
    }
}
