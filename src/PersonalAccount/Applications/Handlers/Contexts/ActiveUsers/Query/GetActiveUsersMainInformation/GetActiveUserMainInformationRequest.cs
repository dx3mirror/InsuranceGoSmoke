using InsuranceGoSmoke.Common.Contracts.Abstract;
using InsuranceGoSmoke.PersonalAccount.Contracts.Responce.ActiveUsers;

namespace InsuranceGoSmoke.PersonalAccount.Applications.Handlers.Contexts.ActiveUsers.Query.GetActiveUsersMainInformation
{
    /// <summary>
    /// Запрос на получение основной информации о пользователе.
    /// </summary>
    public class GetActiveUserMainInformationRequest : Query<UserMainInformationResponse>
    {
        /// <summary>
        /// Создаёт экземпляр <see cref="GetActiveUserMainInformationRequest"/>
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        public GetActiveUserMainInformationRequest(long userId)
        {
            UserId = userId;
        }

        /// <summary>
        /// Идентификатор пользователя.
        /// </summary>
        public long UserId { get; }

    }
}
