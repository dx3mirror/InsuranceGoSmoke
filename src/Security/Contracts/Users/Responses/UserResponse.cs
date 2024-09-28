using InsuranceGoSmoke.Common.Contracts.Contracts.Access.Enums;
using InsuranceGoSmoke.Security.Contracts.Users.Enums;

namespace InsuranceGoSmoke.Security.Contracts.Users.Responses
{
    /// <summary>
    /// Данные пользователя.
    /// </summary>
    public class UserResponse
    {
        /// <summary>
        /// Идентификатор пользователя.
        /// </summary>
        public Guid UserId { get; set; }
        
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
        public SexType? Sex { get; set; }

        /// <summary>
        /// Email.
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// Признак, что почта подтверждена.
        /// </summary>
        public bool IsEmailVerified { get; set; }

        /// <summary>
        /// Признак, что телефон подтвержден.
        /// </summary>
        public bool IsPhoneNumberVerified { get; set; }

        /// <summary>
        /// День рождения.
        /// </summary>
        public DateTime? BirthDate { get; set; }

        /// <summary>
        /// Признак, что дата рождения менялась.
        /// </summary>
        public bool IsBirthdateChanged { get; set; }

        /// <summary>
        /// Дата создания.
        /// </summary>
        public DateTime? CreateDate { get; set; }

        /// <summary>
        /// Роль.
        /// </summary>
        public RoleResponse? Role { get; set; }
        
        /// <summary>
        /// Признак, что первый раз залогинился.
        /// </summary>
        public bool IsFirstLogin { get; set; }
    }
}
