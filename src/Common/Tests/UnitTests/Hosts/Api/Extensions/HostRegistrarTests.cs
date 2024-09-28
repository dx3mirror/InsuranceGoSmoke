using InsuranceGoSmoke.Common.Hosts.Extensions;
using InsuranceGoSmoke.Common.Hosts.Features.AppFeatures.Base;
using InsuranceGoSmoke.Common.Hosts.Features.AppFeatures.HealthCheck;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System.Reflection;

namespace InsuranceGoSmoke.Common.Test.UnitTests.Hosts.Api.Extensions
{
    /// <summary>
    /// Тесты регистрации хоста.
    /// </summary>
    internal class HostRegistrarTests
    {
        [Test(Description = "При добавлении функциональности, она должна быть добавлена")]
        public void AddFeatures_OneFeature_FeatureRegistered()
        {
            // Arrange
            var services = new ServiceCollection();
            var hostBuilder = new Mock<IHostBuilder>();
            var loggingBuilder = new Mock<ILoggingBuilder>();
            var keys = new Dictionary<string, string?>
            {
               { "Features", string.Empty },
               { "Features:HealthCheck", string.Empty }
            };
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(keys)
                .Build();
            var assembly = new Mock<Assembly>();
            assembly.Setup(a => a.GetTypes()).Returns([typeof(HealthCheckFeature)]);

            // Act
            services.AddFeatures(hostBuilder.Object, loggingBuilder.Object, configuration, [assembly.Object]);

            // Assert
            Assert.That(services.Any(s => s.ServiceType == typeof(IAppFeature)), Is.True);
        }

        [Test(Description = "При использовании функциональности, она должна использоваться один раз")]
        public void UseFeatures_OneFeature_FeatureRegistered()
        {
            // Arrange
            var builder = new Mock<IApplicationBuilder>();
            var feature = new Mock<IAppFeature>();
            var provider = new TestServiceProvider(feature.Object);
            builder.SetupGet(b => b.ApplicationServices).Returns(provider);
            var endpointBuilder = new Mock<IEndpointRouteBuilder>();
            endpointBuilder.Setup(b => b.DataSources).Returns([]);
            builder.SetupGet(b => b.Properties).Returns(new Dictionary<string, object?>
            {
                { "__EndpointRouteBuilder", endpointBuilder.Object }
            });
            var environment = new Mock<IWebHostEnvironment>();

            // Act
            builder.Object.UseFeatures(environment.Object);

            // Assert
            feature.Verify(f => f.UseFeature(builder.Object, environment.Object), Times.Once);
        }

        [Test(Description = "При настрйоке локализации не должно быть исключений")]
        public void UseLocalization_Valid_NoExcaption()
        {
            // Arrange
            var builder = new Mock<IApplicationBuilder>();

            // Act
            // Assert
            Assert.DoesNotThrow(() => builder.Object.UseLocalization());
        }

        internal class TestServiceProvider(IAppFeature feature) : IServiceProvider, ISupportRequiredService
        {
            public object GetRequiredService(Type serviceType)
            {
                return serviceType.Name switch
                {
                    "IEnumerable`1" => new[] { feature },
                    "IOptions`1" => new Mock<IOptions<RouteOptions>>().Object,
                    _ => new[] { feature },
                };
            }

            public object? GetService(Type serviceType)
            {
                return serviceType.Name switch
                {
                    "IEnumerable`1" => new[] { feature },
                    "IOptions`1" => new[] { new Mock<IOptions<RouteOptions>>().Object },
                    "RoutingMarkerService" => new[] { feature },
                    _ => null,
                };
            }
        }
    }
}
