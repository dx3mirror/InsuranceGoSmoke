using InsuranceGoSmoke.Common.Contracts.Options;
using InsuranceGoSmoke.Security.Contracts.Identify.Enums;

namespace InsuranceGoSmoke.Security.Contracts.Identify.Options
{
    /// <summary>
    /// Настройки идентификации.
    /// </summary>
    [ConfigurationOptions("Identify")]
    public class IdentifyOptions
    {
        /// <summary>
        /// Тип идентификации.
        /// </summary>
        public required IdentifyTypes IdentifyType { get; set; }

        /// <summary>
        /// Ссылка на подтверждение Email.
        /// </summary>
        public required string EmailVerificationLink { get; set; }

        /// <summary>
        /// Время в часах, через сколько код подтверждения Email станет невалидным.
        /// </summary>
        /// <remarks>По-умолчанию 1 день.</remarks>
        public int EmailVerificationTimeInHours { get; set; } = 24;

        /// <summary>
        /// URL на который редиректит после подтверждения Email.
        /// </summary>
        public required string EmailVerificationRedirectUrl { get; set; }
    }
}
