using InsuranceGoSmoke.Common.Contracts.Abstract;
using InsuranceGoSmoke.Security.Contracts.Identify.Requests;
using InsuranceGoSmoke.Security.Contracts.Identify.Responses;

namespace InsuranceGoSmoke.Security.Applications.Handlers.Contexts.Identify.Commands.Login
{
    /// <summary>
    /// Запрос на авторизацию.
    /// </summary>
    public class LoginRequest : Command<LoginResponse>
    {
        /// <summary>
        /// Создаёт экземпляр <see cref="LoginRequest"/>
        /// </summary>
        /// <param name="data">Данные авторизации.</param>
        public LoginRequest(LoginData data)
        {
            UserName = data.UserName;
            Password = data.Password;
        }

        /// <summary>
        /// Логин.
        /// </summary>
        public string UserName { get; }

        /// <summary>
        /// Пароль.
        /// </summary>
        public string Password { get; }
    }
}
