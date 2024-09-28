using InsuranceGoSmoke.Common.Contracts.Options;
using InsuranceGoSmoke.Common.Hosts.Attributes.TrustedNetwork;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace InsuranceGoSmoke.Common.Hosts.Features.AppFeatures.Swagger.Filters
{
    /// <summary>
    /// Фильтр заголовка доверенной сети.
    /// </summary>
    public class AddTrustedNetworkHeaderFilter : IOperationFilter
    {
        /// <inheritdoc />
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (context.MethodInfo is not null)
            {
                operation.Parameters ??= new List<OpenApiParameter>();
                var hasTrustedNetworkAttribute = context.MethodInfo.GetCustomAttribute<TrustedNetworkAttribute>() != null ||
                                                 context.MethodInfo.DeclaringType?.GetCustomAttribute<TrustedNetworkAttribute>() != null;

                if (!hasTrustedNetworkAttribute)
                {
                    return;
                }
            }

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "AuthorizationHeader",
                In = ParameterLocation.Header,
                Schema = new OpenApiSchema { Type = "string", Default = new OpenApiString($"{TrustedNetworkOptions.Scheme} {TrustedNetworkOptions.Service}") },
                Required = false
            });
        }
    }
}
