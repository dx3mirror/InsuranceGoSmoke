using InsuranceGoSmoke.Common.Contracts.Abstract;

namespace InsuranceGoSmoke.Security.Applications.Handlers.Contexts.Identify.Commands.SendAuthentificationCode
{
    /// <summary>
    /// Запрос на отправку кода для авторизации.
    /// </summary>
    public class SendAuthentificationCodeRequest : Command
    {
        /// <summary>
        /// Создаёт экземпляр <see cref="SendAuthentificationCodeRequest"/>
        /// </summary>
        /// <param name="destination">Назначение для кода.</param>
        public SendAuthentificationCodeRequest(string destination)
        {
            Destination = destination;
        }

        /// <summary>
        /// Назначение для кода.
        /// </summary>
        public string Destination { get; }
    }
}
