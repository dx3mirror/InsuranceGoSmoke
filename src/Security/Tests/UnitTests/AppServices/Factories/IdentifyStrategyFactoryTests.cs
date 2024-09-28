using Kortros.Security.Applications.AppServices.Contexts.Identify.Factories;
using Kortros.Security.Applications.AppServices.Contexts.Identify.Factories.Strategies;
using Kortros.Security.ExternalClients.Keycloak;
using Moq;

namespace Kortros.Security.Tests.UnitTests.AppServices.Factories
{
    internal class IdentifyStrategyFactoryTests
    {
        [Test(Description = "Если тип идентификации указан неопределенный, то выбрасывается исключение.")]
        public void GetIdentifyStrategy_IdentifyTypesUndefined_ArgumentOutOfRangeException()
        {
            // Arrange
            var keycloakApiClient = new Mock<IKeycloakAuthorizationApiClient>();
            var smsFactoryStrategy = new SmsIdentifyStrategy(keycloakApiClient.Object);
            var factory = new IdentifyStrategyFactory(smsFactoryStrategy);

            // Act
            var exception = Assert.Throws<ArgumentOutOfRangeException>(
                                () => factory.GetIdentifyStrategy(Contracts.Identify.Enums.IdentifyTypes.Undefined));

            // Assert
            Assert.That(exception, Is.Not.Null);
        }

        [Test(Description = "Если тип идентификации указан СМС, то возвращается стратегия обработки по СМС.")]
        public void GetIdentifyStrategy_IdentifyTypesSms_SmsStrategy()
        {
            // Arrange
            var keycloakApiClient = new Mock<IKeycloakAuthorizationApiClient>();
            var smsFactoryStrategy = new SmsIdentifyStrategy(keycloakApiClient.Object);
            var factory = new IdentifyStrategyFactory(smsFactoryStrategy);

            // Act
            var strategy = factory.GetIdentifyStrategy(Contracts.Identify.Enums.IdentifyTypes.Sms);

            // Assert
            Assert.That(strategy, Is.EqualTo(smsFactoryStrategy));
        }

        [Test(Description = "Если тип идентификации указан неопределенный, то выбрасывается исключение.")]
        public void GetSendingCodeStrategy_IdentifyTypesUndefined_ArgumentOutOfRangeException()
        {
            // Arrange
            var keycloakApiClient = new Mock<IKeycloakAuthorizationApiClient>();
            var smsFactoryStrategy = new SmsIdentifyStrategy(keycloakApiClient.Object);
            var factory = new IdentifyStrategyFactory(smsFactoryStrategy);

            // Act
            var exception = Assert.Throws<ArgumentOutOfRangeException>(
                                () => factory.GetSendingCodeStrategy(Contracts.Identify.Enums.IdentifyTypes.Undefined));

            // Assert
            Assert.That(exception, Is.Not.Null);
        }

        [Test(Description = "Если тип идентификации указан СМС, то возвращается стратегия обработки по СМС.")]
        public void GetSendingCodeStrategy_IdentifyTypesSms_SmsStrategy()
        {
            // Arrange
            var keycloakApiClient = new Mock<IKeycloakAuthorizationApiClient>();
            var smsFactoryStrategy = new SmsIdentifyStrategy(keycloakApiClient.Object);
            var factory = new IdentifyStrategyFactory(smsFactoryStrategy);

            // Act
            var strategy = factory.GetSendingCodeStrategy(Contracts.Identify.Enums.IdentifyTypes.Sms);

            // Assert
            Assert.That(strategy, Is.EqualTo(smsFactoryStrategy));
        }
    }
}
