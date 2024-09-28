using InsuranceGoSmoke.Common.Contracts.Abstract;
using InsuranceGoSmoke.Security.Applications.Handlers.Contexts.Abstract;

namespace InsuranceGoSmoke.Security.Applications.Handlers.Contexts.Users.Commands.SendVerificationCode
{
    /// <summary>
    /// Запрос на отправку верификационного кода.
    /// </summary>
    public class SendVerificationCodeRequest : Command, IBaseRequestWithUser
    {
        /// <summary>
        /// Создаёт экземпляр <see cref="SendVerificationCodeRequest"/>
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <param name="phoneNumber">Номер телефона.</param>
        public SendVerificationCodeRequest(Guid userId, string phoneNumber)
        {
            UserId = userId;
            PhoneNumber = phoneNumber;
        }

        /// <summary>
        /// Идентификатор пользователя.
        /// </summary>
        public Guid UserId { get; }

        /// <summary>
        /// Номер телефона.
        /// </summary>
        public string PhoneNumber { get; }
    }
}
