using InsuranceGoSmoke.Common.Domain;

namespace InsuranceGoSmoke.PersonalAccount.Domain.Account
{
    /// <summary>
    /// Класс User представляет пользователя системы,
    /// включая его личные данные и настройки профиля.
    /// </summary>
    public class User : Entity<long>
    {
        /// <summary>
        /// Уникальный идентификатор клиента (GUID).
        /// </summary>
        public Guid ClientGuid { get; set; }

        /// <summary>
        /// Имя пользователя.
        /// </summary>
        public required String FirstName { get; set; }

        /// <summary>
        /// Фамилия пользователя.
        /// </summary>
        public required String LastName { get; set; }

        /// <summary>
        /// Дата рождения пользователя.
        /// </summary>
        public required DateTime DateOfBirth { get; set; }

        /// <summary>
        /// Электронная почта пользователя.
        /// </summary>
        public required String Email { get; set; }

        /// <summary>
        /// Статус аккаунта пользователя.
        /// </summary>
        public AccountStatus? AccountStatus { get; set; }

        /// <summary>
        /// Настройки конфиденциальности аккаунта.
        /// </summary>
        public PrivacySettings? PrivacySettings { get; set; }

        /// <summary>
        /// Дизайн профиля пользователя.
        /// </summary>
        public ProfileDesign? ProfileDesign { get; set; }

        /// <summary>
        /// Аватар пользователя.
        /// </summary>
        public Avatar? Avatar { get; set; }

        /// <summary>
        /// Описание курения пользователя.
        /// </summary>
        public SmokingDescription? SmokingDescription { get; set; }

        /// <summary>
        /// История покупок пользователя.
        /// </summary>
        public ICollection<PurchaseHistory>? PurchaseHistories { get; set; }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="clientId"></param>
        public User(Guid clientId)
        {
            ClientGuid = clientId;
        }
    }
}
