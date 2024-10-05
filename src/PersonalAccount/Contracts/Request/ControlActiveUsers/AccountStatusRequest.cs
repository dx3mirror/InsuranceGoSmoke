namespace InsuranceGoSmoke.PersonalAccount.Contracts.Request.ControlActiveUsers
{
    /// <summary>
    /// Представляет запрос на создание или обновление статуса аккаунта пользователя.
    /// </summary>
    /// <remarks>
    /// Этот класс содержит свойства, которые описывают видимость, доступность,
    /// блокировку и статус премиум-подписки пользователя. Он используется для 
    /// передачи данных между клиентом и сервером при добавлении или изменении статуса аккаунта.
    /// </remarks>
    public class AccountStatusRequest
    {
        /// <summary>
        /// Получает или задает значение, указывающее, виден ли аккаунт для других пользователей. По умолчанию true.
        /// </summary>
        public bool IsVisible { get; set; } = true;

        /// <summary>
        /// Получает или задает значение, указывающее, доступен ли аккаунт для взаимодействия. По умолчанию true.
        /// </summary>
        public bool IsAccessible { get; set; } = true;

        /// <summary>
        /// Получает или задает значение, указывающее, заблокирован ли аккаунт. По умолчанию false.
        /// </summary>
        public bool IsBlocked { get; set; } = false;

        /// <summary>
        /// Получает или задает значение, указывающее, является ли аккаунт премиум. По умолчанию false.
        /// </summary>
        public bool IsPremium { get; set; } = false;
    }
}
