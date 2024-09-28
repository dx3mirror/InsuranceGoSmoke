using Microsoft.Exchange.WebServices.Data;
using Microsoft.Extensions.Logging;

namespace Kortros.Notification.Applications.AppServices.Contexts.Emails.Factories.Strategies.Traciers
{
    /// <summary>
    /// Трассировщик.
    /// </summary>
    public class TraceListener : ITraceListener
    {
        private readonly ILogger<TraceListener> _logger;

        #region ITraceListener Members

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="logger">Логгер.</param>
        public TraceListener(ILogger<TraceListener> logger)
        {
            _logger = logger;
        }

        /// <inheritdoc/>
        public void Trace(string traceType, string traceMessage)
        {
            _logger.LogDebug(message: traceMessage.ToString());
        }

        #endregion
    }
}
