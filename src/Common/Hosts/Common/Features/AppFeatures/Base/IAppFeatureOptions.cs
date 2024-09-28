namespace InsuranceGoSmoke.Common.Hosts.Features.AppFeatures.Base
{
    /// <summary>
    /// Настройки функциональности.
    /// </summary>
    public interface IAppFeatureOptions
    {
        /// <summary>
        /// Признак, что функциональность выключена.
        /// </summary>
        bool Disabled { get; set; }
    }
}
