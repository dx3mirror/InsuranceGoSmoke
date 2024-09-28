using FluentValidation;
using IdentityModel.Client;
using System.Text.RegularExpressions;
using InsuranceGoSmoke.Security.ExternalClients.Keycloak;
using InsuranceGoSmoke.Common.Clients.Keycloak.Models.Responses;
using InsuranceGoSmoke.Security.Contracts.Common;

namespace InsuranceGoSmoke.Security.Applications.AppServices.Contexts.Identify.Factories.Strategies
{
    /// <summary>
    /// Стратегия идентификации через SMS.
    /// </summary>
    public class SmsIdentifyStrategy : IIdentifyStrategy, ISendingAuthorizationCodeStrategy
    {
        private readonly IKeycloakAuthorizationApiClient _keycloakApiClient;

        /// <summary>
        /// Создаёт экземпляр <see cref="SmsIdentifyStrategy"/>
        /// </summary>
        /// <param name="keycloakApiClient">Клиент работы с keycloak.</param>
        public SmsIdentifyStrategy(IKeycloakAuthorizationApiClient keycloakApiClient)
        {
            _keycloakApiClient = keycloakApiClient;
        }

        /// <inheritdoc/>
        public async Task<KeycloakResponse> TrySendCodeAsync(string destination, CancellationToken cancellationToken)
        {
            var phoneNumber = ToPhoneFormat(destination);

            CheckPhoneNumber(phoneNumber);

            return await _keycloakApiClient.TrySendSmsAuthenticationCodeAsync(phoneNumber, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<TokenResponse> AuthorizationAsync(string username, string password, CancellationToken cancellationToken)
        {
            var phoneNumber = ToPhoneFormat(username);
            var code = password;

            CheckPhoneNumber(phoneNumber);
            CheckCode(code);

            var result = await _keycloakApiClient.AuthorizationByPhoneAsync(phoneNumber, code, cancellationToken);
            return result;
        }

        private static void CheckPhoneNumber(string phoneNumber)
        {
            var isValid = Regex.IsMatch(phoneNumber, ValidationConstants.ValidPhoneNumberPattern, RegexOptions.None, TimeSpan.FromSeconds(1));
            if (!isValid)
            {
                throw new ValidationException("Формат введенного номер телефона не поддерживается.");
            }
        }

        private static string ToPhoneFormat(string destination)
        {
            return destination.Replace("+", string.Empty).Replace("-", string.Empty).Trim();
        }

        private static void CheckCode(string code)
        {
            var isValid = Regex.IsMatch(code, ValidationConstants.ValidCodePattern, RegexOptions.None, TimeSpan.FromSeconds(1));
            if (!isValid)
            {
                throw new ValidationException("Формат введенного кода не поддерживается.");
            }
        }
    }
}
