namespace InsuranceGoSmoke.Common.Hosts.Api.Features.AppFeatures.Cors
{
    /// <summary>
    /// Настройки Cors'а
    /// </summary>
    internal class CorsFeatureOptions
    {
        /// <summary>
        /// Разрешенные хосты.
        /// </summary>
        public required string[] AllowedOrigins { get; set; } = Array.Empty<string>();
    }
}
