namespace InsuranceGoSmoke.Security.Contracts.Identify.Requests
{
    /// <summary>
    /// Данные авторизации.
    /// </summary>
    public class LoginData
    {
        /// <summary>
        /// Логин.
        /// </summary>
        public required string UserName { get; set; } = string.Empty;

        /// <summary>
        /// Пароль.
        /// </summary>
        public required string Password { get; set; } = string.Empty;
    }
}
