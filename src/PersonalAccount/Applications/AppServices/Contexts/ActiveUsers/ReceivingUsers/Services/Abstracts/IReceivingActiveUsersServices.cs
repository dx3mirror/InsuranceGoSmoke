using InsuranceGoSmoke.Common.Contracts.Exceptions.Common;
using InsuranceGoSmoke.PersonalAccount.Contracts.Responce.ActiveUsers;

namespace InsuranceGoSmoke.PersonalAccount.Applications.AppServices.Contexts.ActiveUsers.ReceivingUsers.Services.Abstracts
{
    /// <summary>
    /// Интерфейс для сервиса получения основной информации о активных пользователях.
    /// </summary>
    public interface IReceivingActiveUsersServices
    {
        /// <summary>
        /// Получает основную информацию о пользователе по его идентификатору.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <param name="cancellationToken">Токен отмены для асинхронной операции.</param>
        /// <returns>
        /// Объект <see cref="UserMainInformationResponse"/> с основной информацией о пользователе.
        /// </returns>
        /// <exception cref="NotFoundException">Выбрасывается, если пользователь не найден.</exception>
        Task<UserMainInformationResponse> GetActiveUsersMainInformationAsync(int userId, CancellationToken cancellationToken);

        /// <summary>
        /// Получает настройки конфиденциальности пользователя по его идентификатору.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <param name="cancellationToken">Токен отмены для асинхронной операции.</param>
        /// <returns>Объект <see cref="UserPrivacySettingsResponse"/> с настройками конфиденциальности пользователя.</returns>
        /// <exception cref="NotFoundException">Выбрасывается, если пользователь не найден.</exception>
        Task<UserPrivacySettingsResponse> GetRulePrivacyUserAsync(int userId, CancellationToken cancellationToken);

        /// <summary>
        /// Получает описание пользователя по его идентификатору.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <param name="cancellationToken">Токен отмены для асинхронной операции.</param>
        /// <returns>Объект <see cref="UserDescriptionResponse"/> с описанием пользователя.</returns>
        /// <exception cref="NotFoundException">Выбрасывается, если пользователь не найден.</exception>
        Task<UserDescriptionResponse> GetDescriptionUserAsync(int userId, CancellationToken cancellationToken);

        /// <summary>
        /// Получает настройки дизайна профиля пользователя по его идентификатору.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <param name="cancellationToken">Токен отмены для асинхронной операции.</param>
        /// <returns>Объект <see cref="UserProfileDesignResponse"/> с настройками дизайна профиля пользователя.</returns>
        /// <exception cref="NotFoundException">Выбрасывается, если пользователь не найден.</exception>
        Task<UserProfileDesignResponse> GetUserProfileDesignAsync(int userId, CancellationToken cancellationToken);

        /// <summary>
        /// Получает аватар пользователя по его идентификатору.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <param name="cancellationToken">Токен отмены для асинхронной операции.</param>
        /// <returns>Объект <see cref="UserAvatarResponse"/> с информацией об аватаре пользователя.</returns>
        /// <exception cref="NotFoundException">Выбрасывается, если пользователь не найден.</exception>
        Task<UserAvatarResponse> GetUserAvatarAsync(int userId, CancellationToken cancellationToken);
    }

}
