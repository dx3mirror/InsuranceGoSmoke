using InsuranceGoSmoke.Security.Applications.AppServices.Contexts.Identify.Services.Abstract;
using InsuranceGoSmoke.Security.Contracts.Identify.Options;
using InsuranceGoSmoke.Security.Contracts.Identify.Responses;
using Microsoft.Extensions.Options;

namespace InsuranceGoSmoke.Security.Applications.AppServices.Contexts.Identify.Services
{
    /// <summary>
    /// Сервис конфигурации.
    /// </summary>
    public class ConfigurationService : IConfigurationService
    {
        private readonly IdentifyOptions _options;

        /// <summary>
        /// Создаёт экземпляр <see cref="ConfigurationService" />
        /// </summary>
        /// <param name="options">Настройки авторизации.</param>
        public ConfigurationService(IOptions<IdentifyOptions> options)
        {
            _options = options.Value;
        }

        /// <inheritdoc/>
        public IdentifyConfiguration GetConfig()
        {
            var config = new IdentifyConfiguration
            {
                IdentifyType = _options.IdentifyType
            };
            return config;
        }
    }
}
