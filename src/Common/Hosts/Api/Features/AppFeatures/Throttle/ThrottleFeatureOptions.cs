using AspNetCoreRateLimit;

namespace InsuranceGoSmoke.Common.Hosts.Api.Features.AppFeatures.Throttle
{
    /// <summary>
    /// Настройки троттлинга.
    /// </summary>
    public class ThrottleFeatureOptions
    {
        /// <summary>
        /// Признак, что ограничение по endpoint'у, а не глобально
        /// </summary>
        public bool EnableEndpointRateLimiting { get; set; }

        /// <summary>
        /// Признак, что ограничение отсчитывается от последнего успешного, заблокированные не учитываются
        /// </summary>
        public bool StackBlockedRequests { get; set; }

        /// <summary>
        /// Общие правила.
        /// </summary>
        public List<RateLimitRule> GeneralRules { get; set; } = [];

        /// <summary>
        /// IP правила.
        /// </summary>
        public List<IpRateLimitPolicy> IpRules { get; set; } = [];
    }
}
