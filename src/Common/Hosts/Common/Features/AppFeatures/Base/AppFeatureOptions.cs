namespace InsuranceGoSmoke.Common.Hosts.Features.AppFeatures.Base
{
    /// <inheritdoc/>
    public class AppFeatureOptions : IAppFeatureOptions
    {
        /// <inheritdoc/>
        public bool Disabled { get; set; }

        /// <summary>
        /// Порядок применения функциональности.
        /// </summary>
        public int Order { get; set; }
    }
}
