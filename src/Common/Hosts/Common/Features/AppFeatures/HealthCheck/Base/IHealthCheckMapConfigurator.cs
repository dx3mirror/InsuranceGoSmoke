using Microsoft.AspNetCore.Routing;

namespace InsuranceGoSmoke.Common.Hosts.Features.AppFeatures.HealthCheck.Base
{
    /// <summary>
    /// Конфигуратор маппинга сервисов для проверок.
    /// </summary>
    public interface IHealthCheckMapConfigurator
    {
        /// <summary>
        /// Маппинг сервисов для проверок. 
        /// </summary>
        /// <param name="routeBuilder">Builder.</param>
        public void MapHealthCheckService(IEndpointRouteBuilder routeBuilder);
    }
}
