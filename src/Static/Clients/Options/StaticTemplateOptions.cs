using InsuranceGoSmoke.Common.Contracts.Options;

namespace InsuranceGoSmoke.Static.Clients.Options
{
    /// <summary>
    /// Настройки шаблонов.
    /// </summary>
    [ConfigurationOptions("StaticTemplate")]
    public class StaticTemplateOptions
    {
        /// <summary>
        /// URL.
        /// </summary>
        public string? Url { get; set; }
    }
}
