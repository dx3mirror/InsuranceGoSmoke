using InsuranceGoSmoke.Common.Contracts.Contracts.Authorization;
using InsuranceGoSmoke.Common.Contracts.Exceptions.Authorization;
using InsuranceGoSmoke.Security.Applications.AppServices.Contexts.Identify.Factories;
using InsuranceGoSmoke.Security.Applications.AppServices.Contexts.Identify.Services.Abstract;
using InsuranceGoSmoke.Security.Contracts.Identify.Enums;
using InsuranceGoSmoke.Security.Contracts.Identify.Responses;
using InsuranceGoSmoke.Security.ExternalClients.Keycloak;
using Microsoft.Extensions.Logging;
using System.Data;

namespace InsuranceGoSmoke.Security.Applications.AppServices.Contexts.Identify.Services
{
    /// <inheritdoc/>
    public class AuthorizationService : IAuthorizationService
    {
        private readonly IConfigurationService _configurationService;
        private readonly IIdentifyStrategyFactory _identifyStrategyFactory;
        private readonly ILogger<AuthorizationService> _logger;
        private readonly IKeycloakAuthorizationApiClient _keycloakApiClient;
        private readonly Lazy<IAuthorizationData> _authorizationData;

        /// <summary>
        /// Список типов идентификации для которых доступна отправка кодов.
        /// </summary>
        private readonly static IReadOnlyCollection<IdentifyTypes> AuthorizationTypesAvailableSendingCode = [IdentifyTypes.Sms];

        /// <summary>
        /// Создаёт экземпляр <see cref="AuthorizationService" />
        /// </summary>
        /// <param name="сonfigurationService">Сервис конфигурации.</param>
        /// <param name="identifyStrategyFactory">Фабика стратегий идентификации.</param>
        /// <param name="logger">Логгер.</param>
        /// <param name="keycloakApiClient">Клиент работы с keycloak.</param>
        /// <param name="authorizationData">Авторизационные данные.</param>
        public AuthorizationService(IConfigurationService сonfigurationService,
            IIdentifyStrategyFactory identifyStrategyFactory,
            ILogger<AuthorizationService> logger,
            IKeycloakAuthorizationApiClient keycloakApiClient,
            Lazy<IAuthorizationData> authorizationData)
        {
            _configurationService = сonfigurationService;
            _identifyStrategyFactory = identifyStrategyFactory;
            _logger = logger;
            _keycloakApiClient = keycloakApiClient;
            _authorizationData = authorizationData;
        }

        /// <inheritdoc/>
        public async Task<LoginResponse> AuthorizationAsync(string userName, string password, CancellationToken cancellationToken)
        {
            var config = _configurationService.GetConfig();
            var strategy = _identifyStrategyFactory.GetIdentifyStrategy(config.IdentifyType);
            var response = await strategy.AuthorizationAsync(userName, password, cancellationToken);
            if (response.IsError)
            {
                _logger.LogError("При авторизации '{UserName}' способом '{IdentifyType}' произошла ошибка: {Error} ({ErrorDescription})",
                    userName, config.IdentifyType, response.Error, response.ErrorDescription);
                throw new AuthorizationException($"При авторизации '{userName}' произошла ошибка.");
            }

            return new LoginResponse(response.AccessToken!, response.RefreshToken!);
        }

        /// <inheritdoc/>
        public async Task LogoutAsync(CancellationToken cancellationToken)
        {
            var userId = _authorizationData.Value.UserId;
            if (userId.HasValue)
            {
                await _keycloakApiClient.LogoutAsync(userId.Value, cancellationToken);
            }
        }

        /// <inheritdoc/>
        public async Task<LoginResponse> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken)
        {
            var response = await _keycloakApiClient.RefreshTokenAsync(refreshToken, cancellationToken);
            return new LoginResponse(response.AccessToken!, response.RefreshToken!);
        }

        /// <inheritdoc/>
        public async Task SendAuthentificationCodeAsync(string destination, CancellationToken cancellationToken)
        {
            var config = _configurationService.GetConfig();
            if (!AuthorizationTypesAvailableSendingCode.Contains(config.IdentifyType))
            {
                _logger.LogError("Отправка авторизационного кода при авторизации способом '{IdentifyType}' недоступна",
                    config.IdentifyType);
                throw new AuthorizationException("Отправка авторизационного кода для данного типа авторизации недоступно.");
            }

            var authorizationStrategy = _identifyStrategyFactory.GetSendingCodeStrategy(config.IdentifyType);
            var response = await authorizationStrategy.TrySendCodeAsync(destination, cancellationToken);
            if (response.IsError)
            {
                _logger.LogError("При отправке кода на '{Destination}' способом '{IdentifyType}' произошла ошибка: {Error} ({ErrorDescription})",
                    destination, config.IdentifyType, response.Error, response.ErrorDescription);
                throw new AuthorizationException($"При отправке кода на '{destination}' произошла ошибка.");
            }
        }
    }
}
