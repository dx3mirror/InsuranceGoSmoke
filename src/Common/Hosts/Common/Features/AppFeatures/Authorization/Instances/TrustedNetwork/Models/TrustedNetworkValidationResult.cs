using System.Net;

namespace InsuranceGoSmoke.Common.Hosts.Features.AppFeatures.Authorization.Instances.TrustedNetwork.Models
{
    /// <summary>
    /// Результат проверки сети на доверие.
    /// </summary>
    /// <param name="originalIp">Исходный IP адрес.</param>
    /// <param name="errors">Список ошибок.</param>
    public class TrustedNetworkValidationResult(IPAddress? originalIp, IReadOnlyCollection<string> errors)
    {
        /// <summary>
        /// Возвращает успешный результат проверки.
        /// </summary>
        /// <param name="originalIp">Оригинальный IP адрес.</param>
        /// <returns>Успешный результат проверки.</returns>
        public static TrustedNetworkValidationResult Success(IPAddress? originalIp)
        {
            return new TrustedNetworkValidationResult(originalIp, Array.Empty<string>().AsReadOnly());
        }

        /// <summary>
        /// Возвращает результат проверки с ошибками.
        /// </summary>
        /// <param name="originalIp">Оригинальный IP адрес.</param>
        /// <param name="errors">Список ошибок.</param>
        /// <returns>Результат проверки с ошибками.</returns>
        public static TrustedNetworkValidationResult Error(IPAddress? originalIp, IReadOnlyCollection<string> errors)
        {
            return new TrustedNetworkValidationResult(originalIp, errors);
        }

        /// <summary>
        /// Список ошибок.
        /// </summary>
        public readonly IReadOnlyCollection<string> Errors = errors ?? Array.Empty<string>();

        /// <summary>
        /// Исходный IP адрес.
        /// </summary>
        public readonly IPAddress? OriginalIp = originalIp;

        /// <summary>
        /// Возвращает true, если нет ошибок.
        /// </summary>
        /// <param name="result">true, если нет ошибок.</param>
        public static implicit operator bool(TrustedNetworkValidationResult result)
        {
            return result.Errors == null || result.Errors.Count == 0;
        }
    }
}
