using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace InsuranceGoSmoke.Common.Hosts.Features.AppFeatures.Swagger
{
    /// <summary>
    /// Маппер типов Swagger.
    /// </summary>
    public static class SwaggerConfigure
    {
        /// <summary>
        /// Маппинг типов.
        /// </summary>
        /// <param name="options">Настройки.</param>
        public static void MapTypes(this SwaggerGenOptions options)
        {
            options.MapType<DateOnly>(() => new OpenApiSchema
            {
                Type = "string",
                Format = "date"
            });
        }
    }
}
