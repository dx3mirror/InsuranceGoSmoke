using System;

namespace InsuranceGoSmoke.Common.Hosts.Features.AppFeatures.HealthCheck.Base
{
    /// <summary>
    /// Исключение, бросаемое при проблемах в конфигурировании проверок функциональностей.
    /// </summary>
    public class HealthCheckConfigurationException : Exception
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="message">Сообщение об ошибке.</param>
        public HealthCheckConfigurationException(string message) : base(message)
        {
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="message">Сообщение об ошибке.</param>
        /// <param name="innerException">Внутреннее исключение.</param>
        public HealthCheckConfigurationException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
