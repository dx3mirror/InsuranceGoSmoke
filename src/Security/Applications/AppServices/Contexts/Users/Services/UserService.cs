using InsuranceGoSmoke.Common.Clients.Keycloak.Models;
using InsuranceGoSmoke.Common.Clients.Keycloak.Models.Enums;
using InsuranceGoSmoke.Common.Contracts.Contracts.Access.Enums;
using InsuranceGoSmoke.Common.Contracts.Contracts.Authorization;
using InsuranceGoSmoke.Common.Contracts.Contracts.Paged;
using InsuranceGoSmoke.Common.Contracts.Exceptions.Common;
using InsuranceGoSmoke.Common.Contracts.Utilities.Helpers;
using InsuranceGoSmoke.Common.Contracts.Utilities.Helpers.Access;
using InsuranceGoSmoke.Security.Applications.AppServices.Contexts.Users.Models;
using InsuranceGoSmoke.Security.Applications.AppServices.Contexts.Users.Services.Abstract;
using InsuranceGoSmoke.Security.Contracts.Users.Enums;
using InsuranceGoSmoke.Security.Contracts.Users.Responses;
using InsuranceGoSmoke.Security.ExternalClients.Keycloak;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Text.Json;

namespace InsuranceGoSmoke.Security.Applications.AppServices.Contexts.Users.Services
{
    /// <inheritdoc/>
    public class UserService : IUserService
    {
        private readonly IDistributedCache _distributedCache;
        private readonly IAuthorizationData _authorizeData;
        private readonly IKeycloakUserApiClient _keycloakApiClient;
        private readonly ILogger<UserService> _logger;

        /// <summary>
        /// Создаёт экземпляр <see cref="UserService"/>
        /// </summary>
        /// <param name="distributedCache">Кэш.</param>
        /// <param name="keycloakApiClient">Клиент для работы с Keycloak.</param>
        /// <param name="authorizeData">Данные авторизации.</param>
        /// <param name="logger">Логгер.</param>
        public UserService(
            IDistributedCache distributedCache,
            IKeycloakUserApiClient keycloakApiClient,
            ILogger<UserService> logger,
            IAuthorizationData authorizeData)
        {
            _distributedCache = distributedCache;
            _keycloakApiClient = keycloakApiClient;
            _logger = logger;
            _authorizeData = authorizeData;
        }

        /// <inheritdoc/>
        public async Task EmailVerificationAsync(string code, CancellationToken cancellationToken)
        {
            var json = await _distributedCache.GetStringAsync(string.Format(UserEmailModel.EmailVerificationCodeCacheKey, code), cancellationToken);
            if (string.IsNullOrEmpty(json))
            {
                throw new NotFoundException("Произошла ошибка при подтверждении Email. Попробуйте снова позже.");
            }

            var data = JsonSerializer.Deserialize<UserEmailModel>(json);
            if (data?.UserId is null || data.Email is null)
            {
                throw new NotFoundException("Произошла ошибка при подтверждении Email. Попробуйте снова позже.");
            }

            await _keycloakApiClient.EmailVerificationAsync(data.UserId, data.Email, cancellationToken);

            // Удаляем ключ, чтобы больше не могли вызвать.
            await _distributedCache.RemoveAsync(string.Format(UserEmailModel.EmailVerificationCodeCacheKey, code), cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<UserResponse> GetUserAsync(Guid userId, CancellationToken cancellationToken)
        {
            var user = await _keycloakApiClient.GetUserAsync(userId, cancellationToken);
            if (user is null)
            {
                _logger.LogError("Пользователь '{UserId}' не найден", userId);
                throw new ReadableException("Пользователь не найден");
            }

            var result = new UserResponse
            {
                UserId = user.UserId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                IsEmailVerified = user.IsEmailVerified,
                Sex = (SexType?)user.Sex,
                BirthDate = user.BirthDate,
                CreateDate = user.CreateDate,
                IsBirthdateChanged = user.IsBirthDateChanged ?? false,
                IsPhoneNumberVerified = true, // Всегда true, потому что пока другого варианта не может быть
                IsFirstLogin = (user.IsFirstLogin || user.PhoneNumber is "79999999999") && user.PhoneNumber is not "79999999995"
            };

            var roleName = user.Roles.FirstOrDefault()?.Name ?? string.Empty;
            var role = RoleTypeHelper.GetRole(roleName);
            if (role is not RoleTypes.Undefined)
            {
                result.Role = new RoleResponse(role, RoleTypeHelper.GetRoleName(role));
            }

            return result;
        }

        /// <inheritdoc/>
        public async Task UpdateUserAsync(UserModel data, CancellationToken cancellationToken)
        {
            var user = await GetUserAsync(data.UserId, cancellationToken)
                            ?? throw new ReadableException("Пользователь не найден.");
            CheckChangesBirthdate(data.BirthDate, user);

            var keycloakUserData = new KeycloakUserData(data.UserId)
            {
                FirstName = data.FirstName,
                LastName = data.LastName,
                BirthDate = data.BirthDate,
                IsBirthDateChanged = user.BirthDate.HasValue && user.BirthDate != data.BirthDate, // Если ДР было задано и текущее не равно предыдущему.
                Sex = (KeycloakSexType?)data.Sex,
                Email = data.Email,
                IsEmailVerified = user.IsEmailVerified && user.Email == data.Email
            };
            await _keycloakApiClient.UpdateUserAsync(keycloakUserData, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task UpdateUserFieldAsync(Guid userId, string field, JsonElement value, CancellationToken cancellationToken)
        {
            var user = await GetUserAsync(userId, cancellationToken) 
                            ?? throw new ReadableException("Пользователь не найден.");

            var keycloakUserData = new KeycloakUserData(userId);
            switch (field.FirstCharToUpper())
            {
                case nameof(KeycloakUserData.FirstName):
                    keycloakUserData.FirstName = value.GetString();
                    break;
                case nameof(KeycloakUserData.LastName):
                    keycloakUserData.LastName = value.GetString();
                    break;
                case nameof(KeycloakUserData.Sex):
                    keycloakUserData.Sex = EnumHelper.GetEnumValue<KeycloakSexType>(value.GetString());
                    break;
                case nameof(KeycloakUserData.BirthDate):
                    var date = DateTimeHelper.ToDateTime(value.GetString());
                    CheckChangesBirthdate(date, user);
                    keycloakUserData.BirthDate = date;
                    keycloakUserData.IsBirthDateChanged = user.BirthDate.HasValue && user.BirthDate != date; // Если ДР было задано и текущее не равно предыдущему.
                    break;
                case nameof(KeycloakUserData.Email):
                    keycloakUserData.Email = value.GetString();
                    keycloakUserData.IsEmailVerified = false;
                    break;
                default:
                    return;
            }
            await _keycloakApiClient.UpdateUserFieldAsync(keycloakUserData, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task SendVerificationCodeAsync(string phoneNumber, CancellationToken cancellationToken)
        {
            var result = await _keycloakApiClient.SendVerificationCodeAsync(phoneNumber, cancellationToken);
            if (result.IsError)
            {
                _logger.LogError("Произошла ошибка при отправке кода верификации. Телефон: {PhoneNumber}, Ошибка: {Error} ({ErrorDescritipn})", 
                    phoneNumber, result.Error, result.ErrorDescription);
                throw new ReadableException("Произошла ошибка при отправке кода верификации.");
            }
        }

        /// <inheritdoc/>
        public async Task UpdatePhoneNumberAsync(Guid userId, string phoneNumber, CancellationToken cancellationToken)
        {
            await _keycloakApiClient.UpdatePhoneNumberAsync(userId, phoneNumber, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<PagedList<UserPagedListItem>> GetPagedUsersAsync(GetPagedUsersModel filter, CancellationToken cancellationToken)
        {
            var userFilter = new UserFilter()
            {
                Email = filter.Email,
                FirstName = filter.FirstName,
                LastName = filter.LastName,
                PhoneNumber = filter.PhoneNumber,
                Role = filter.Role,
                IsEnabled = filter.IsEnabled,
                Max = filter.Take,
                Offset = filter.Skip ?? 0
            };
            var count = await _keycloakApiClient.GetUsersCountByFilterAsync(userFilter, cancellationToken);

            var users = await _keycloakApiClient.GetUsersByFilterAsync(userFilter, cancellationToken);
            UserPagedListItem[] list = MapUsers(users);

            // В Keycloak есть баг, что Count возвращает 0 при фильтрации, в то время как Users возвращает > 0 записей, поэтому здесь переопределяем количество.
            var total = users.Count > count ? users.Count : count;
            return new PagedList<UserPagedListItem>(list, total);
        }

        /// <inheritdoc/>
        public async Task<PagedList<UserPagedListItem>> GetPagedUsersByQueryAsync(string? query, int take, int? skip, CancellationToken cancellationToken)
        {
            var count = await _keycloakApiClient.GetUsersCountByQueryAsync(query, cancellationToken);

            var users = await _keycloakApiClient.GetUsersByQueryAsync(query, take, skip, cancellationToken);
            UserPagedListItem[] list = MapUsers(users);

            // В Keycloak есть баг, что Count возвращает 0 при фильтрации, в то время как Users возвращает > 0 записей, поэтому здесь переопределяем количество.
            var total = users.Count > count ? users.Count : count;
            return new PagedList<UserPagedListItem>(list, total);
        }

        /// <inheritdoc/>
        public Task SetStatusUserAsync(Guid userId, bool isEnabled, CancellationToken cancellationToken)
        {
            return _keycloakApiClient.SetStatusUserAsync(userId, isEnabled, cancellationToken);
        }

        /// <inheritdoc/>
        public Task ChangeUserRoleAsync(Guid userId, RoleTypes role, CancellationToken cancellationToken)
        {
            return _keycloakApiClient.ChangeUserRoleAsync(userId, role, cancellationToken);
        }

        private static UserPagedListItem[] MapUsers(IReadOnlyCollection<KeycloakUsersListItemData> users)
        {
            return users.Select(u =>
            {
                var user = new UserPagedListItem(u.UserId)
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    PhoneNumber = u.PhoneNumber ?? string.Empty,
                    Email = u.Email,
                    IsEnabled = u.IsEnabled ?? false,
                };

                var roleName = u.Roles.FirstOrDefault()?.Name ?? string.Empty;
                var role = RoleTypeHelper.GetRole(roleName);
                if (role is RoleTypes.Undefined)
                {
                    role = RoleTypes.Client;
                }
                user.Role = new RoleResponse(role, RoleTypeHelper.GetRoleName(role));

                return user;
            }).ToArray();
        }

        private void CheckChangesBirthdate(DateTime? birthDate, UserResponse user)
        {
            // Админам разрешено менять дату.
            if (_authorizeData.IsAdmin)
            {
                return;
            }

            if (user.IsBirthdateChanged && user.BirthDate != birthDate)
            {
                throw new ReadableException("Дата рождения уже менялась. Для повторного изменения обратитесь к администратору.");
            }
        }
    }
}
