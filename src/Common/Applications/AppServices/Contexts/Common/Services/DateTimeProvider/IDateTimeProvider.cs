namespace InsuranceGoSmoke.Common.Applications.AppServices.Contexts.Common.Services.DateTimeProvider
{
    /// <summary>
    /// Провайдер для работы с датой и временем.
    /// </summary>
    public interface IDateTimeProvider
    {
        /// <summary>
        /// Возвращает текущее время.
        /// </summary>
        DateTime UtcNow { get; }
    }
}
