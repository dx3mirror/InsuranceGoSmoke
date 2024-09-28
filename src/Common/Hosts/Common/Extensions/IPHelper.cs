using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System.Net;

namespace InsuranceGoSmoke.Common.Hosts.Extensions
{
    /// <summary>
    /// Helper для работы с IP.
    /// </summary>
    public static class IPHelper
    {
        private static readonly string XRealIp = "X-Real-IP";
        private static readonly string XOriginalForwardedIp = "X-Original-Forwarded-For";

        /// <summary>
        /// Возвращает оригинальный IP адрес из заголовка.
        /// </summary>
        /// <param name="request">Запрос.</param>
        /// <returns>Оригинальный IP адрес.</returns>
        public static IPAddress? GetOriginalIp(this HttpRequest request)
        {
            return TryGetIpFromHeader(request, XOriginalForwardedIp)
                ?? TryGetIpFromHeader(request, XRealIp)
                ?? request.HttpContext.Connection.RemoteIpAddress;
        }

        private static IPAddress? TryGetIpFromHeader(HttpRequest request, string headerKey)
        {
            if (request.Headers.TryGetValue(headerKey, out StringValues value)
                && IPAddress.TryParse(value, out IPAddress? address))
            {
                return address;
            }

            return null;
        }
    }
}
