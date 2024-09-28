using Flurl;
using InsuranceGoSmoke.Common.Clients.Keycloak.Generated;
using InsuranceGoSmoke.Common.Clients.Keycloak.Helpers;
using InsuranceGoSmoke.Common.Clients.Keycloak.Models;
using InsuranceGoSmoke.Common.Clients.Keycloak.Models.Enums;
using InsuranceGoSmoke.Common.Clients.Keycloak.Models.Responses;
using InsuranceGoSmoke.Common.Contracts.Contracts.Access.Enums;
using InsuranceGoSmoke.Common.Contracts.Exceptions.Common;
using InsuranceGoSmoke.Common.Contracts.Utilities.Helpers;
using InsuranceGoSmoke.Common.Contracts.Utilities.Helpers.Access;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Text.Json;

namespace InsuranceGoSmoke.Common.Clients.Keycloak
{
    /// <inheritdoc/>
    public class KeycloakUserApiExternalClient : IKeycloakUserApiExternalClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IKeycloakGeneratedExternalApiClient _generatedApi;
        private readonly KeycloakAuthorizationOptions _options;
        private readonly ILogger<KeycloakUserApiExternalClient> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDistributedCache _distributedCache;
        
        private static readonly string UserRolesCacheKey = "UserRoles_{0}";

        /// <summary>
        /// Создаёт экземпляр <see cref="KeycloakUserApiExternalClient"/>
        /// </summary>
        /// <param name="options">Настройки.</param>
        /// <param name="generatedApi">API.</param>
        /// <param name="logger">Логгер.</param>
        /// <param name="httpClientFactory">Фабрика клиентов.</param>
        /// <param name="httpContextAccessor">Http-контекст.</param>
        /// <param name="distributedCache">Кэш.</param>
        public KeycloakUserApiExternalClient(
            IOptions<KeycloakAuthorizationOptions> options,
            IKeycloakGeneratedExternalApiClient generatedApi,
            ILogger<KeycloakUserApiExternalClient> logger,
            IHttpClientFactory httpClientFactory,
            IHttpContextAccessor httpContextAccessor,
            IDistributedCache distributedCache)
        {
            _options = options.Value;
            _generatedApi = generatedApi;
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
            _distributedCache = distributedCache;
        }

        /// <inheritdoc/>
        public async Task EmailVerificationAsync(Guid userId, string email, CancellationToken cancellationToken)
        {
            var user = await GetKeycloakUserAsync(userId, cancellationToken) 
                        ?? throw new KeycloakApiException($"Пользователь не найден: {userId}.");
            user.EmailVerified = email == user.Email;
            await UpdateUserAsync(user, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<KeycloakUserData?> GetUserAsync(Guid userId, CancellationToken cancellationToken)
        {
            var user = await GetKeycloakUserAsync(userId, cancellationToken);
            if (user is null)
            {
                return null;
            }

            var userRoles = await _generatedApi.ClientsAll9Async(_options.Realm, user.Id!.ToString(), _options.ClientUuid, cancellationToken);

            var userLoginEvents = await _generatedApi.EventsAllAsync(_options.Realm, _options.ClientId, null, null, null, null, null, ["LOGIN", "CLIENT_LOGIN"], user.Id!, cancellationToken);
            
            return MapUserResponse(userId, user, userRoles, userLoginEvents);
        }

        /// <inheritdoc/>
        public async Task<int> GetUsersCountByFilterAsync(UserFilter filter, CancellationToken cancellationToken)
        {
            try
            {
                var search = GetSearchUserFilter(filter);

                return await _generatedApi.Count2Async(_options.Realm, enabled: filter.IsEnabled, email: filter.Email,
                    search: search, cancellationToken: cancellationToken);
            }
            catch (Exception ex)
            {
                const string message = "Произошла ошибка при получении количества пользователей по фильтру";
                _logger.LogError(ex, message + ": {Filter}", JsonSerializer.Serialize(filter));
                throw new KeycloakApiException(message, ex);
            }
        }

        /// <inheritdoc/>
        public async Task<IReadOnlyCollection<KeycloakUsersListItemData>> GetUsersByFilterAsync(UserFilter filter, CancellationToken cancellationToken)
        {
            var users = await GetUsersByFilterInternalAsync(filter, cancellationToken);
            if (users == null || users.Count == 0)
            {
                return [];
            }

            var roles = await GetUserRolesAsync(users, cancellationToken);

            return users.Select(u => MapUsersListItemResponse(Guid.Parse(u.Id!), u, roles[u.Id!])).ToArray();
        }

        /// <inheritdoc/>
        public async Task<int> GetUsersCountByQueryAsync(string query, CancellationToken cancellationToken)
        {
            try
            {
                var search = string.Empty;
                if (!string.IsNullOrEmpty(query))
                {
                    search = $"*{string.Join("* *", query.Split(" "))}*";
                }

                return await _generatedApi.Count2Async(_options.Realm, search: search, cancellationToken: cancellationToken);
            }
            catch (Exception ex)
            {
                const string message = "Произошла ошибка при получении количества пользователей по запросу";
                _logger.LogError(ex, message + ": {Query}", query);
                throw new KeycloakApiException(message, ex);
            }
        }

        /// <inheritdoc/>
        public async Task<IReadOnlyCollection<KeycloakUsersListItemData>> GetUsersByQueryAsync(
            string query, int take, int? skip, CancellationToken cancellationToken)
        {
            var users = await GetUsersByQueryInternalAsync(query, take, skip, cancellationToken);
            if (users == null || users.Count == 0)
            {
                return [];
            }

            var roles = await GetUserRolesAsync(users, cancellationToken);

            return users.Select(u => MapUsersListItemResponse(Guid.Parse(u.Id!), u, roles[u.Id!])).ToArray();
        }

        /// <inheritdoc/>
        public async Task UpdateUserAsync(KeycloakUserData userData, CancellationToken cancellationToken)
        {
            try
            {
                var user = await GetKeycloakUserAsync(userData.UserId, cancellationToken) 
                                    ?? throw new KeycloakApiException($"Пользователь '{userData.UserId}' не найден");
                var data = MapUserFieldRepresentation(user, userData);
                await UpdateUserAsync(data, cancellationToken);
            }
            catch (Exception ex)
            {
                const string message = "Произошла ошибка при редактировании пользователя по идентификатору";
                _logger.LogError(ex, message + ": {UserId}", userData.UserId);
                throw new KeycloakApiException(message, ex);
            }
        }

        /// <inheritdoc/>
        public async Task UpdateUserFieldAsync(KeycloakUserData userData, CancellationToken cancellationToken)
        {
            try
            {
                var user = await GetKeycloakUserAsync(userData.UserId, cancellationToken)
                                    ?? throw new KeycloakApiException($"Пользователь '{userData.UserId}' не найден");
                var data = MapUserFieldRepresentation(user, userData);
                await UpdateUserAsync(data, cancellationToken);
            }
            catch (Exception ex)
            {
                const string message = "Произошла ошибка при редактировании пользователя по идентификатору";
                _logger.LogError(ex, message + ": {UserId}", userData.UserId);
                throw new KeycloakApiException(message, ex);
            }
        }

        /// <inheritdoc/>
        public async Task<KeycloakResponse> TrySendSmsVerificationCodeAsync(string phoneNumber, CancellationToken cancellationToken)
        {
            var url = $"{_options.Authority}/sms/verification-code".SetQueryParam(nameof(phoneNumber), phoneNumber, isEncoded: true);
            return await TrySendSmsCodeAsync(phoneNumber, url, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<KeycloakResponse> VerifyCodeAsync(string phoneNumber, string code, CancellationToken cancellationToken)
        {
            var url = $"{_options.Authority}/sms/verification-code"
                        .SetQueryParam(nameof(phoneNumber), phoneNumber, isEncoded: true)
                        .SetQueryParam(nameof(code), code);

            try
            {
                using var httpClient = _httpClientFactory.CreateClient();
                var message = new HttpRequestMessage(HttpMethod.Post, url);
                message.Headers.Authorization = await GetAuthorizationTokenAsync();
                var response = await httpClient.SendAsync(message, cancellationToken);
                if (!response.IsSuccessStatusCode)
                {
                    var errorMessage = await response.Content.ReadAsStringAsync(cancellationToken);
                    return KeycloakResponse.Fail($"Ошибка при отправке кода по номеру телефона {phoneNumber}", $"{response.StatusCode}: {errorMessage}");
                }
            }
            catch (Exception ex)
            {
                const string message = "Произошла ошибка при проверке верификационного кода по телефону";
                _logger.LogError(ex, message + ": {PhoneNumber}", phoneNumber);
                return KeycloakResponse.Fail($"Ошибка при проверке верификационного кода по номеру телефона {phoneNumber}", ex.Message);
                throw new KeycloakApiException(message, ex);
            }

            return KeycloakResponse.Success();
        }

        /// <inheritdoc/>
        public async Task UpdatePhoneNumberAsync(Guid userId, string phoneNumber, CancellationToken cancellationToken)
        {
            try
            {
                var user = await GetUserByFilterAsync(new UserFilter { UserName = phoneNumber }, cancellationToken);
                if (user is not null)
                {
                    // Если нашли текущего пользователя.
                    if (user.Id == userId.ToString())
                    {
                        return;
                    }
                    throw new KeycloakApiException($"Пользователь с номером телефона '{phoneNumber}' уже существует.");
                }

                user = await GetKeycloakUserAsync(userId, cancellationToken)
                                ?? throw new KeycloakApiException($"Пользователь '{userId}' не найден");
                user.Username = phoneNumber;
                user.Attributes ??= new Dictionary<string, ICollection<string>>();
                AddAttribute(user.Attributes, UserAttributes.PhoneNumber, phoneNumber);
                await _generatedApi.UsersPUTAsync(_options.Realm, userId.ToString(), user, cancellationToken);
            }
            catch (Exception ex)
            {
                const string message = "Произошла ошибка при редактировании номера телефона пользователя";
                _logger.LogError(ex, message + ": {UserId}", userId);
                throw new KeycloakApiException(message, ex);
            }
        }

        /// <inheritdoc/>
        public async Task SetStatusUserAsync(Guid userId, bool isEnabled, CancellationToken cancellationToken)
        {
            try
            {
                var user = await GetKeycloakUserAsync(userId, cancellationToken)
                                ?? throw new KeycloakApiException($"Пользователь '{userId}' не найден");
                user.Enabled = isEnabled;
                await _generatedApi.UsersPUTAsync(_options.Realm, userId.ToString(), user, cancellationToken);
            }
            catch (Exception ex)
            {
                const string message = "Произошла ошибка при редактировании номера телефона пользователя";
                _logger.LogError(ex, message + ": {UserId}", userId);
                throw new KeycloakApiException(message, ex);
            }
        }

        /// <inheritdoc/>
        public async Task ChangeUserRoleAsync(Guid userId, string role, CancellationToken cancellationToken)
        {
            var userIdParam = userId.ToString();

            try
            {
                var availableRoles = await _generatedApi.Available9Async(_options.Realm, userIdParam, _options.ClientUuid.ToString(), cancellationToken);
                var newRole = availableRoles.FirstOrDefault(r => r.Name == role) 
                                ?? throw new NotFoundException($"Роль '{role}' недоступна для установки пользователю '{userIdParam}'");

                // Текущие роли пользователя.
                var userRoles = await _generatedApi.ClientsAll9Async(_options.Realm, userIdParam, _options.ClientUuid, cancellationToken);

                // Добавляем новую роль.
                await _generatedApi.ClientsPOST6Async(_options.Realm, userIdParam, _options.ClientUuid, [newRole], cancellationToken);

                // Удаляем старые роли.
                if (userRoles.Count != 0)
                {
                    await _generatedApi.ClientsDELETE6Async(_options.Realm, userIdParam, _options.ClientUuid.ToString(), userRoles, cancellationToken);
                }

                // Очищаем кэш с ролями пользователя.
                await _distributedCache.RemoveAsync(string.Format(UserRolesCacheKey, userId), cancellationToken);
            }
            catch (Exception ex)
            {
                const string message = "Произошла ошибка при редактировании роли пользователя";
                _logger.LogError(ex, message + ": {UserId}", userIdParam);
                throw new KeycloakApiException(message, ex);
            }
        }

        /// <inheritdoc/>
        public async Task<IReadOnlyCollection<RoleTypes>> GetCachedUserRolesAsync(Guid userId, CancellationToken cancellationToken)
        {
            var roles = await CacheHelper.GetCachedData(string.Format(UserRolesCacheKey, userId), async () =>
            {
                try
                {
                    var roles = await _generatedApi.ClientsAll9Async(_options.Realm, userId.ToString(), _options.ClientUuid, cancellationToken);
                    return roles.Select(r => RoleTypeHelper.GetRole(r.Name!)).ToArray();
                }
                catch (Exception ex)
                {
                    const string message = "Произошла ошибка при редактировании роли пользователя";
                    _logger.LogError(ex, message + ": {UserId}", userId);
                    throw new KeycloakApiException(message, ex);
                }
            }, new DistributedCacheEntryOptions() { AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1) }, _distributedCache, _logger, cancellationToken);
            return roles!;
        }

        private async Task<AuthenticationHeaderValue?> GetAuthorizationTokenAsync()
        {
            if (_httpContextAccessor.HttpContext is null)
            {
                return null;
            }
            var token = await _httpContextAccessor.HttpContext.GetUserAccessTokenAsync();
            return new AuthenticationHeaderValue("Bearer", token ?? string.Empty);
        }

        private async Task<KeycloakResponse> TrySendSmsCodeAsync(string phoneNumber, Url url, CancellationToken cancellationToken)
        {
            try
            {
                using var httpClient = _httpClientFactory.CreateClient();
                var message = new HttpRequestMessage(HttpMethod.Get, url);
                var response = await httpClient.SendAsync(message, cancellationToken);
                if (!response.IsSuccessStatusCode)
                {
                    var errorMessage = await response.Content.ReadAsStringAsync(cancellationToken);
                    return KeycloakResponse.Fail($"Ошибка при отправке кода по номеру телефона {phoneNumber}", $"{response.StatusCode}: {errorMessage}");
                }
            }
            catch (Exception ex)
            {
                return KeycloakResponse.Fail($"Ошибка при отправке кода по номеру телефона {phoneNumber}", ex.Message);
            }

            return KeycloakResponse.Success();
        }

        private async Task<UserRepresentation?> GetKeycloakUserAsync(Guid userId, CancellationToken cancellationToken)
        {
            try
            {
                return await _generatedApi.UsersGET2Async(_options.Realm, userId.ToString(), cancellationToken: cancellationToken);
            }
            catch (Exception ex)
            {
                const string message = "Произошла ошибка при получении пользователя по идентификатору";
                _logger.LogError(ex, message + ": {UserId}", userId);
                throw new KeycloakApiException(message, ex);
            }
        }

        private async Task UpdateUserAsync(UserRepresentation data, CancellationToken cancellationToken)
        {
            try
            {
                await _generatedApi.UsersPUTAsync(_options.Realm, data.Id!, data, cancellationToken);
            }
            catch (Exception ex)
            {
                const string message = "Произошла ошибка при изменении данных пользователя";
                _logger.LogError(ex, message + " '{UserId}' на новые данные: {UserData}", data.Id, JsonSerializer.Serialize(data));
                throw new KeycloakApiException(message, ex);
            }
        }

        private static UserRepresentation MapUserFieldRepresentation(UserRepresentation user, KeycloakUserData newUserData)
        {
            user.Attributes ??= new Dictionary<string, ICollection<string>>();

            if (newUserData.FirstName is not null)
            {
                user.FirstName = newUserData.FirstName;
            }
            if (newUserData.LastName is not null)
            {
                user.LastName = newUserData.LastName;
            }
            if (newUserData.Email is not null)
            {
                user.Email = newUserData.Email;
                user.EmailVerified = newUserData.IsEmailVerified;
            }
            if (newUserData.Sex is not null)
            {
                AddAttribute(user.Attributes, UserAttributes.Sex, newUserData.Sex.ToString());
            }
            if (newUserData.BirthDate is not null)
            {
                AddAttribute(user.Attributes, UserAttributes.BirthDate, DateTimeHelper.ToDateString(newUserData.BirthDate));
            }
            if (newUserData.IsBirthDateChanged is not null)
            {
                AddAttribute(user.Attributes, UserAttributes.IsBirthDateChanged, newUserData.IsBirthDateChanged.ToString());
            }
            return user;
        }

        private async Task<UserRepresentation?> GetUserByFilterAsync(UserFilter filter, CancellationToken cancellationToken)
        {
            var users = await GetUsersByFilterInternalAsync(filter, cancellationToken);
            return users.FirstOrDefault();
        }

        private async Task<ICollection<UserRepresentation>> GetUsersByFilterInternalAsync(UserFilter filter, CancellationToken cancellationToken)
        {
            try
            {
                var search = GetSearchUserFilter(filter);

                var users = await _generatedApi.UsersAll3Async(_options.Realm, enabled: filter.IsEnabled, email: filter.Email,
                    search: search, username: filter.UserName, first: filter.Offset, max: filter.Max, cancellationToken: cancellationToken);
                return users;
            }
            catch (Exception ex)
            {
                const string message = "Произошла ошибка при получении списка пользователей по фильтру";
                _logger.LogError(ex, message + ": {Filter}", JsonSerializer.Serialize(filter));
                throw new KeycloakApiException(message, ex);
            }
        }

        private async Task<ICollection<UserRepresentation>> GetUsersByQueryInternalAsync(string query, int take, int? skip, CancellationToken cancellationToken)
        {
            try
            {
                var search = string.Empty;
                if (!string.IsNullOrEmpty(query))
                {
                    search = $"*{string.Join("* *", query.Split(" "))}*";
                }

                var users = await _generatedApi.UsersAll3Async(_options.Realm, 
                                        search: search, first: skip, max: take, cancellationToken: cancellationToken);
                return users;
            }
            catch (Exception ex)
            {
                const string message = "Произошла ошибка при получении списка пользователей по фильтру";
                _logger.LogError(ex, message + ": {Query}", query);
                throw new KeycloakApiException(message, ex);
            }
        }

        private static string? GetSearchUserFilter(UserFilter filter)
        {
            var search = new List<string>();

            if (!string.IsNullOrEmpty(filter.PhoneNumber))
            {
                search.Add($"*{filter.PhoneNumber}*");
            }
            if (!string.IsNullOrEmpty(filter.FirstName))
            {
                search.Add($"*{filter.FirstName}*");
            }
            if (!string.IsNullOrEmpty(filter.LastName))
            {
                search.Add($"*{filter.LastName}*");
            }

            return search.Count == 0 ? null : string.Join(" ", search);
        }

        private async Task<Dictionary<string, ICollection<RoleRepresentation>>> GetUserRolesAsync(
            ICollection<UserRepresentation> users, CancellationToken cancellationToken)
        {
            var roles = new Dictionary<string, ICollection<RoleRepresentation>>();
            foreach (var user in users)
            {
                try
                {
                    var userRoles = await _generatedApi.ClientsAll9Async(_options.Realm, user.Id!.ToString(), _options.ClientUuid, cancellationToken);
                    roles.Add(user.Id!, userRoles);
                }
                catch (Exception ex)
                {
                    const string message = "Произошла ошибка при получении ролей пользователя по идентификатору";
                    _logger.LogError(ex, message + ": {UserId}", user.Id);
                    throw new KeycloakApiException(message, ex);
                }
            }

            return roles;
        }

        private static void AddAttribute(IDictionary<string, ICollection<string>> attributes, string name, string? value)
        {
            // Если значение уже есть.
            if(attributes.ContainsKey(name))
            {
                attributes.Remove(name);
            }

            if (string.IsNullOrEmpty(value))
            {
                return;
            }
            attributes.Add(name, [value]);
        }

        private static string? GetFromAttributes(IDictionary<string, ICollection<string>>? attributes, string name)
        {
            if (attributes is null)
            {
                return null;
            }

            attributes.TryGetValue(name, out var value);
            if (value is not null)
            {
                return value.FirstOrDefault();
            }
            return null;
        }

        private static KeycloakUserData MapUserResponse(Guid userId, UserRepresentation userData, ICollection<RoleRepresentation> roles, IEnumerable<EventRepresentation> events)
        {
            return new KeycloakUserData(userId)
            {
                FirstName = userData.FirstName!,
                LastName = userData.LastName!,
                Sex = EnumHelper.GetEnumValue<KeycloakSexType>(GetFromAttributes(userData.Attributes, UserAttributes.Sex)),
                BirthDate = GetFromAttributes(userData.Attributes, UserAttributes.BirthDate).ToDateTime().SpecifyKind(DateTimeKind.Utc),
                IsBirthDateChanged = GetFromAttributes(userData.Attributes, UserAttributes.IsBirthDateChanged).ToNullBool(),
                Email = userData.Email!,
                IsEmailVerified = userData.EmailVerified ?? false,
                PhoneNumber = GetFromAttributes(userData.Attributes, UserAttributes.PhoneNumber) ?? string.Empty,
                IsPhoneNumberVerified = GetFromAttributes(userData.Attributes, UserAttributes.PhoneNumberVerified)?.ToBool() ?? false,
                IsEnabled = userData.Enabled ?? false,
                CreateDate = userData.CreatedTimestamp.HasValue ? DateTime.UnixEpoch.AddMilliseconds(userData.CreatedTimestamp.Value) : null,
                Roles = roles.Select(r => new KeycloakUserRoleData(r.Id!) { Name = r.Name! }).ToArray(),
                IsFirstLogin = events.Count() == 1
            };
        }

        private static KeycloakUsersListItemData MapUsersListItemResponse(Guid userId, UserRepresentation userData, ICollection<RoleRepresentation> roles)
        {
            return new KeycloakUsersListItemData(userId)
            {
                Email = userData.Email!,
                FirstName = userData.FirstName!,
                LastName = userData.LastName!,
                PhoneNumber = GetFromAttributes(userData.Attributes, UserAttributes.PhoneNumber) ?? string.Empty,
                IsEnabled = userData.Enabled ?? false,
                Roles = roles.Select(r => new KeycloakUserRoleData (r.Id!) { Name = r.Name! }).ToArray()
            };
        }
    }
}
