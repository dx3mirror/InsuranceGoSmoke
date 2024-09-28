using InsuranceGoSmoke.Common.Applications.AppServices.Contexts.Common.Services.DateTimeProvider;
using InsuranceGoSmoke.Common.Applications.AppServices.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Kortros.Common.Test.UnitTests.AppServices.Extensions
{
    /// <summary>
    /// Тесты регистрации сервисов.
    /// </summary>
    internal class AppServicesRegistrarTests
    {
        [Test(Description = "Если список пустой, то после регистрации в списке должен быть один элемент.")]
        public void AddDateTimeProvider_ServiceCollectionIsEmpty_DOneElementAddedToServiceCollection()
        {
            // Arrange
            var serviceCollection = new ServiceCollection();

            // Act
            AppServicesRegistrar.AddDateTimeProvider(serviceCollection);

            // Assert
            Assert.That(serviceCollection, Has.Count.EqualTo(1));
        }

        [Test(Description = "Если список пустой, то после регистрации в списке должен быть провайдер работы с датой и временем.")]
        public void AddDateTimeProvider_ServiceCollectionIsEmpty_DateTimeProviderAddedToServiceCollection()
        {
            // Arrange
            var serviceCollection = new ServiceCollection();

            // Act
            AppServicesRegistrar.AddDateTimeProvider(serviceCollection);

            // Assert
            var element = serviceCollection.FirstOrDefault();
            Assert.That(element?.ServiceType, Is.EqualTo(typeof(IDateTimeProvider)));
        }
    }
}
