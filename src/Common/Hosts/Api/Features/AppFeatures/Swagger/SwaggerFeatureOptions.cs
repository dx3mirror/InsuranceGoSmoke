namespace InsuranceGoSmoke.Common.Hosts.Api.Features.AppFeatures.Swagger
{
    /// <summary>
    /// Настройки Swagger'а
    /// </summary>
    internal class SwaggerFeatureOptions
    {
        /// <summary>
        /// Базовый путь.
        /// </summary>
        public required string BasePath { get; set; } = "/";
    }
}
