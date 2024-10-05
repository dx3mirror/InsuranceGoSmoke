using InsuranceGoSmoke.Common.Contracts.Abstract;
using InsuranceGoSmoke.PersonalAccount.Contracts.Responce.ActiveUsers;

namespace InsuranceGoSmoke.PersonalAccount.Applications.Handlers.Contexts.ActiveUsers.Query.GetDescriptionUser
{
    /// <summary>
    /// Запрос на получение описания пользователя.
    /// </summary>
    public class GetUserDescriptionRequest : Query<UserDescriptionResponse>
    {
        /// <summary>
        /// Создаёт экземпляр <see cref="GetUserDescriptionRequest"/>
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        public GetUserDescriptionRequest(long userId)
        {
            UserId = userId;
        }

        /// <summary>
        /// Идентификатор пользователя.
        /// </summary>
        public long UserId { get; }
    }
}
