using InsuranceGoSmoke.Common.Applications.Handlers.Abstract;
using InsuranceGoSmoke.Common.Cqrs.Behaviors.Events;
using InsuranceGoSmoke.Security.Applications.AppServices.Contexts.Users.Models;
using InsuranceGoSmoke.Security.Applications.AppServices.Contexts.Users.Services.Abstract;
using InsuranceGoSmoke.Security.Applications.Handlers.Contexts.Users.Events.EmailConfirmNotification;

namespace InsuranceGoSmoke.Security.Applications.Handlers.Contexts.Users.Commands.UpdateUser
{
    /// <summary>
    /// Обработчик команды <see cref="UpdateUserRequest"/>
    /// </summary>
    public class UpdateUserRequestHandler : ICommandHandler<UpdateUserRequest>
    {
        private readonly IUserService _userService;
        private readonly IEventMessageProvider _eventMessageProvider;

        /// <summary>
        /// Создаёт экземпляр <see cref="UpdateUserRequestHandler"/>
        /// </summary>
        /// <param name="userService">Сервис работы с пользователем.</param>
        /// <param name="eventMessageProvider">Провайдер работы с событиями.</param>
        public UpdateUserRequestHandler(
            IUserService userService, IEventMessageProvider eventMessageProvider)
        {
            _userService = userService;
            _eventMessageProvider = eventMessageProvider;
        }

        /// <inheritdoc/>
        public async Task Handle(UpdateUserRequest request, CancellationToken cancellationToken)
        {
            var isEmailChanged = await IsEmailChangedAsync(request.UserId, request.Email, cancellationToken);

            var user = new UserModel(request.UserId);
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.BirthDate = request.BirthDate;
            user.Sex = request.Sex;
            user.Email = request.Email;
            await _userService.UpdateUserAsync(user, cancellationToken);

            if (isEmailChanged)
            {
                _eventMessageProvider.Add(new EmailConfirmNotificationEvent(request.UserId, request.Email!));
            }
        }

        private async Task<bool> IsEmailChangedAsync(Guid userId, string? email, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(email))
            {
                return false;
            }

            var user = await _userService.GetUserAsync(userId, cancellationToken);
            if (user == null) 
            { 
                return false;
            }

            return user.Email != email;
        }
    }
}
