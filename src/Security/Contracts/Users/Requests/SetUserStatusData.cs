namespace InsuranceGoSmoke.Security.Contracts.Users.Requests
{
    /// <summary>
    /// Данные по установке статуса пользователя.
    /// </summary>
    public class SetUserStatusData
    {
        /// <summary>
        /// Признак активности пользователя.
        /// </summary>
        public bool? IsEnabled { get; set; }
    }
}
