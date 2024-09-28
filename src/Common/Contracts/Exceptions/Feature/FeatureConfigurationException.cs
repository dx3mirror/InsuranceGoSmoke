using System;

namespace InsuranceGoSmoke.Common.Contracts.Exceptions.Feature
{
    /// <summary>
    /// Исключение, бросаемое при проблемах в конфигурировании функциональностей.
    /// </summary>
    /// <param name="message">Сообщение об ошибке.</param>
    public class FeatureConfigurationException(string message) : Exception(message)
    {
    }
}
