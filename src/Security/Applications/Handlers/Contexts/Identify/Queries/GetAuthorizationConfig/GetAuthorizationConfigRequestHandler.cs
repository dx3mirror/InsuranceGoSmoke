using InsuranceGoSmoke.Common.Applications.Handlers.Abstract;
using InsuranceGoSmoke.Security.Applications.AppServices.Contexts.Identify.Services.Abstract;
using InsuranceGoSmoke.Security.Contracts.Identify.Responses;

namespace InsuranceGoSmoke.Security.Applications.Handlers.Contexts.Identify.Queries.GetAuthorizationConfig
{
    /// <summary>
    /// Обработчик запроса <see cref="GetAuthorizationConfigRequestHandler"/>
    /// </summary>
    public class GetAuthorizationConfigRequestHandler : IQueryHandler<GetAuthorizationConfigRequest, IdentifyConfiguration>
    {
        private readonly IConfigurationService _configurationService;

        /// <summary>
        /// Создаёт экземпляр <see cref="GetAuthorizationConfigRequestHandler"/>
        /// </summary>
        /// <param name="configurationService">Сервис работы с конфигурацией.</param>
        public GetAuthorizationConfigRequestHandler(IConfigurationService configurationService)
        {
            _configurationService = configurationService;
        }

        /// <inheritdoc/>
        public Task<IdentifyConfiguration> Handle(GetAuthorizationConfigRequest request, CancellationToken cancellationToken)
        {
            var config = _configurationService.GetConfig();
            return Task.FromResult(config);
        }
    }
}
