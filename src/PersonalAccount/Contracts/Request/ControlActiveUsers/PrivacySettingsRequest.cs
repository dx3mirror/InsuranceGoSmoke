namespace InsuranceGoSmoke.PersonalAccount.Contracts.Request.ControlActiveUsers
{
    /// <summary>
    /// Представляет запрос на создание или обновление настроек конфиденциальности пользователя.
    /// </summary>
    /// <remarks>
    /// Этот класс используется для передачи данных о настройках конфиденциальности, 
    /// таких как видимость электронной почты, даты рождения и описания пользователя.
    /// </remarks>
    public class PrivacySettingsRequest
    {
        /// <summary>
        /// Получает или задает значение, указывающее, отображать ли электронную почту пользователю. По умолчанию false.
        /// </summary>
        public bool ShowEmail { get; set; } = false;

        /// <summary>
        /// Получает или задает значение, указывающее, отображать ли дату рождения пользователю. По умолчанию false.
        /// </summary>
        public bool ShowBirthdate { get; set; } = false;

        /// <summary>
        /// Получает или задает значение, указывающее, отображать ли описание пользователя. По умолчанию false.
        /// </summary>
        public bool ShowDescription { get; set; } = false;
    }
}
