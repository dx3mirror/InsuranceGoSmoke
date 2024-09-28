using InsuranceGoSmoke.Common.Applications.Handlers.Abstract;
using InsuranceGoSmoke.Security.Applications.AppServices.Contexts.Users.Services.Abstract;

namespace InsuranceGoSmoke.Security.Applications.Handlers.Contexts.Users.Commands.ChangePhoneNumber
{
    /// <summary>
    /// Обработчик команды <see cref="UpdatePhoneNumberRequest"/>
    /// </summary>
    public class UpdatePhoneNumberRequestHandler : ICommandHandler<UpdatePhoneNumberRequest>
    {
        private readonly IUserService _userService;

        /// <summary>
        /// Создаёт экземпляр <see cref="UpdatePhoneNumberRequestHandler"/>
        /// </summary>
        /// <param name="userService">Сервис работы с пользователем.</param>
        public UpdatePhoneNumberRequestHandler(IUserService userService)
        {
            _userService = userService;
        }

        /// <inheritdoc/>
        public Task Handle(UpdatePhoneNumberRequest request, CancellationToken cancellationToken)
        {
            return _userService.UpdatePhoneNumberAsync(request.UserId, request.PhoneNumber, cancellationToken);
        }
    }
}
