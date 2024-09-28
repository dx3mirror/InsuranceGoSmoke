using InsuranceGoSmoke.Common.Contracts.Abstract;

namespace InsuranceGoSmoke.Security.Applications.Handlers.Contexts.Users.Commands.EmailVerification
{
    /// <summary>
    /// Команда на подтверждение Email.
    /// </summary>
    public class EmailVerificationRequest : Command
    {
        /// <summary>
        /// Создаёт экземпляр <see cref="EmailVerificationRequest"/>
        /// </summary>
        /// <param name="code">Код.</param>
        public EmailVerificationRequest(string code)
        {
            Code = code;
        }

        /// <summary>
        /// Код.
        /// </summary>
        public string Code { get; }
    }
}
