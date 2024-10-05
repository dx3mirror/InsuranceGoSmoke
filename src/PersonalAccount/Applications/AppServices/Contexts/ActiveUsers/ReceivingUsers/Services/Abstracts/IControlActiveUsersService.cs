using InsuranceGoSmoke.PersonalAccount.Contracts.Request.ControlActiveUsers;

namespace InsuranceGoSmoke.PersonalAccount.Applications.AppServices.Contexts.ActiveUsers.ReceivingUsers.Services.Abstracts
{
    /// <summary>
    /// Интерфейс для управления активными пользователями в системе.
    /// </summary>
    /// <remarks>
    /// Этот интерфейс определяет методы для создания, обновления и управления данными пользователей, включая их статусы, настройки конфиденциальности,
    /// аватары и настройки дизайна профиля. Реализация данного интерфейса позволяет изолировать логику управления пользователями от других компонентов приложения
    /// </remarks>
    public interface IControlActiveUsersService
    {
        /// <summary>
        /// Создает нового пользователя вместе с его деталями (статусом аккаунта, настройками конфиденциальности).
        /// </summary>
        /// <param name="userRequest">Запрос на создание пользователя.</param>
        /// <param name="accountStatusRequest">Запрос на создание статуса аккаунта.</param>
        /// <param name="privacySettingsRequest">Запрос на создание настроек конфиденциальности.</param>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        Task CreateUserWithDetailsAsync(
            UserRequest userRequest,
            AccountStatusRequest accountStatusRequest,
            PrivacySettingsRequest privacySettingsRequest,
            CancellationToken cancellationToken);

        /// <summary>
        /// Добавляет аватар для указанного клиента.
        /// </summary>
        /// <param name="clientId">Идентификатор клиента.</param>
        /// <param name="imageData">Данные изображения аватара.</param>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        Task AddAvatarAsync(long clientId, Guid imageData, CancellationToken cancellationToken);

        /// <summary>
        /// Добавляет настройки дизайна профиля для указанного клиента.
        /// </summary>
        /// <param name="request">Запрос на добавление настроек дизайна профиля.</param>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        Task AddProfileDesignAsync(ProfileDesignRequest request, CancellationToken cancellationToken);

        /// <summary>
        /// Добавляет описание курильщика для указанного клиента.
        /// </summary>
        /// <param name="request">Запрос на добавление описания курильщика.</param>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        Task AddSmokingDescriptionAsync(SmokingDescriptionRequest request, CancellationToken cancellationToken);

        /// <summary>
        /// Обновляет описание курильщика для указанного клиента.
        /// </summary>
        /// <param name="request">Запрос на обновление описания курильщика.</param>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        Task UpdateSmokingDescriptionAsync(SmokingDescriptionRequest request, CancellationToken cancellationToken);

        /// <summary>
        /// Обновляет настройки дизайна профиля для указанного клиента.
        /// </summary>
        /// <param name="request">Запрос на обновление настроек дизайна профиля.</param>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        Task UpdateProfileDesignAsync(ProfileDesignRequest request, CancellationToken cancellationToken);

        /// <summary>
        /// Обновляет аватар для указанного клиента.
        /// </summary>
        /// <param name="clientId">Идентификатор клиента.</param>
        /// <param name="newImageData">Новые данные изображения аватара.</param>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        Task UpdateAvatarAsync(long clientId, Guid newImageData, CancellationToken cancellationToken);

        /// <summary>
        /// Обновляет данные пользователя.
        /// </summary>
        /// <param name="userRequest">Запрос на обновление данных пользователя.</param>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        Task UpdateUserAsync(UserRequest userRequest, CancellationToken cancellationToken);
    }
}
