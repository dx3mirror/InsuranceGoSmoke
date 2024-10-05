using InsuranceGoSmoke.Common.Infrastructures.DataAccess.Repositories;
using InsuranceGoSmoke.PersonalAccount.Domain.Account;
using InsuranceGoSmoke.PersonalAccount.Domain;
using InsuranceGoSmoke.PersonalAccount.Contracts.Request.ControlActiveUsers;
using InsuranceGoSmoke.Common.Contracts.Exceptions.Common;
using Microsoft.EntityFrameworkCore;
using InsuranceGoSmoke.PersonalAccount.Applications.AppServices.Contexts.ActiveUsers.ReceivingUsers.Services.Abstracts;

namespace InsuranceGoSmoke.PersonalAccount.Applications.AppServices.Contexts.ActiveUsers.ReceivingUsers.Services
{
    /// <summary>
    /// Сервис для управления активными пользователями, включая создание, обновление и 
    /// изменение статуса аккаунтов, конфиденциальности и других связанных данных пользователей.
    /// </summary>
    public class ControlActiveUsersService : IControlActiveUsersService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<AccountStatus> _accountStatusRepository;
        private readonly IRepository<PrivacySettings> _privacySettingsRepository;
        private readonly IRepository<Avatar> _avatarRepository;
        private readonly IRepository<ProfileDesign> _profileDesignRepository;
        private readonly IRepository<SmokingDescription> _smokingDescriptionRepository;

        /// <summary>
        /// Конструктор класса ControlActiveUsersService.
        /// </summary>
        /// <param name="userRepository">Репозиторий для работы с пользователями.</param>
        /// <param name="accountStatusRepository">Репозиторий для работы со статусами аккаунтов.</param>
        /// <param name="privacySettingsRepository">Репозиторий для работы с настройками конфиденциальности.</param>
        /// <param name="avatarRepository">Репозиторий для работы с аватарами.</param>
        /// <param name="profileDesignRepository">Репозиторий для работы с дизайном профиля.</param>
        /// <param name="smokingDescriptionRepository">Репозиторий для работы с описанием курильщиков.</param>
        public ControlActiveUsersService(IRepository<User> userRepository,
                           IRepository<AccountStatus> accountStatusRepository,
                           IRepository<PrivacySettings> privacySettingsRepository,
                           IRepository<Avatar> avatarRepository,
                           IRepository<ProfileDesign> profileDesignRepository,
                           IRepository<SmokingDescription> smokingDescriptionRepository)
        {
            _userRepository = userRepository;
            _accountStatusRepository = accountStatusRepository;
            _privacySettingsRepository = privacySettingsRepository;
            _avatarRepository = avatarRepository;
            _profileDesignRepository = profileDesignRepository;
            _smokingDescriptionRepository = smokingDescriptionRepository;
        }

        /// <summary>
        /// Создает нового пользователя вместе с его деталями (статусом аккаунта, настройками конфиденциальности).
        /// </summary>
        /// <param name="userRequest">Запрос на создание пользователя.</param>
        /// <param name="accountStatusRequest">Запрос на создание статуса аккаунта.</param>
        /// <param name="privacySettingsRequest">Запрос на создание настроек конфиденциальности.</param>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        public async Task CreateUserWithDetailsAsync(UserRequest userRequest,
            AccountStatusRequest accountStatusRequest,
            PrivacySettingsRequest privacySettingsRequest,
            CancellationToken cancellationToken)
        {
            using var transaction = await _userRepository.BeginTransactionAsync(cancellationToken);
            try
            {
                var user = new User
                {
                    ClientGuid = userRequest.ClientGuid,
                    FirstName = userRequest.FirstName,
                    LastName = userRequest.LastName,
                    DateOfBirth = userRequest.DateOfBirth,
                    Email = userRequest.Email
                };

                await _userRepository.AddAsync(user, cancellationToken);

                var accountStatus = new AccountStatus(user.Id)
                {
                    ClientId = user.Id,
                    IsVisible = accountStatusRequest.IsVisible,
                    IsAccessible = accountStatusRequest.IsAccessible,
                    IsBlocked = accountStatusRequest.IsBlocked,
                    IsPremium = accountStatusRequest.IsPremium
                };

                await _accountStatusRepository.AddAsync(accountStatus, cancellationToken);

                var privacySettings = new PrivacySettings(user.Id)
                {
                    ClientId = user.Id,
                    ShowEmail = privacySettingsRequest.ShowEmail,
                    ShowBirthdate = privacySettingsRequest.ShowBirthdate,
                    ShowDescription = privacySettingsRequest.ShowDescription
                };

                await _privacySettingsRepository.AddAsync(privacySettings, cancellationToken);

                await _userRepository.CommitTransactionAsync(transaction, cancellationToken);
            }
            catch
            {
                await _userRepository.RollbackTransactionAsync(transaction, cancellationToken);
                throw;
            }
        }

        /// <summary>
        /// Добавляет аватар для указанного клиента.
        /// </summary>
        /// <param name="clientId">Идентификатор клиента.</param>
        /// <param name="imageData">Данные изображения аватара.</param>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        public async Task AddAvatarAsync(long clientId, Guid imageData, CancellationToken cancellationToken)
        {
            var avatar = new Avatar(clientId, imageData) { ImageData = imageData };

            await _avatarRepository.AddAsync(avatar, cancellationToken);
        }

        /// <summary>
        /// Добавляет настройки дизайна профиля для указанного клиента.
        /// </summary>
        /// <param name="request">Запрос на добавление настроек дизайна профиля.</param>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        public async Task AddProfileDesignAsync(ProfileDesignRequest request, CancellationToken cancellationToken)
        {
            var profileDesign = new ProfileDesign(request.ClientId)
            {
                ThemeColor = request.ThemeColor,
                BackgroundImage = request.BackgroundImage,
                FontStyle = request.FontStyle,
                EnableAnimations = request.EnableAnimations
            };

            await _profileDesignRepository.AddAsync(profileDesign, cancellationToken);
        }

        /// <summary>
        /// Добавляет описание курильщика для указанного клиента.
        /// </summary>
        /// <param name="request">Запрос на добавление описания курильщика.</param>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        public async Task AddSmokingDescriptionAsync(SmokingDescriptionRequest request, CancellationToken cancellationToken)
        {
            var smokingDescription = new SmokingDescription(request.ClientId)
            {
                ClientId = request.ClientId,
                SmokingExperienceYears = request.SmokingExperienceYears,
                ReasonStartedSmoking = request.ReasonStartedSmoking,
                IsSmoked = request.IsSmoked,
                IsVape = request.IsVape,
                IsDrink = request.IsDrink,
                ReadyMeeting = request.ReadyMeeting,
                About = request.About
            };

            await _smokingDescriptionRepository.AddAsync(smokingDescription, cancellationToken);
        }

        /// <summary>
        /// Обновляет описание курильщика для указанного клиента.
        /// </summary>
        /// <param name="request">Запрос на обновление описания курильщика.</param>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        public async Task UpdateSmokingDescriptionAsync(SmokingDescriptionRequest request, CancellationToken cancellationToken)
        {
            var smokingDescription = await _smokingDescriptionRepository.AsQueryable()
                                                 .SingleOrDefaultAsync(sd => sd.ClientId == request.ClientId, cancellationToken)
                                                 ?? throw new NotFoundException($"Smoking description for client with Id {request.ClientId} not found.");

            smokingDescription.SmokingExperienceYears = request.SmokingExperienceYears;
            smokingDescription.ReasonStartedSmoking = request.ReasonStartedSmoking;
            smokingDescription.IsSmoked = request.IsSmoked;
            smokingDescription.IsVape = request.IsVape;
            smokingDescription.IsDrink = request.IsDrink;
            smokingDescription.ReadyMeeting = request.ReadyMeeting;
            smokingDescription.About = request.About;

            await _smokingDescriptionRepository.UpdateAsync(smokingDescription, cancellationToken);
        }

        /// <summary>
        /// Обновляет настройки дизайна профиля для указанного клиента.
        /// </summary>
        /// <param name="request">Запрос на обновление настроек дизайна профиля.</param>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        public async Task UpdateProfileDesignAsync(ProfileDesignRequest request, CancellationToken cancellationToken)
        {
            var profileDesign = await _profileDesignRepository.AsQueryable()
                                                 .SingleOrDefaultAsync(pd => pd.ClientId == request.ClientId, cancellationToken)
                                                 ?? throw new NotFoundException($"Profile design for client with Id {request.ClientId} not found.");

            profileDesign.ThemeColor = request.ThemeColor;
            profileDesign.BackgroundImage = request.BackgroundImage;
            profileDesign.FontStyle = request.FontStyle;
            profileDesign.EnableAnimations = request.EnableAnimations;

            await _profileDesignRepository.UpdateAsync(profileDesign, cancellationToken);
        }

        /// <summary>
        /// Обновляет аватар для указанного клиента.
        /// </summary>
        /// <param name="clientId">Идентификатор клиента.</param>
        /// <param name="newImageData">Новые данные изображения аватара.</param>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        public async Task UpdateAvatarAsync(long clientId, Guid newImageData, CancellationToken cancellationToken)
        {
            var avatar = await _avatarRepository.AsQueryable()
                                                 .SingleOrDefaultAsync(a => a.ClientId == clientId, cancellationToken)
                                                 ?? throw new NotFoundException($"Avatar for client with Id {clientId} not found.");

            avatar.ImageData = newImageData;

            await _avatarRepository.UpdateAsync(avatar, cancellationToken);
        }

        /// <summary>
        /// Обновляет данные пользователя.
        /// </summary>
        /// <param name="userRequest">Запрос на обновление данных пользователя.</param>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        public async Task UpdateUserAsync(UserRequest userRequest, CancellationToken cancellationToken)
        {
            var user = await _userRepository.AsQueryable()
                                             .SingleOrDefaultAsync(u => u.Id == userRequest.Id, cancellationToken)
                                             ?? throw new NotFoundException($"User with Id {userRequest.Id} not found.");

            user.FirstName = userRequest.FirstName;
            user.LastName = userRequest.LastName;
            user.DateOfBirth = userRequest.DateOfBirth;
            user.Email = userRequest.Email;

            await _userRepository.UpdateAsync(user, cancellationToken);
        }
    }
}
