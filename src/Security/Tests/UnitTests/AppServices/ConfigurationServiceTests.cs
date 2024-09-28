using Kortros.Security.Applications.AppServices.Contexts.Identify.Services;
using Kortros.Security.Contracts.Identify.Options;
using Microsoft.Extensions.Options;

namespace Kortros.Security.Tests.UnitTests.AppServices
{
    internal class ConfigurationServiceTests
    {
        [Test(Description = "При получении конфигурации, данные берутся из настроек")]
        public void GetConfig_OptionsFilled_IdentityTypeFilled()
        {
            // Arrange
            var config = new IdentifyOptions()
            {
                IdentifyType = Contracts.Identify.Enums.IdentifyTypes.Sms,
                EmailVerificationLink = string.Empty,
                EmailVerificationRedirectUrl = string.Empty,
            };
            var options = Options.Create(config);
            var configurationService = new ConfigurationService(options);

            // Act
            var result = configurationService.GetConfig();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IdentifyType, Is.EqualTo(config.IdentifyType));
        }
    }
}
