namespace InsuranceGoSmoke.Common.Hosts.Features.AppFeatures.Authorization.Models
{
    /// <summary>
    /// Исключение, бросаемое при ошибках при авторизации.
    /// </summary>
    public class AuthorizationException(string message) : Exception(message)
    {
    }
}
