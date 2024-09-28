namespace InsuranceGoSmoke.Common.Clients.Keycloak.Helpers
{
    /// <summary>
    /// Атрибуты пользователя.
    /// </summary>
    internal static class UserAttributes
    {
        /// <summary>
        /// Пол.
        /// </summary>
        internal static readonly string Sex = nameof(Sex);

        /// <summary>
        /// День рождения.
        /// </summary>
        internal static readonly string BirthDate = nameof(BirthDate);

        /// <summary>
        /// Признак, что пользователь активен.
        /// </summary>
        internal static readonly string IsBirthDateChanged = nameof(IsBirthDateChanged);

        /// <summary>
        /// Номер телефона.
        /// </summary>
        internal static readonly string PhoneNumber = "phoneNumber";

        /// <summary>
        /// Признак, что номер телефона подтвержден.
        /// </summary>
        internal static readonly string PhoneNumberVerified = "phoneNumberVerified";
    }
}
