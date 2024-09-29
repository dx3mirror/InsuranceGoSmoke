namespace InsuranceGoSmoke.PersonalAccount.Domain
{
    /// <summary>
    /// Класс PrivacySettings содержит настройки приватности аккаунта пользователя,
    /// включая видимость электронной почты, даты рождения и описания.
    /// </summary>
    public class PrivacySettings
    {
        /// <summary>
        /// Уникальный идентификатор клиента, связанный с настройками приватности (ID).
        /// </summary>
        public required Int32 ClientId { get; set; }

        /// <summary>
        /// Указывает, отображать ли электронную почту пользователю. По умолчанию false.
        /// </summary>
        public bool ShowEmail { get; set; } = false;

        /// <summary>
        /// Указывает, отображать ли дату рождения пользователю. По умолчанию false.
        /// </summary>
        public bool ShowBirthdate { get; set; } = false;

        /// <summary>
        /// Указывает, отображать ли описание пользователя. По умолчанию false.
        /// </summary>
        public bool ShowDescription { get; set; } = false;

        /// <summary>
        /// Конструктор, принимающий уникальный идентификатор клиента.
        /// </summary>
        /// <param name="clientId">Уникальный идентификатор клиента.</param>
        public PrivacySettings(Int32 clientId)
        {
            ClientId = clientId;
        }
    }
}
