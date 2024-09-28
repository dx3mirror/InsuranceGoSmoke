using Microsoft.Extensions.Logging;
using Moq;
using System.Reflection;
using DiagnosticProviderClass = InsuranceGoSmoke.Common.Applications.AppServices.Contexts.Common.Services.DiagnosticProvider.DiagnosticProvider;

namespace InsuranceGoSmoke.Common.Tests.UnitTests.AppServices.Services.DiagnosticProvider
{
    /// <summary>
    /// Тесты провайдера диагностики.
    /// </summary>
    internal class DiagnosticProviderTests
    {
        [Test(Description = "При создании активности и передаче сборки, активность не должна быть неопределенной")]
        public void StartActivity_ValidAssembly_ActivityIsNotNull()
        {
            // Arrange
            var logger = new Mock<ILogger<DiagnosticProviderClass>>();
            var provider = new DiagnosticProviderClass(logger.Object);
            var assembly = new Mock<Assembly>();
            assembly.Setup(a => a.GetName()).Returns(new AssemblyName("Assembly.Name"));

            // Act
            var activity = provider.StartActivity(assembly.Object);

            // Assert
            Assert.That(activity, Is.Not.Null);
        }

        [Test(Description = "При создании активности, если сборка неопределена, бросается исключение")]
        public void StartActivity_AssemblyIsNull_ThrowArgumentNullException()
        {
            // Arrange
            var logger = new Mock<ILogger<DiagnosticProviderClass>>();
            var provider = new DiagnosticProviderClass(logger.Object);

            // Act
            var exception = Assert.Throws<ArgumentNullException>(() => 
                                provider.StartActivity(assembly: null));

            // Assert
            Assert.That(exception, Is.Not.Null);
        }

        [Test(Description = "При создании счетчика, логируется если сборка неопределена")]
        public void CreateCounter_AssemblyIsNull_LogAssemblyIsNull()
        {
            // Arrange
            var logger = new Mock<ILogger<DiagnosticProviderClass>>();
            var provider = new DiagnosticProviderClass(logger.Object);

            // Act
            var counter = provider.CreateCounter<int>(name: "name");

            // Assert
            logger.Verify(l =>
                l.Log(LogLevel.Warning,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) => true),
                    It.IsAny<Exception?>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
            Assert.That(counter, Is.Null);
        }

        [Test(Description = "При создании счетчика, создается счетки")]
        public void CreateCounter_AssemblyIsNotNull_CounterCreated()
        {
            // Arrange
            var logger = new Mock<ILogger<DiagnosticProviderClass>>();
            var provider = new DiagnosticProviderClass(logger.Object);
            var assembly = new Mock<Assembly>();
            assembly.Setup(a => a.GetName()).Returns(new AssemblyName("Assembly.Name"));
            provider.StartActivity(assembly.Object);

            // Act
            var counter = provider.CreateCounter<int>(name: "name");

            // Assert
            Assert.That(counter, Is.Not.Null);
        }

        [Test(Description = "При создании счетчика, логируется если сборка неопределена")]
        public void CreateCounterWithDescription_AssemblyIsNull_LogAssemblyIsNull()
        {
            // Arrange
            var logger = new Mock<ILogger<DiagnosticProviderClass>>();
            var provider = new DiagnosticProviderClass(logger.Object);

            // Act
            var counter = provider.CreateCounter<int>(name: "name", description: "description");

            // Assert
            logger.Verify(l =>
                l.Log(LogLevel.Warning,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) => true),
                    It.IsAny<Exception?>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
            Assert.That(counter, Is.Null);
        }

        [Test(Description = "При создании счетчика, создается счетки")]
        public void CreateCounterWithDescription_AssemblyIsNotNull_CounterCreated()
        {
            // Arrange
            var logger = new Mock<ILogger<DiagnosticProviderClass>>();
            var provider = new DiagnosticProviderClass(logger.Object);
            var assembly = new Mock<Assembly>();
            assembly.Setup(a => a.GetName()).Returns(new AssemblyName("Assembly.Name"));
            provider.StartActivity(assembly.Object);

            // Act
            var counter = provider.CreateCounter<int>(name: "name", description: "description");

            // Assert
            Assert.That(counter, Is.Not.Null);
        }

        [Test(Description = "При создании счетчика, еогда у сборке задана версия, создается счетки")]
        public void CreateCounterWithDescription_AssemblyVersionIsNotNull_CounterCreated()
        {
            // Arrange
            var logger = new Mock<ILogger<DiagnosticProviderClass>>();
            var provider = new DiagnosticProviderClass(logger.Object);
            var assembly = new Mock<Assembly>();
            var assemblyName = new AssemblyName("Assembly.Name");
            assemblyName.Version = new Version("1.0.0");
            assembly.Setup(a => a.GetName()).Returns(assemblyName);
            provider.StartActivity(assembly.Object);

            // Act
            var counter = provider.CreateCounter<int>(name: "name", description: "description");

            // Assert
            Assert.That(counter, Is.Not.Null);
        }
    }
}
