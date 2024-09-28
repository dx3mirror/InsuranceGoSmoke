
namespace InsuranceGoSmoke.Common.Applications.AppServices.Contexts.Common.Services.DateTimeProvider
{
    /// <inheritdoc/>
    public class DateTimeProvider : IDateTimeProvider
    {
        /// <inheritdoc/>
        public DateTime UtcNow => DateTime.UtcNow;
    }
}
