using InsuranceGoSmoke.Common.Clients.Keycloak.Models;
using InsuranceGoSmoke.Common.Clients.Keycloak.Models.Responses;
using InsuranceGoSmoke.Common.Contracts.Contracts.Access.Enums;

namespace InsuranceGoSmoke.Common.Clients.Keycloak
{
    /// <summary>
    /// Клиент работы с пользователями через Keycloak.
    /// </summary>
    public interface IKeycloakUserApiExternalClient
    {
        /// <summary>
        /// Отправляет верификационный код через SMS.
        /// </summary>
        /// <param name="phoneNumber">Номер телефона.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Результат отправки кода.</returns>
        Task<KeycloakResponse> TrySendSmsVerificationCodeAsync(string phoneNumber, CancellationToken cancellationToken);

        /// <summary>
        /// Подтверждает Email пользователя.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <param name="email">Email.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        Task EmailVerificationAsync(Guid userId, string email, CancellationToken cancellationToken);

        /// <summary>
        /// Возвращает данные пользователя.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Данные пользователя.</returns>
        Task<KeycloakUserData?> GetUserAsync(Guid userId, CancellationToken cancellationToken);

        /// <summary>
        /// Возвращает количество пользователей по фильтру.
        /// </summary>
        /// <param name="filter">Фильтр.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Количество пользователей.</returns>
        Task<int> GetUsersCountByFilterAsync(UserFilter filter, CancellationToken cancellationToken);

        /// <summary>
        /// Возвращает список пользователей по фильтру.
        /// </summary>
        /// <param name="filter">Фильтр.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Список пользователей.</returns>
        Task<IReadOnlyCollection<KeycloakUsersListItemData>> GetUsersByFilterAsync(UserFilter filter, CancellationToken cancellationToken);

        /// <summary>
        /// Возвращает количество пользователей по фильтру.
        /// </summary>
        /// <param name="query">Запрос.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Количество пользователей.</returns>
        Task<int> GetUsersCountByQueryAsync(string query, CancellationToken cancellationToken);

        /// <summary>
        /// Возвращает список пользователей по фильтру.
        /// </summary>
        /// <param name="query">Запрос.</param>
        /// <param name="take">Количество возвращаемых записей.</param>
        /// <param name="skip">Количество пропускаемых записей.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Список пользователей.</returns>
        Task<IReadOnlyCollection<KeycloakUsersListItemData>> GetUsersByQueryAsync(string query, int take, int? skip, CancellationToken cancellationToken);

        /// <summary>
        /// Изменяет данные пользователя.
        /// </summary>
        /// <param name="userData">Данные пользователя.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        Task UpdateUserAsync(KeycloakUserData userData, CancellationToken cancellationToken);

        /// <summary>
        /// Изменяет данные пользователя.
        /// </summary>
        /// <param name="userData">Данные пользователя</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        Task UpdateUserFieldAsync(KeycloakUserData userData, CancellationToken cancellationToken);

        /// <summary>
        /// Проверяет верификационный код.
        /// </summary>
        /// <param name="phoneNumber">Номер телефона.</param>
        /// <param name="code">Код.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Результат проверки.</returns>
        Task<KeycloakResponse> VerifyCodeAsync(string phoneNumber, string code, CancellationToken cancellationToken);

        /// <summary>
        /// Изменяет номер телефона.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <param name="phoneNumber">Номер телефона.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        Task UpdatePhoneNumberAsync(Guid userId, string phoneNumber, CancellationToken cancellationToken);

        /// <summary>
        /// Устаначливает статус активности пользователя.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <param name="isEnabled">Признак активности пользователя.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        Task SetStatusUserAsync(Guid userId, bool isEnabled, CancellationToken cancellationToken);

        /// <summary>
        /// Изменяет роль пользователя.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <param name="role">Роль.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        Task ChangeUserRoleAsync(Guid userId, string role, CancellationToken cancellationToken);

        /// <summary>
        /// Возвращает закэшированные роли пользователя.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Список ролей.</returns>
        Task<IReadOnlyCollection<RoleTypes>> GetCachedUserRolesAsync(Guid userId, CancellationToken cancellationToken);
    }
}
