using InsuranceGoSmoke.Common.Contracts.Abstract;
using InsuranceGoSmoke.PersonalAccount.Contracts.Responce.ActiveUsers;

namespace InsuranceGoSmoke.PersonalAccount.Applications.Handlers.Contexts.ActiveUsers.Query.GetUserProfileDesign
{
    /// <summary>
    /// Запрос на получение настроек дизайна профиля пользователя.
    /// </summary>
    public class GetUserProfileDesignRequest : Query<UserProfileDesignResponse>
    {
        /// <summary>
        /// Создаёт экземпляр <see cref="GetUserProfileDesignRequest"/>
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        public GetUserProfileDesignRequest(long userId)
        {
            UserId = userId;
        }

        /// <summary>
        /// Идентификатор пользователя.
        /// </summary>
        public long UserId { get; }
    }
}
