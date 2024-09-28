using InsuranceGoSmoke.Common.Contracts.Options;

namespace InsuranceGoSmoke.Common.Clients.Options
{
    /// <summary>
    /// Настройки получение данных недвижимости.
    /// </summary>
    [ConfigurationOptions("RealtyClient")]
    public class RealtyClientOptions : IClientOptions
    {
        /// <inheritdoc/>
        public string Url { get; set; } = string.Empty;
    }
}
