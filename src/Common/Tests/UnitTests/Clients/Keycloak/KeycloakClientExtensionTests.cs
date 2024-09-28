using InsuranceGoSmoke.Common.Clients.Keycloak;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using InsuranceGoSmoke.Common.Contracts.Exceptions.Feature;

namespace InsuranceGoSmoke.Common.Tests.UnitTests.Clients.Keycloak
{
    internal class KeycloakClientExtensionTests
    {
        [Test(Description = "Если при регистрации Keycloak API клиента нет настроек в конфиге, то выбрасывается исключение.")]
        public void AddKeycloakExternalApiClient_ConfigurationSectionIsEmpty_RegistrationException()
        {
            // Arrange
            var services = new ServiceCollection();
            var keys = new Dictionary<string, string?>();
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(keys)
                .Build();

            // Act
            // Assert
            Assert.Throws<FeatureConfigurationException>(() => 
                KeycloakClientExtension.AddKeycloakExternalApiClient(services, configuration));

            Assert.That(services, Is.Empty);
        }

        [Test(Description = "Если при регистрации Keycloak API клиента настройки заполнены, то коллекция сервисов не пуста.")]
        public void AddKeycloakExternalApiClient_ConfigurationSectionIsNotEmpty_ServicesCollectionIsNotEmpty()
        {
            // Arrange
            var services = new ServiceCollection();
            var keys = new Dictionary<string, string?>
            {
                { "Features:Authorization:Sections:0:Keycloak", string.Empty },
                { "Features:Authorization:Sections:0:Keycloak:ApiAdminBaseUrl", @"http:\\localhost" },
                { "Features:Authorization:Sections:0:Keycloak:ApiClientId", "admin-cli" },
                { "Features:Authorization:Sections:0:Keycloak:ApiClientSecret", "123" },
            };
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(keys)
                .Build();

            // Act
            // Assert
            Assert.DoesNotThrow(() =>
                KeycloakClientExtension.AddKeycloakExternalApiClient(services, configuration));

            Assert.That(services, Is.Not.Empty);
        }

        [Test(Description = "При заполненных настройках созданный запрос на токен не должен быть неопределенным.")]
        public void CreateClientCredentialsTokenRequest_OptionsAreFilled_RequestIsNotNull()
        {
            // Arrange
            var options = new KeycloakAuthorizationOptions();
            options.ApiAdminBaseUrl = @"http:\\localhost";
            options.ApiClientId = "admin-cli";
            options.ApiClientSecret = "123";

            // Act
            var request = KeycloakClientExtension.CreateClientCredentialsTokenRequest(options);

            // Assert
            Assert.That(request, Is.Not.Null);
        }
    }
}
