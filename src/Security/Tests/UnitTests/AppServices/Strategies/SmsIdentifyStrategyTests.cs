using Kortros.Common.Contracts.Exceptions.Common;
using Kortros.Security.Applications.AppServices.Contexts.Identify.Factories.Strategies;
using Kortros.Security.ExternalClients.Keycloak;
using Moq;

namespace Kortros.Security.Tests.UnitTests.AppServices.Strategies
{
    internal class SmsIdentifyStrategyTests
    {
        [Test(Description = "Если при отправке кода указан номер телефона формат которого не поддерживается, выбрасывается исключение")]
        public void TrySendCodeAsync_InvalidPhoneNumber_ReadableException()
        {
            // Arrange
            var keycloakApiClient = new Mock<IKeycloakAuthorizationApiClient>();
            var strategy = new SmsIdentifyStrategy(keycloakApiClient.Object);
            var destination = "123";

            // Act
            var exception = Assert.ThrowsAsync<ReadableException>(async () => await strategy.TrySendCodeAsync(destination, CancellationToken.None));

            // Assert
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.EqualTo("Формат введенного номер телефона не поддерживается."));
        }

        [Test(Description = "Если при отправке кода указан валидный номер телефона, должен вызываться метод отправки кода")]
        public async Task TrySendCodeAsync_ValidPhoneNumber_SendCodeCalled()
        {
            // Arrange
            var keycloakApiClient = new Mock<IKeycloakAuthorizationApiClient>();
            var strategy = new SmsIdentifyStrategy(keycloakApiClient.Object);
            var destination = "79999999999";

            // Act
            await strategy.TrySendCodeAsync(destination, CancellationToken.None);

            // Assert
            keycloakApiClient.Verify(c => c.TrySendSmsAuthenticationCodeAsync(destination, CancellationToken.None), Times.Once);
        }

        [Test(Description = "Если при авторизации указан номер телефона формат которого не поддерживается, выбрасывается исключение")]
        public void AuthorizationAsync_InvalidPhoneNumber_ReadableException()
        {
            // Arrange
            var keycloakApiClient = new Mock<IKeycloakAuthorizationApiClient>();
            var strategy = new SmsIdentifyStrategy(keycloakApiClient.Object);
            var phoneNumber = "123";
            var code = "1234";

            // Act
            var exception = Assert.ThrowsAsync<ReadableException>(async () => 
                                await strategy.AuthorizationAsync(phoneNumber, code, CancellationToken.None));

            // Assert
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.EqualTo("Формат введенного номер телефона не поддерживается."));
        }

        [Test(Description = "Если при авторизации указан код формат которого не поддерживается, выбрасывается исключение")]
        public void AuthorizationAsync_InvalidCode_ReadableException()
        {
            // Arrange
            var keycloakApiClient = new Mock<IKeycloakAuthorizationApiClient>();
            var strategy = new SmsIdentifyStrategy(keycloakApiClient.Object);
            var phoneNumber = "79999999999";
            var code = "12345";

            // Act
            var exception = Assert.ThrowsAsync<ReadableException>(async () =>
                                await strategy.AuthorizationAsync(phoneNumber, code, CancellationToken.None));

            // Assert
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.EqualTo("Формат введенного кода не поддерживается."));
        }

        [Test(Description = "Если при авторизации указан валидный номер телефона и код, должен вызываться метод авторизации")]
        public async Task AuthorizationAsync_ValidPhoneNumber_AuthorizationByPhoneCalled()
        {
            // Arrange
            var keycloakApiClient = new Mock<IKeycloakAuthorizationApiClient>();
            var strategy = new SmsIdentifyStrategy(keycloakApiClient.Object);
            var phoneNumber = "79999999999";
            var code = "1234";

            // Act
            await strategy.AuthorizationAsync(phoneNumber, code, CancellationToken.None);

            // Assert
            keycloakApiClient.Verify(c => c.AuthorizationByPhoneAsync(phoneNumber, code, CancellationToken.None), Times.Once);
        }
    }
}
