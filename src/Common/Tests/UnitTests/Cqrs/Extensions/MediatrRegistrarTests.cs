using InsuranceGoSmoke.Common.Cqrs.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Reflection;

namespace InsuranceGoSmoke.Common.Test.UnitTests.Cqrs.Extensions
{
    /// <summary>
    /// Тесты регистрации mediatr.
    /// </summary>
    internal class MediatrRegistrarTests
    {
        [Test(Description = "Если запрос не является командой, то не должно быть никаких транзакций")]
        public void AddMediatR_RegisterForAssembly_ServcesIsNotEmpty()
        {
            // Arrange
            var serviceCollection = new ServiceCollection();
            var assembly = new Mock<Assembly>();

            //Act
            serviceCollection.AddMediatR(assembly.Object);

            //Assert
            Assert.That(serviceCollection, Is.Not.Empty);
        }
    }
}
