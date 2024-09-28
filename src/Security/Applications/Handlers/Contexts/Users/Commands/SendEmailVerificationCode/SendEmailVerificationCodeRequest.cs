using InsuranceGoSmoke.Common.Contracts.Abstract;
using InsuranceGoSmoke.Security.Applications.Handlers.Contexts.Abstract;

namespace InsuranceGoSmoke.Security.Applications.Handlers.Contexts.Users.Commands.SendEmailVerificationCode
{
    /// <summary>
    /// Запрос на отправку кода для подтверждения email.
    /// </summary>
    public class SendEmailVerificationCodeRequest : Command, IBaseRequestWithUser
    {
        /// <summary>
        /// Создаёт экземпляр <see cref="SendEmailVerificationCodeRequest"/>
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <param name="email">Email</param>
        public SendEmailVerificationCodeRequest(Guid userId, string email)
        {
            UserId = userId;
            Email = email;
        }

        /// <summary>
        /// Идентификатор пользователя.
        /// </summary>
        public Guid UserId { get; }

        /// <summary>
        /// Email.
        /// </summary>
        public string Email { get; }
    }
}
