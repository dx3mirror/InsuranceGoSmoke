namespace InsuranceGoSmoke.PersonalAccount.Domain
{
    /// <summary>
    /// Класс ProfileDesign содержит настройки оформления профиля пользователя,
    /// включая цвет темы и дополнительные параметры дизайна.
    /// </summary>
    public class ProfileDesign
    {
        /// <summary>
        /// Уникальный идентификатор клиента, связанный с дизайном профиля (FKGUIDCLIENT).
        /// </summary>
        public Int32 ClientId { get; set; }

        /// <summary>
        /// Цвет темы профиля, например, для оформления интерфейса.
        /// </summary>
        public String? ThemeColor { get; set; }

        /// <summary>
        /// Фон профиля (URL изображения).
        /// </summary>
        public String? BackgroundImage { get; set; }

        /// <summary>
        /// Стиль шрифта, используемый в профиле (например, "Arial", "Verdana").
        /// </summary>
        public String? FontStyle { get; set; }

        /// <summary>
        /// Указывает, включены ли анимации в профиле (например, для элементов интерфейса).
        /// </summary>
        public bool EnableAnimations { get; set; } = true;

        /// <summary>
        /// Конструктор, принимающий уникальный идентификатор клиента.
        /// </summary>
        /// <param name="clientId"></param>
        public ProfileDesign(Int32 clientId)
        {
            ClientId = clientId;
        }
    }
}
