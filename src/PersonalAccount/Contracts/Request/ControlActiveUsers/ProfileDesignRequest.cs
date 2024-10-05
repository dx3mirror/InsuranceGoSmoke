namespace InsuranceGoSmoke.PersonalAccount.Contracts.Request.ControlActiveUsers
{
    /// <summary>
    /// Представляет запрос на обновление или создание настроек дизайна профиля пользователя.
    /// </summary>
    /// <remarks>
    /// Этот класс содержит свойства, которые определяют визуальные аспекты дизайна профиля пользователя,
    /// включая цвет темы, фоновое изображение, стиль шрифта и настройки анимации. Он используется для 
    /// передачи данных между клиентом и сервером при изменении дизайна профиля пользователя.
    /// </remarks>
    public class ProfileDesignRequest
    {
        /// <summary>
        /// Получает или задает идентификатор клиента (пользователя), связанный с настройками дизайна профиля.
        /// </summary>
        public long ClientId { get; set; }

        /// <summary>
        /// Получает или задает цвет темы для профиля пользователя. Может быть null, если не указан.
        /// </summary>
        public string? ThemeColor { get; set; }

        /// <summary>
        /// Получает или задает URL фонового изображения для профиля пользователя. Может быть null, если не указано.
        /// </summary>
        public string? BackgroundImage { get; set; }

        /// <summary>
        /// Получает или задает стиль шрифта для профиля пользователя. Может быть null, если не указан.
        /// </summary>
        public string? FontStyle { get; set; }

        /// <summary>
        /// Получает или задает значение, указывающее, включены ли анимации в профиле пользователя. По умолчанию true.
        /// </summary>
        public bool EnableAnimations { get; set; } = true;
    }
}
