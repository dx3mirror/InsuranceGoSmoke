using InsuranceGoSmoke.Security.Contracts.Identify.Enums;

namespace InsuranceGoSmoke.Security.Contracts.Identify.Responses
{
    /// <summary>
    /// Данные конфигурации идентификации.
    /// </summary>
    public class IdentifyConfiguration
    {
        /// <summary>
        /// Тип идентификации.
        /// </summary>
        public IdentifyTypes IdentifyType { get; set; }
    }
}
