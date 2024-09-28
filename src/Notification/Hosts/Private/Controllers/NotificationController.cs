using InsuranceGoSmoke.Common.Hosts.Attributes.TrustedNetwork;
using InsuranceGoSmoke.Notification.Applications.Handlers.Contexts.Notifications.Commands.SendEmail;
using InsuranceGoSmoke.Notification.Contracts.Notifications.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace InsuranceGoSmoke.Notification.Public.Hosts.Api.Controllers
{
    /// <summary>
    /// API уведомлений.
    /// </summary>
    /// <param name="_mediator">Обработчик сообщений.</param>
    [Route("notification")]
    [ApiController]
    [TrustedNetwork]
    public class NotificationController(IMediator _mediator) : ControllerBase
    {
        /// <summary>
        /// Отправляет письмо для подтверждения Email.
        /// </summary>
        /// <param name="data">Данные.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        [HttpPost("email/send")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> SendEmail(EmailNotificationData data, CancellationToken cancellationToken)
        {
            var request = new SendEmailRequest(data);
            await _mediator.Send(request, cancellationToken);
            return NoContent();
        }
    }
}
