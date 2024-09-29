namespace InsuranceGoSmoke.PersonalAccount.Contracts.Responce.ActiveUsers
{
    /// <summary>
    /// Представляет основную информацию о пользователе.
    /// </summary>
    public class UserMainInformationResponse
    {
        /// <summary>
        /// Получает или задает фамилию пользователя.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Получает или задает имя пользователя.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Получает или задает адрес электронной почты пользователя.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Получает или задает дату рождения пользователя.
        /// </summary>
        public DateTime DateOfBirth { get; set; }
    }
}
