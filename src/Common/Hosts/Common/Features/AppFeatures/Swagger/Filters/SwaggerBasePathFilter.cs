using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace InsuranceGoSmoke.Common.Hosts.Features.AppFeatures.Swagger.Filters
{
    /// <summary>
    /// Фильтр базового пути.
    /// </summary>
    public class SwaggerBasePathFilter : IDocumentFilter
    {
        private readonly string _basePath;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="basePath">Базовый путь.</param>
        public SwaggerBasePathFilter(string basePath)
        {
            _basePath = basePath;
        }

        /// <summary>
        /// Применение.
        /// </summary>
        /// <param name="swaggerDoc">Документация сваггера.</param>
        /// <param name="context">Контекст.</param>
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            swaggerDoc.Servers.Clear();
            swaggerDoc.Servers.Add(new OpenApiServer { Url = _basePath });
        }
    }
}
