namespace InsuranceGoSmoke.Security.Applications.AppServices.Contexts.Users.Models
{
    /// <summary>
    /// Данные Email пользователя.
    /// </summary>
    public class UserEmailModel
    {
        /// <summary>
        /// Ключ кэширования верификационного кода.
        /// </summary>
        public readonly static string EmailVerificationCodeCacheKey = "VerificationCode_{0}";

        /// <summary>
        /// Создаёт экземпляр <see cref="UserEmailModel"/>
        /// </summary>
        /// <remarks>Оставлено для десериализации</remarks>
        public UserEmailModel()
        {
        }

        /// <summary>
        /// Создаёт экземпляр <see cref="UserEmailModel"/>
        /// </summary>
        /// <param name="userId">Идентификтор пользователя.</param>
        /// <param name="email">Email.</param>
        public UserEmailModel(Guid userId, string email)
        {
            UserId = userId;
            Email = email;
        }

        /// <summary>
        /// Идентификтор пользователя.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Email.
        /// </summary>
        public string Email { get; set; }
    }
}
