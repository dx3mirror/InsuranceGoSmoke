using InsuranceGoSmoke.Common.Hosts.Features.AppFeatures.Base;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace InsuranceGoSmoke.Common.Hosts.Api.Features.AppFeatures.ExceptionHandler
{
    /// <summary>
    /// Функциональность обработки исключений.
    /// </summary>
    internal class ExceptionHandlerFeature : AppFeature
    {
        /// <inheritdoc />
        public override void UseFeature(IApplicationBuilder application, IWebHostEnvironment environment)
        {
            base.UseFeature(application, environment);

            application.UseMiddleware<ExceptionHandlingMiddleware>();
        }
    }
}
