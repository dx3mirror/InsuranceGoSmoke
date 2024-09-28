using InsuranceGoSmoke.Common.Clients.Keycloak;
using InsuranceGoSmoke.Common.Contracts;
using InsuranceGoSmoke.Common.Contracts.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace InsuranceGoSmoke.Common.Tests.UnitTests.Contracts
{
    internal class RegistrarHelperTests
    {
        [Test(Description = "Если при регистрации валидные данные, то коллекция сервисов не пуста.")]
        public void RegistrarHelper_ValidData_ServiceIsNotEmpty()
        {
            // Arrange
            ServiceCollection services = new();

            // Act
            RegistrarHelper.AddConfigurationOptions<TestOptions>(services);

            // Assert
            Assert.That(services, Is.Not.Empty);
        }

        [Test(Description = "Если при регистрации нет атрибута над классом настроек, то выбрасывается исключение.")]
        public void Configure_AttributeOptionIsEmpty_Exception()
        {
            // Arrange
            TestOptions options = new();
            var keys = new Dictionary<string, string?>();
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(keys)
                .Build();

            // Act
            var exception = Assert.Throws<InvalidOperationException>(() => RegistrarHelper.Configure(options, configuration));

            // Assert
            Assert.That(exception, Is.Not.Null);
        }

        [Test(Description = "Если при регистрации конфигурация пуста, то выбрасывается исключение.")]
        public void Configure_ConfigurationIsEmpty_Exception()
        {
            // Arrange
            TestAttributeOptions options = new();
            var keys = new Dictionary<string, string?>();
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(keys)
                .Build();

            // Act
            var exception = Assert.Throws<InvalidOperationException>(() => RegistrarHelper.Configure(options, configuration));

            // Assert
            Assert.That(exception, Is.Not.Null);
        }

        [Test(Description = "Если при регистрации конфигурация заполнена, то успешно связывает.")]
        public void Configure_ConfigurationIsNotEmpty_Success()
        {
            // Arrange
            TestAttributeOptions options = new();
            var keys = new Dictionary<string, string?>
            {
                { "TestOptions", "TestOptions"}
            };
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(keys)
                .Build();

            // Act
            // Assert
            Assert.DoesNotThrow(() => RegistrarHelper.Configure(options, configuration));
        }

        public class TestOptions
        { 
        }

        [ConfigurationOptions("TestOptions")]
        public class TestAttributeOptions
        {
        }
    }
}
