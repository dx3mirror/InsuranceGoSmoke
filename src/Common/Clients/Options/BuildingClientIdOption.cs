using InsuranceGoSmoke.Common.Contracts.Options;

namespace InsuranceGoSmoke.Common.Clients.Options
{
    /// <summary>
    /// Настройки получение данных о здании недвижимости.
    /// </summary>
    [ConfigurationOptions("BuildingClientId")]
    public class BuildingClientIdOptions : IClientOptions
    {
        /// <inheritdoc/>
        public string Url { get; set; } = string.Empty;
    }
}
