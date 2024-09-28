using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace InsuranceGoSmoke.Common.Hosts.Features.AppFeatures.Swagger.Filters
{
    /// <summary>
    /// Фильтр обязательных полей для swagger.
    /// </summary>
    public class AddSwaggerRequiredSchemaFilter : ISchemaFilter
    {
        /// <inheritdoc/>
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (schema.Properties == null)
            {
                return;
            }

            var properties = context.Type.GetProperties();

            foreach (var schemaProperty in schema.Properties)
            {
                var codeProperty =
                    properties.SingleOrDefault(x => System.Text.Json.JsonNamingPolicy.CamelCase.ConvertName(x.Name) == schemaProperty.Key)
                    ?? throw new MissingFieldException($"Could not find property {schemaProperty.Key} in {context.Type}, or several names conflict.");

                var isRequired = Attribute.IsDefined(codeProperty, typeof(System.Runtime.CompilerServices.RequiredMemberAttribute));
                if (isRequired)
                {
                    schemaProperty.Value.Nullable = false;
                    _ = schema.Required.Add(schemaProperty.Key);
                }
            }
        }
    }
}
