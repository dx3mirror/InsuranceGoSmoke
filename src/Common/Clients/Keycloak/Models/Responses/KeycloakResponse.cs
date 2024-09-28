namespace InsuranceGoSmoke.Common.Clients.Keycloak.Models.Responses
{
    /// <summary>
    /// Результат запроса в Keycloak.
    /// </summary>
    public class KeycloakResponse
    {
        /// <summary>
        /// Признак, что произошла ошибка.
        /// </summary>
        public bool IsError { get; set; }

        /// <summary>
        /// Текст ошибки.
        /// </summary>
        public string Error { get; set; } = string.Empty;

        /// <summary>
        /// Описание ошибки.
        /// </summary>
        public string ErrorDescription { get; set; } = string.Empty;

        /// <summary>
        /// Создаёт экземпляр <see cref="KeycloakResponse"/>
        /// </summary>
        protected KeycloakResponse()
        {
        }

        /// <summary>
        /// Возвращает результат с ошибкой.
        /// </summary>
        /// <param name="error">Ошибка.</param>
        /// <param name="errorDescription">Описание ошибки.</param>
        /// <returns>Результат с ошибкой.</returns>
        public static KeycloakResponse Fail(string error, string errorDescription)
        {
            var result = new KeycloakResponse();
            result.IsError = true;
            result.Error = error;
            result.ErrorDescription = errorDescription;
            return result;
        }

        /// <summary>
        /// Возвращает успешный результат.
        /// </summary>
        /// <returns>Успешный результат.</returns>
        public static KeycloakResponse Success()
        {
            var result = new KeycloakResponse();
            result.IsError = false;
            return result;
        }
    }
}
