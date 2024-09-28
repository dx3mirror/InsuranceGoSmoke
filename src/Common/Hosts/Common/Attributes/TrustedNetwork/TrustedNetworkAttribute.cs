using InsuranceGoSmoke.Common.Contracts.Options;
using Microsoft.AspNetCore.Authorization;

namespace InsuranceGoSmoke.Common.Hosts.Attributes.TrustedNetwork
{
    /// <summary>
    /// Атрибут авторизации через доверенные сети.
    /// </summary>
    public sealed class TrustedNetworkAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        public TrustedNetworkAttribute()
        {
            AuthenticationSchemes = $"{TrustedNetworkOptions.Scheme}";
        }
    }
}
