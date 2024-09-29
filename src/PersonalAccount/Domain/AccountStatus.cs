namespace InsuranceGoSmoke.PersonalAccount.Domain
{
    /// <summary>
    /// Класс AccountStatus представляет статус аккаунта пользователя, включая видимость,
    /// доступность, заблокирован ли аккаунт, а также наличие премиального статуса.
    /// </summary>
    public class AccountStatus
    {
        /// <summary>
        /// Уникальный идентификатор клиента, связанный с аккаунтом ().
        /// </summary>
        public required Int32 ClientId { get; set; }

        /// <summary>
        /// Показывает, виден ли аккаунт для других пользователей. По умолчанию true.
        /// </summary>
        public bool IsVisible { get; set; } = true;

        /// <summary>
        /// Указывает, доступен ли аккаунт для использования. По умолчанию true.
        /// </summary>
        public bool IsAccessible { get; set; } = true;

        /// <summary>
        /// Указывает, заблокирован ли аккаунт. По умолчанию false.
        /// </summary>
        public bool IsBlocked { get; set; } = true;

        /// <summary>
        /// Указывает, является ли аккаунт премиальным. По умолчанию false.
        /// </summary>
        public bool IsPremium { get; set; } = true;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="clientId"></param>
        public AccountStatus(Int32 clientId)
        {
            ClientId = clientId;
        }
    }
}
