using InsuranceGoSmoke.Common.Contracts.Contracts.Access.Enums;
using InsuranceGoSmoke.Common.Contracts.Contracts.Paged;
using InsuranceGoSmoke.Security.Applications.AppServices.Contexts.Users.Models;
using InsuranceGoSmoke.Security.Contracts.Users.Responses;
using System.Text.Json;

namespace InsuranceGoSmoke.Security.Applications.AppServices.Contexts.Users.Services.Abstract
{
    /// <summary>
    /// Сервис работы с пользователем.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Отправляет верификационный код.
        /// </summary>
        /// <param name="phoneNumber">Номер телефона.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        Task SendVerificationCodeAsync(string phoneNumber, CancellationToken cancellationToken);

        /// <summary>
        /// Подтверждает Email.
        /// </summary>
        /// <param name="code">Код.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        Task EmailVerificationAsync(string code, CancellationToken cancellationToken);

        /// <summary>
        /// Возвращает данные пользователя.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Данные пользователя.</returns>
        Task<UserResponse> GetUserAsync(Guid userId, CancellationToken cancellationToken);

        /// <summary>
        /// Изменяет данные пользователя.
        /// </summary>
        /// <param name="data">Данные пользователя.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        Task UpdateUserAsync(UserModel data, CancellationToken cancellationToken);

        /// <summary>
        /// Изменяет поле данных пользователя.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <param name="field">Поле.</param>
        /// <param name="value">Значение.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        Task UpdateUserFieldAsync(Guid userId, string field, JsonElement value, CancellationToken cancellationToken);

        /// <summary>
        /// Изменяет номер телефона.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <param name="phoneNumber">Номер телефона.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        Task UpdatePhoneNumberAsync(Guid userId, string phoneNumber, CancellationToken cancellationToken);

        /// <summary>
        /// Возвращает постраничный список пользователей.
        /// </summary>
        /// <param name="filter">Фильтр.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Постраничный список пользователей.</returns>
        Task<PagedList<UserPagedListItem>> GetPagedUsersAsync(GetPagedUsersModel filter, CancellationToken cancellationToken);

        /// <summary>
        /// Возвращает список пользователей.
        /// </summary>
        /// <param name="query">Запрос.</param>
        /// <param name="take">Количество возвращаемых записей.</param>
        /// <param name="skip">Количество пропускаемых записей.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Постраничный список пользователей.</returns>
        Task<PagedList<UserPagedListItem>> GetPagedUsersByQueryAsync(string? query, int take, int? skip, CancellationToken cancellationToken);

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
        Task ChangeUserRoleAsync(Guid userId, RoleTypes role, CancellationToken cancellationToken);
    }
}
