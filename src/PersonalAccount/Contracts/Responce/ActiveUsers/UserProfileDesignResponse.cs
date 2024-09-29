namespace InsuranceGoSmoke.PersonalAccount.Contracts.Responce.ActiveUsers
{
    /// <summary>
    /// Представляет настройки дизайна профиля пользователя.
    /// </summary>
    public class UserProfileDesignResponse
    {
        /// <summary>
        /// Получает или устанавливает цвет темы профиля.
        /// </summary>
        public string ThemeColor { get; set; }

        /// <summary>
        /// Получает или устанавливает стиль шрифта профиля.
        /// </summary>
        public string FontStyle { get; set; }

        /// <summary>
        /// Получает или устанавливает изображение фона профиля.
        /// </summary>
        public string BackgroundImage { get; set; }

        /// <summary>
        /// Получает или устанавливает значение, указывающее, включены ли анимации.
        /// </summary>
        public bool EnableAnimations { get; set; }
    }
}
