namespace InsuranceGoSmoke.PersonalAccount.Contracts.Responce.ActiveUsers
{
    /// <summary>
    /// Представляет настройки конфиденциальности пользователя.
    /// </summary>
    public class UserPrivacySettingsResponse
    {
        /// <summary>
        /// Получает или задает значение, указывающее, отображается ли дата рождения.
        /// </summary>
        public bool ShowBirthdate { get; set; }

        /// <summary>
        /// Получает или задает значение, указывающее, отображается ли описание.
        /// </summary>
        public bool ShowDescription { get; set; }

        /// <summary>
        /// Получает или задает значение, указывающее, отображается ли адрес электронной почты.
        /// </summary>
        public bool ShowEmail { get; set; }
    }

}
