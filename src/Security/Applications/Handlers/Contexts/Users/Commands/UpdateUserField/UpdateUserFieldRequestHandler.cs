using InsuranceGoSmoke.Common.Applications.Handlers.Abstract;
using InsuranceGoSmoke.Common.Clients.Keycloak.Models;
using InsuranceGoSmoke.Common.Contracts.Utilities.Helpers;
using InsuranceGoSmoke.Common.Cqrs.Behaviors.Events;
using InsuranceGoSmoke.Security.Applications.AppServices.Contexts.Users.Services.Abstract;
using InsuranceGoSmoke.Security.Applications.Handlers.Contexts.Users.Events.EmailConfirmNotification;
using System.Diagnostics.Tracing;

namespace InsuranceGoSmoke.Security.Applications.Handlers.Contexts.Users.Commands.UpdateUserField
{
    /// <summary>
    /// Обработчик команды <see cref="UpdateUserFieldRequest"/>
    /// </summary>
    public class UpdateUserFieldRequestHandler : ICommandHandler<UpdateUserFieldRequest>
    {
        private readonly IUserService _userService;
        private readonly IEventMessageProvider _eventMessageProvider;

        /// <summary>
        /// Создаёт экземпляр <see cref="UpdateUserFieldRequestHandler"/>
        /// </summary>
        /// <param name="userService">Сервис работы с пользователем.</param>
        /// <param name="eventMessageProvider">Провайдер событий.</param>
        public UpdateUserFieldRequestHandler(
            IUserService userService,
            IEventMessageProvider eventMessageProvider)
        {
            _userService = userService;
            _eventMessageProvider = eventMessageProvider;
        }

        /// <inheritdoc/>
        public async Task Handle(UpdateUserFieldRequest request, CancellationToken cancellationToken)
        {
            await _userService.UpdateUserFieldAsync(request.UserId, request.Field, request.Value, cancellationToken);

            if (request.Field.FirstCharToUpper() == nameof(KeycloakUserData.Email))
            {
                _eventMessageProvider.Add(new EmailConfirmNotificationEvent(request.UserId, request.Value.GetString()!));
            }
        }
    }
}
