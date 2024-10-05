using InsuranceGoSmoke.Common.Contracts.Abstract;
using InsuranceGoSmoke.PersonalAccount.Contracts.Responce.ActiveUsers;

namespace InsuranceGoSmoke.PersonalAccount.Applications.Handlers.Contexts.ActiveUsers.Query.GetRulePrivacyUser
{
    /// <summary>
    /// Запрос на получение настроек конфиденциальности пользователя.
    /// </summary>
    public class GetUserPrivacySettingsRequest : Query<UserPrivacySettingsResponse>
    {
        /// <summary>
        /// Создаёт экземпляр <see cref="GetUserPrivacySettingsRequest"/>
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        public GetUserPrivacySettingsRequest(long userId)
        {
            UserId = userId;
        }

        /// <summary>
        /// Идентификатор пользователя.
        /// </summary>
        public long UserId { get; }
    }

}
