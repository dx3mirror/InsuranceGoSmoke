using IdentityModel.Client;
using Kortros.Common.Clients.Keycloak.Models.Responses;
using Kortros.Common.Contracts.Contracts.Authorization;
using Kortros.Common.Contracts.Exceptions.Authorization;
using Kortros.Security.Applications.AppServices.Contexts.Identify.Factories;
using Kortros.Security.Applications.AppServices.Contexts.Identify.Factories.Strategies;
using Kortros.Security.Applications.AppServices.Contexts.Identify.Services;
using Kortros.Security.Applications.AppServices.Contexts.Identify.Services.Abstract;
using Kortros.Security.Contracts.Identify.Enums;
using Kortros.Security.ExternalClients.Keycloak;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Text.Json;

namespace Kortros.Security.Tests.UnitTests.AppServices
{
    internal class AuthorizationServiceTests
    {
        [Test(Description = "Если при авторизации произошла ошибка, то выбрасывается исключение")]
        public void AuthorizationAsync_ResultIsError_AuthorizationException()
        {
            // Arrange
            var configurationService = new Mock<IConfigurationService>();
            var identifyStrategyFactory = new Mock<IIdentifyStrategyFactory>();
            var logger = new Mock<ILogger<AuthorizationService>>();
            var keycloakApiClient = new Mock<IKeycloakAuthorizationApiClient>();
            var strategy = new Mock<IIdentifyStrategy>();
            var authorizationData = new Mock<Lazy<IAuthorizationData>>();
            var service = new AuthorizationService(configurationService.Object, identifyStrategyFactory.Object, logger.Object, keycloakApiClient.Object, authorizationData.Object);

            configurationService.Setup(s => s.GetConfig()).Returns(new Contracts.Identify.Responses.IdentifyConfiguration { IdentifyType = IdentifyTypes.Sms });
            identifyStrategyFactory.Setup(f => f.GetIdentifyStrategy(It.IsAny<IdentifyTypes>())).Returns(strategy.Object);
            strategy.Setup(s => s.AuthorizationAsync(It.IsAny<string>(), It.IsAny<string>(), CancellationToken.None))
                    .ReturnsAsync(ProtocolResponse.FromException<TokenResponse>(new Exception(), "Ошибка"));

            var phoneNumber = "123";
            var password = "123";

            // Act
            var exception = Assert.ThrowsAsync<AuthorizationException>(async () =>
                                await service.AuthorizationAsync(phoneNumber, password, CancellationToken.None));

            // Assert
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.EqualTo($"При авторизации '{phoneNumber}' произошла ошибка."));
        }

        [Test(Description = "Если при авторизации не было ошибок, то токен доступа не должен быть пустым")]
        public async Task AuthorizationAsync_ValidData_AccessTokenIsNotEmpty()
        {
            // Arrange
            var configurationService = new Mock<IConfigurationService>();
            var identifyStrategyFactory = new Mock<IIdentifyStrategyFactory>();
            var logger = new Mock<ILogger<AuthorizationService>>();
            var keycloakApiClient = new Mock<IKeycloakAuthorizationApiClient>();
            var strategy = new Mock<IIdentifyStrategy>();
            var authorizationData = new Mock<Lazy<IAuthorizationData>>();
            var service = new AuthorizationService(configurationService.Object, identifyStrategyFactory.Object, logger.Object, keycloakApiClient.Object, authorizationData.Object);

            configurationService.Setup(s => s.GetConfig()).Returns(new Contracts.Identify.Responses.IdentifyConfiguration { IdentifyType = IdentifyTypes.Sms });
            identifyStrategyFactory.Setup(f => f.GetIdentifyStrategy(It.IsAny<IdentifyTypes>())).Returns(strategy.Object);
            var httpResponseMessage = new HttpResponseMessage();
            httpResponseMessage.StatusCode = System.Net.HttpStatusCode.OK;
            httpResponseMessage.Content = new StringContent("{ \"access_token\": \"123\" }");
            strategy.Setup(s => s.AuthorizationAsync(It.IsAny<string>(), It.IsAny<string>(), CancellationToken.None))
                    .Returns(ProtocolResponse.FromHttpResponseAsync<TokenResponse>(httpResponseMessage));

            var phoneNumber = "123";
            var password = "123";

            // Act
            var result = await service.AuthorizationAsync(phoneNumber, password, CancellationToken.None);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.AccessToken, Is.Not.Null);
            Assert.That(result.AccessToken, Is.Not.Empty);
        }

        [Test(Description = "Если при обновлении токена произошла ошибка, то выбрасывается исключение")]
        public void RefreshTokenAsync_AuthorizationThrowException_AuthorizationException()
        {
            // Arrange
            var configurationService = new Mock<IConfigurationService>();
            var identifyStrategyFactory = new Mock<IIdentifyStrategyFactory>();
            var logger = new Mock<ILogger<AuthorizationService>>();
            var keycloakApiClient = new Mock<IKeycloakAuthorizationApiClient>();
            var authorizationData = new Mock<Lazy<IAuthorizationData>>();
            var service = new AuthorizationService(configurationService.Object, identifyStrategyFactory.Object, logger.Object, keycloakApiClient.Object, authorizationData.Object);

            keycloakApiClient.Setup(c => c.RefreshTokenAsync(It.IsAny<string>(), CancellationToken.None))
                             .ThrowsAsync(new Exception("Ошибка"));

            // Act
            var exception = Assert.ThrowsAsync<Exception>(async () =>
                                await service.RefreshTokenAsync(It.IsAny<string>(), CancellationToken.None));

            // Assert
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.EqualTo("Ошибка"));
        }

        [Test(Description = "Если при обновлении токена не было ошибок, то токен доступа не должен быть пустым")]
        public async Task RefreshTokenAsync_ValidData_AccessTokenIsNotEmpty()
        {
            // Arrange
            var configurationService = new Mock<IConfigurationService>();
            var identifyStrategyFactory = new Mock<IIdentifyStrategyFactory>();
            var logger = new Mock<ILogger<AuthorizationService>>();
            var keycloakApiClient = new Mock<IKeycloakAuthorizationApiClient>();
            var authorizationData = new Mock<Lazy<IAuthorizationData>>();
            var service = new AuthorizationService(configurationService.Object, identifyStrategyFactory.Object, logger.Object, keycloakApiClient.Object, authorizationData.Object);

            var httpResponseMessage = new HttpResponseMessage();
            httpResponseMessage.StatusCode = System.Net.HttpStatusCode.OK;
            httpResponseMessage.Content = new StringContent("{ \"access_token\": \"123\" }");
            keycloakApiClient.Setup(c => c.RefreshTokenAsync(It.IsAny<string>(), CancellationToken.None))
                             .Returns(ProtocolResponse.FromHttpResponseAsync<TokenResponse>(httpResponseMessage));

            var token = "123";

            // Act
            var result = await service.RefreshTokenAsync(token, CancellationToken.None);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.AccessToken, Is.Not.Null);
            Assert.That(result.AccessToken, Is.Not.Empty);
        }

        [Test(Description = "Если при отправке кода в конфиге указан авторизационный тип не поддерживающий отправку кодов, то выбрасывается исключение")]
        public void SendCodeAsync_NotAllowedIdentifyType_AuthorizationException()
        {
            // Arrange
            var configurationService = new Mock<IConfigurationService>();
            var identifyStrategyFactory = new Mock<IIdentifyStrategyFactory>();
            var logger = new Mock<ILogger<AuthorizationService>>();
            var keycloakApiClient = new Mock<IKeycloakAuthorizationApiClient>();
            var authorizationData = new Mock<Lazy<IAuthorizationData>>();
            var service = new AuthorizationService(configurationService.Object, identifyStrategyFactory.Object, logger.Object, keycloakApiClient.Object, authorizationData.Object);

            configurationService.Setup(s => s.GetConfig())
                                .Returns(new Contracts.Identify.Responses.IdentifyConfiguration { IdentifyType = IdentifyTypes.Undefined });

            // Act
            var exception = Assert.ThrowsAsync<AuthorizationException>(async () =>
                                await service.SendAuthentificationCodeAsync(It.IsAny<string>(), CancellationToken.None));

            // Assert
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.EqualTo("Отправка авторизационного кода для данного типа авторизации недоступно."));
        }

        [Test(Description = "Если при отправке кода вернулся результат с ошибкой, то выбрасывается исключение")]
        public void SendCodeAsync_ResultIsError_AuthorizationException()
        {
            // Arrange
            var configurationService = new Mock<IConfigurationService>();
            var identifyStrategyFactory = new Mock<IIdentifyStrategyFactory>();
            var logger = new Mock<ILogger<AuthorizationService>>();
            var keycloakApiClient = new Mock<IKeycloakAuthorizationApiClient>();
            var strategy = new Mock<ISendingAuthorizationCodeStrategy>();
            var authorizationData = new Mock<Lazy<IAuthorizationData>>();
            var service = new AuthorizationService(configurationService.Object, identifyStrategyFactory.Object, logger.Object, keycloakApiClient.Object, authorizationData.Object);

            configurationService.Setup(s => s.GetConfig())
                                .Returns(new Contracts.Identify.Responses.IdentifyConfiguration { IdentifyType = IdentifyTypes.Sms });
            identifyStrategyFactory.Setup(f => f.GetSendingCodeStrategy(It.IsAny<IdentifyTypes>())).Returns(strategy.Object);
            strategy.Setup(s => s.TrySendCodeAsync(It.IsAny<string>(), CancellationToken.None))
                    .ReturnsAsync(KeycloakResponse.Fail("Ошибка", "Подробности"));
            var phoneNumber = "123";

            // Act
            var exception = Assert.ThrowsAsync<AuthorizationException>(async () =>
                                await service.SendAuthentificationCodeAsync(phoneNumber, CancellationToken.None));

            // Assert
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.EqualTo($"При отправке кода на '{phoneNumber}' произошла ошибка."));
        }

        [Test(Description = "Если при отправке кода нет ошибок, то выбрасываться исключение не должно")]
        public void SendCodeAsync_ResultSuccess_NoException()
        {
            // Arrange
            var configurationService = new Mock<IConfigurationService>();
            var identifyStrategyFactory = new Mock<IIdentifyStrategyFactory>();
            var logger = new Mock<ILogger<AuthorizationService>>();
            var keycloakApiClient = new Mock<IKeycloakAuthorizationApiClient>();
            var strategy = new Mock<ISendingAuthorizationCodeStrategy>();
            var authorizationData = new Mock<Lazy<IAuthorizationData>>();
            var service = new AuthorizationService(configurationService.Object, identifyStrategyFactory.Object, logger.Object, keycloakApiClient.Object, authorizationData.Object);

            configurationService.Setup(s => s.GetConfig())
                                .Returns(new Contracts.Identify.Responses.IdentifyConfiguration { IdentifyType = IdentifyTypes.Sms }); 
            identifyStrategyFactory.Setup(f => f.GetSendingCodeStrategy(It.IsAny<IdentifyTypes>())).Returns(strategy.Object);
            strategy.Setup(s => s.TrySendCodeAsync(It.IsAny<string>(), CancellationToken.None))
                    .ReturnsAsync(KeycloakResponse.Success());

            // Act
            // Assert
            Assert.DoesNotThrowAsync(async () =>
                await service.SendAuthentificationCodeAsync(It.IsAny<string>(), CancellationToken.None));
        }
    }
}
