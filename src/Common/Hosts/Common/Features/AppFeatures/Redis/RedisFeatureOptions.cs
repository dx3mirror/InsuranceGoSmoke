namespace InsuranceGoSmoke.Common.Hosts.Features.AppFeatures.Redis
{
    /// <summary>
    /// Натсройки Redis.
    /// </summary>
    internal class RedisFeatureOptions
    {
        /// <summary>
        /// Строка соединения.
        /// </summary>
        public string? ConnectionString { get; set; }

        /// <summary>
        /// Наименование приложения.
        /// </summary>
        public string? ApplicationName { get; set; }
    }
}
