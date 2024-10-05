using InsuranceGoSmoke.Common.Contracts.Abstract;
using InsuranceGoSmoke.PersonalAccount.Contracts.Responce.ActiveUsers;

namespace InsuranceGoSmoke.PersonalAccount.Applications.Handlers.Contexts.ActiveUsers.Query.GetUserAvatar
{
    /// <summary>
    /// Запрос на получение аватара пользователя.
    /// </summary>
    public class GetUserAvatarRequest : Query<UserAvatarResponse>
    {
        /// <summary>
        /// Создаёт экземпляр <see cref="GetUserAvatarRequest"/>
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        public GetUserAvatarRequest(long userId)
        {
            UserId = userId;
        }

        /// <summary>
        /// Идентификатор пользователя.
        /// </summary>
        public long UserId { get; }
    }
}
