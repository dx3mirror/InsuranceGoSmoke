namespace InsuranceGoSmoke.PersonalAccount.Contracts.Request.ControlActiveUsers
{
    /// <summary>
    /// Представляет запрос на создание или обновление информации о пользователе.
    /// </summary>
    /// <remarks>
    /// Этот класс используется для передачи данных о пользователе, 
    /// таких как идентификатор, уникальный клиентский GUID, 
    /// имя, фамилия, дата рождения и электронная почта.
    /// </remarks>
    public class UserRequest
    {
        /// <summary>
        /// Получает или задает уникальный идентификатор пользователя.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Получает или задает уникальный идентификатор клиента (GUID).
        /// </summary>
        public Guid ClientGuid { get; set; }

        /// <summary>
        /// Получает или задает имя пользователя. Это обязательное поле.
        /// </summary>
        public required string FirstName { get; set; }

        /// <summary>
        /// Получает или задает фамилию пользователя. Это обязательное поле.
        /// </summary>
        public required string LastName { get; set; }

        /// <summary>
        /// Получает или задает дату рождения пользователя. Это обязательное поле.
        /// </summary>
        public required DateTime DateOfBirth { get; set; }

        /// <summary>
        /// Получает или задает электронную почту пользователя. Это обязательное поле.
        /// </summary>
        public required string Email { get; set; }
    }
}
