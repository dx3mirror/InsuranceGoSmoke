using InsuranceGoSmoke.Common.Infrastructures.DataAccess.Repositories;
using InsuranceGoSmoke.Common.Applications.AppServices.Extensions;
using InsuranceGoSmoke.PersonalAccount.Applications.AppServices.Contexts.ActiveUsers.ReceivingUsers.Services.Specifications;
using InsuranceGoSmoke.PersonalAccount.Domain.Account;
using Microsoft.EntityFrameworkCore;
using InsuranceGoSmoke.Common.Contracts.Exceptions.Common;
using InsuranceGoSmoke.PersonalAccount.Contracts.Responce.ActiveUsers;
using InsuranceGoSmoke.PersonalAccount.Applications.AppServices.Contexts.ActiveUsers.ReceivingUsers.Services.Abstracts;

namespace InsuranceGoSmoke.PersonalAccount.Applications.AppServices.Contexts.ActiveUsers.ReceivingUsers.Services
{
    /// <summary>
    /// Сервис для получения основной информации о активных пользователях.
    /// </summary>
    public class ReceivingActiveUsersServices : IReceivingActiveUsersServices
    {
        private readonly IRepository<User> _userRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReceivingActiveUsersServices"/> class.
        /// </summary>
        /// <param name="userRepository">Репозиторий пользователей.</param>
        public ReceivingActiveUsersServices(IRepository<User> userRepository)
    {
        _userRepository = userRepository;
    }

    /// <summary>
    /// Получает основную информацию о пользователе по его идентификатору.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <param name="cancellationToken">Токен отмены для асинхронной операции.</param>
    /// <returns>
    /// Объект <see cref="UserMainInformationResponse"/> с основной информацией о пользователе.
    /// </returns>
    /// <exception cref="NotFoundException">Выбрасывается, если пользователь не найден.</exception>
    public async Task<UserMainInformationResponse> GetActiveUsersMainInformationAsync(long userId, CancellationToken cancellationToken)
        {
            var combinedSpecification = new UserByIdSpecification(userId)
                .And(new UserAccessibleSpecification())
                .And(new UserNotBlockedSpecification())
                .And(new UserVisibleSpecification());

            var user = await _userRepository.AsQueryable()
                          .AsNoTracking()
                          .Where(combinedSpecification)
                          .Select(q => new UserMainInformationResponse
                          {
                              LastName = q.LastName,
                              FirstName = q.FirstName,
                              Email = q.Email,
                              DateOfBirth = q.DateOfBirth
                          })
                          .SingleOrDefaultAsync(cancellationToken) ?? throw new NotFoundException($"Пользователь с идентификатором {userId} не найден.");

            return user;
        }

        /// <summary>
        /// Получает настройки конфиденциальности пользователя по его идентификатору.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <param name="cancellationToken">Токен отмены для асинхронной операции.</param>
        /// <returns>Объект <see cref="UserPrivacySettingsResponse"/> с настройками конфиденциальности пользователя.</returns>
        /// <exception cref="NotFoundException">Выбрасывается, если пользователь не найден.</exception>
        public async Task<UserPrivacySettingsResponse> GetRulePrivacyUserAsync(long userId, CancellationToken cancellationToken)
        {
            var combinedSpecification = new UserByIdSpecification(userId)
                .And(new UserAccessibleSpecification())
                .And(new UserNotBlockedSpecification())
                .And(new UserVisibleSpecification());

            var privacySettings = await _userRepository.AsQueryable()
                          .AsNoTracking()
                          .Where(combinedSpecification)
                          .Select(q => new UserPrivacySettingsResponse
                          {
                              ShowBirthdate = q.PrivacySettings.ShowBirthdate,
                              ShowDescription = q.PrivacySettings.ShowDescription,
                              ShowEmail = q.PrivacySettings.ShowEmail
                          })
                          .SingleOrDefaultAsync(cancellationToken) ?? throw new NotFoundException($"Пользователь с идентификатором {userId} не найден.");

            return privacySettings;
        }

        /// <summary>
        /// Получает описание пользователя по его идентификатору.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <param name="cancellationToken">Токен отмены для асинхронной операции.</param>
        /// <returns>Объект <see cref="UserDescriptionResponse"/> с описанием пользователя.</returns>
        /// <exception cref="NotFoundException">Выбрасывается, если пользователь не найден.</exception>
        public async Task<UserDescriptionResponse> GetDescriptionUserAsync(long userId, CancellationToken cancellationToken)
        {
            var combinedSpecification = new UserByIdSpecification(userId)
                .And(new UserAccessibleSpecification())
                .And(new UserNotBlockedSpecification())
                .And(new UserVisibleSpecification());

            var description = await _userRepository.AsQueryable()
                  .AsNoTracking()
                  .Where(combinedSpecification)
                  .Select(q => new UserDescriptionResponse
                  {
                      SmokingExperienceYears = q.SmokingDescription.SmokingExperienceYears ?? 0,
                      ReasonStartedSmoking = q.SmokingDescription.ReasonStartedSmoking ?? string.Empty,
                      ReadyMeeting = q.SmokingDescription.ReadyMeeting,
                      IsSmoked = q.SmokingDescription.IsSmoked,
                      IsVape = q.SmokingDescription.IsVape,
                      About = q.SmokingDescription.About ?? string.Empty
                  })
                  .SingleOrDefaultAsync(cancellationToken) ?? throw new NotFoundException($"Пользователь с идентификатором {userId} не найден.");

            return description;
        }

        /// <summary>
        /// Получает настройки дизайна профиля пользователя по его идентификатору.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <param name="cancellationToken">Токен отмены для асинхронной операции.</param>
        /// <returns>Объект <see cref="UserProfileDesignResponse"/> с настройками дизайна профиля пользователя.</returns>
        /// <exception cref="NotFoundException">Выбрасывается, если пользователь не найден.</exception>
        public async Task<UserProfileDesignResponse> GetUserProfileDesignAsync(long userId, CancellationToken cancellationToken)
        {
            var combinedSpecification = new UserByIdSpecification(userId)
                .And(new UserAccessibleSpecification())
                .And(new UserNotBlockedSpecification())
                .And(new UserVisibleSpecification());

            var profileDesign = await _userRepository.AsQueryable()
                  .AsNoTracking()
                  .Where(combinedSpecification)
                  .Select(q => new UserProfileDesignResponse
                  {
                      ThemeColor = q.ProfileDesign.ThemeColor,
                      FontStyle = q.ProfileDesign.FontStyle,
                      BackgroundImage = q.ProfileDesign.BackgroundImage,
                      EnableAnimations = q.ProfileDesign.EnableAnimations
                  })
                  .SingleOrDefaultAsync(cancellationToken) ?? throw new NotFoundException($"Пользователь с идентификатором {userId} не найден.");

            return profileDesign;
        }

        /// <summary>
        /// Получает аватар пользователя по его идентификатору.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <param name="cancellationToken">Токен отмены для асинхронной операции.</param>
        /// <returns>Объект <see cref="UserAvatarResponse"/> с информацией об аватаре пользователя.</returns>
        /// <exception cref="NotFoundException">Выбрасывается, если пользователь не найден.</exception>
        public async Task<UserAvatarResponse> GetUserAvatarAsync(long userId, CancellationToken cancellationToken)
        {
            var combinedSpecification = new UserByIdSpecification(userId)
                .And(new UserAccessibleSpecification())
                .And(new UserNotBlockedSpecification())
                .And(new UserVisibleSpecification());

            var avatar = await _userRepository.AsQueryable()
                  .AsNoTracking()
                  .Where(combinedSpecification)
                  .Select(q => new UserAvatarResponse
                  {
                      ImageData = q.Avatar.ImageData,
                      IsVisible = q.Avatar.IsVisible
                  })
                  .SingleOrDefaultAsync(cancellationToken) ?? throw new NotFoundException($"Пользователь с идентификатором {userId} не найден.");

            return avatar;
        }
    }
}
