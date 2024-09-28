using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace InsuranceGoSmoke.Common.Hosts.Api.Extensions
{
    /// <summary>
    /// Регистрация для API.
    /// </summary>
    public static class HostRegistrar
    {
        /// <summary>
        /// Маппинг точек доступа.
        /// </summary>
        /// <param name="app">Данные приложения.</param>
        public static void MapServices(this WebApplication app)
        {
            app.Map("/ping", (app) => app.Run(async context =>
            {
                context.Response.StatusCode = StatusCodes.Status200OK;
                await context.Response.WriteAsync("pong");
            }));
            app.MapControllers();
        }
    }
}
