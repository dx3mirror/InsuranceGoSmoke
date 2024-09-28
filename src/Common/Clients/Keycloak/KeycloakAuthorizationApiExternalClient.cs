using Flurl;
using IdentityModel.Client;
using InsuranceGoSmoke.Common.Clients.Keycloak.Generated;
using InsuranceGoSmoke.Common.Clients.Keycloak.Models;
using InsuranceGoSmoke.Common.Clients.Keycloak.Models.Responses;
using InsuranceGoSmoke.Common.Contracts.Contracts.Access.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Data;

namespace InsuranceGoSmoke.Common.Clients.Keycloak
{
    /// <inheritdoc/>
    public class KeycloakAuthorizationApiExternalClient : IKeycloakAuthorizationApiExternalClient
    {
        private static readonly string OpenIdScope = "openid";

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly KeycloakAuthorizationOptions _options;
        private readonly ILogger<KeycloakAuthorizationApiExternalClient> _logger;
        private readonly IKeycloakGeneratedExternalApiClient _generatedApi;

        /// <summary>
        /// Создаёт экземпляр <see cref="KeycloakAuthorizationApiExternalClient"/>
        /// </summary>
        /// <param name="options">Настройки.</param>
        /// <param name="httpClientFactory">Фабрика клиентов.</param>
        /// <param name="logger">Логгер.</param>
        /// <param name="generatedApi">Сгенерированное API keycloak.</param>
        public KeycloakAuthorizationApiExternalClient(
            IOptions<KeycloakAuthorizationOptions> options,
            IHttpClientFactory httpClientFactory,
            ILogger<KeycloakAuthorizationApiExternalClient> logger,
            IKeycloakGeneratedExternalApiClient generatedApi)
        {
            _options = options.Value;
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _generatedApi = generatedApi;
        }

        /// <inheritdoc/>
        public async Task<KeycloakResponse> TrySendSmsAuthenticationCodeAsync(string phoneNumber, CancellationToken cancellationToken)
        {
            var url = $"{_options.Authority}/sms/authentication-code".SetQueryParam(nameof(phoneNumber), phoneNumber, isEncoded: true);
            return await TrySendSmsCodeAsync(phoneNumber, url, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<TokenResponse> AuthorizationAsync(string username, string password, CancellationToken cancellationToken)
        {
            TokenResponse response;
            try
            {
                var request = new PasswordTokenRequest
                {
                    ClientId = _options.ClientId,
                    ClientSecret = _options.ClientSecret,
                    GrantType = IdentityModel.OidcConstants.GrantTypes.Password,
                    Method = HttpMethod.Post,
                    UserName = username,
                    Password = password,
                    Scope = OpenIdScope
                };

                using var httpClient = _httpClientFactory.CreateClient();
                var discoveryDocumentResponse = await GetDiscoveryDocumentAsync(httpClient, cancellationToken);
                if (discoveryDocumentResponse is null ||
                    string.IsNullOrEmpty(discoveryDocumentResponse.TokenEndpoint))
                {
                    throw new KeycloakApiException("Не удалось получить данные о ссылках для авторизации. " + discoveryDocumentResponse?.Error);
                }

                request.RequestUri = new Uri(discoveryDocumentResponse.TokenEndpoint);
                response = await httpClient.RequestPasswordTokenAsync(request, cancellationToken);

                if (response.HttpStatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    _logger.LogError("Не удалось авторизовать пользователя: {UserName}. Ошибка: {Error} ({ErrorDescription})",
                        username, response.Error, response.ErrorDescription);
                    throw new KeycloakApiException($"Введен неверный логин или пароль.");
                }

                if (response.IsError)
                {
                    _logger.LogError("Не удалось авторизовать пользователя: {UserName}. Ошибка: {Error} ({ErrorDescription})",
                        username, response.Error, response.ErrorDescription);
                    throw new KeycloakApiException($"Не удалось авторизовать пользователя: {username}.");
                }

                await SetDefaultRoleAsync(username, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "При авторизации {UserName} произошла ошибка.", username);
                throw new KeycloakApiException("При авторизации произошла ошибка.", ex);
            }

            return response;
        }

        /// <inheritdoc/>
        public async Task<TokenResponse> AuthorizationByPhoneAsync(string phoneNumber, string code, CancellationToken cancellationToken)
        {
            TokenResponse response;
            try
            {
                var request = new PhoneAuthenticationCodeTokenRequest
                {
                    ClientId = _options.ClientId,
                    ClientSecret = _options.ClientSecret,
                    GrantType = IdentityModel.OidcConstants.GrantTypes.Password,
                    Method = HttpMethod.Post,
                    PhoneNumber = phoneNumber,
                    Code = code,
                    ClientCredentialStyle = ClientCredentialStyle.PostBody
                };
                request.Parameters.AddRequired(OidcConstants.PhoneAuthenticationCodeTokenRequest.PhoneNumber, request.PhoneNumber);
                request.Parameters.AddRequired(IdentityModel.OidcConstants.TokenRequest.Code, request.Code);

                using var httpClient = _httpClientFactory.CreateClient();
                var discoveryDocumentResponse = await GetDiscoveryDocumentAsync(httpClient, cancellationToken);
                if (discoveryDocumentResponse is null ||
                    string.IsNullOrEmpty(discoveryDocumentResponse.TokenEndpoint))
                {
                    throw new KeycloakApiException("Не удалось получить данные о ссылках для авторизации");
                }

                request.RequestUri = new Uri(discoveryDocumentResponse.TokenEndpoint);
                response = await httpClient.RequestTokenAsync(request, cancellationToken);

                if (response.HttpStatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    _logger.LogError("Не удалось авторизовать пользователя по номеру телефона: {PhoneNumber}. Ошибка: {Error} ({ErrorDescription})",
                        phoneNumber, response.Error, response.ErrorDescription);
                    throw new KeycloakApiException($"Введен неверный авторизационный код.");
                }

                if (response.IsError)
                {
                    _logger.LogError("Не удалось авторизовать пользователя по номеру телефона: {PhoneNumber}. Ошибка: {Error} ({ErrorDescription})",
                        phoneNumber, response.Error, response.ErrorDescription);
                    throw new KeycloakApiException($"Не удалось авторизовать пользователя по номеру телефона.");
                }

                await SetDefaultRoleAsync(phoneNumber, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "При авторизации по телефону {PhoneNumber} произошла ошибка.", phoneNumber);
                throw new KeycloakApiException($"При авторизации по телефону {phoneNumber} произошла ошибка.", ex);
            }

            return response;
        }

        /// <inheritdoc/>
        public async Task<TokenResponse> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken)
        {
            TokenResponse response;
            try
            {
                using var httpClient = _httpClientFactory.CreateClient();
                var discoveryDocumentResponse = await GetDiscoveryDocumentAsync(httpClient, cancellationToken);
                if (discoveryDocumentResponse is null ||
                    string.IsNullOrEmpty(discoveryDocumentResponse.TokenEndpoint))
                {
                    throw new KeycloakApiException("Не удалось получить данные о ссылках для авторизации");
                }

                var request = new RefreshTokenRequest
                {
                    RequestUri = new Uri(discoveryDocumentResponse.TokenEndpoint),
                    ClientId = _options.ClientId,
                    ClientSecret = _options.ClientSecret,
                    GrantType = IdentityModel.OidcConstants.GrantTypes.Password,
                    Method = HttpMethod.Post,
                    RefreshToken = refreshToken,
                    Scope = OpenIdScope
                };
                response = await httpClient.RequestRefreshTokenAsync(request, cancellationToken);
                if (response.IsError)
                {
                    _logger.LogError("Не удалось обновить токен. Ошибка: {Error} ({ErrorDescription})",
                        response.Error, response.ErrorDescription);
                    throw new KeycloakApiException($"Не удалось обновить токен.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Не удалось обновить токен.");
                throw new KeycloakApiException("Не удалось обновить токен.", ex);
            }
            return response;
        }

        /// <inheritdoc/>
        public async Task LogoutAsync(Guid userId, CancellationToken cancellationToken)
        {
            try
            {
                await _generatedApi.LogoutAsync(_options.Realm, userId.ToString(), cancellationToken: cancellationToken);
            }
            catch (Exception ex)
            {
                const string message = "Произошла ошибка при разлогине пользователя по идентификатору";
                _logger.LogError(ex, message + ": {UserId}", userId);
                throw new KeycloakApiException(message, ex);
            }
        }

        private async Task SetDefaultRoleAsync(string username, CancellationToken cancellationToken)
        {
            // Получаем текущего пользователя.
            var users = await _generatedApi.UsersAll3Async(_options.Realm, enabled: true, username: username, max: 1, cancellationToken: cancellationToken);
            var user = users.FirstOrDefault();
            if (user is null)
            {
                return;
            }

            var userIdParam = user.Id!.ToString();

            // Текущие роли пользователя.
            var userRoles = await _generatedApi.ClientsAll9Async(_options.Realm, userIdParam, _options.ClientUuid, cancellationToken);
            // Если роль уже есть, то ничего не делаем
            if (userRoles?.Count != 0)
            {
                return;
            }

            // Ролей нет, поэтому добавляем default роль - Клиент.
            var availableRoles = await _generatedApi.Available9Async(_options.Realm, userIdParam, _options.ClientUuid.ToString(), cancellationToken);
            var defaultRole = availableRoles.FirstOrDefault(r => r.Name == RoleTypes.Client.ToString());
            if (defaultRole is null)
            {
                return;
            }
            await _generatedApi.ClientsPOST6Async(_options.Realm, userIdParam, _options.ClientUuid, [defaultRole], cancellationToken);
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

        private Task<DiscoveryDocumentResponse> GetDiscoveryDocumentAsync(HttpClient client, CancellationToken cancellationToken)
        {
            return client.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = _options.MetadataAddress,
                Policy = { RequireHttps = _options.RequireHttpsMetadata }
            }, cancellationToken);
        }
    }
}
