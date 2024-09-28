namespace InsuranceGoSmoke.Common.Clients.Options
{
    /// <summary>
    /// Базовые настройки клиента.
    /// </summary>
    public interface IClientOptions
    {
        /// <summary>
        /// URL.
        /// </summary>
        string Url { get; set; }
    }
}
