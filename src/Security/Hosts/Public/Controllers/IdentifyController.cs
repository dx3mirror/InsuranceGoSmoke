using InsuranceGoSmoke.Common.Hosts.Api.Filters.RateLimit;
using InsuranceGoSmoke.Security.Applications.Handlers.Contexts.Identify.Commands.Login;
using InsuranceGoSmoke.Security.Applications.Handlers.Contexts.Identify.Commands.Logout;
using InsuranceGoSmoke.Security.Applications.Handlers.Contexts.Identify.Commands.SendAuthentificationCode;
using InsuranceGoSmoke.Security.Applications.Handlers.Contexts.Identify.Queries.GetAuthorizationConfig;
using InsuranceGoSmoke.Security.Applications.Handlers.Contexts.Identify.Queries.RefreshToken;
using InsuranceGoSmoke.Security.Contracts.Identify.Requests;
using InsuranceGoSmoke.Security.Contracts.Identify.Responses;
using InsuranceGoSmoke.Security.Public.Hosts.Api.Helpers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Configuration;

namespace InsuranceGoSmoke.Security.Public.Hosts.Api.Controllers
{
    /// <summary>
    /// API авторизации.
    /// </summary>
    /// <param name="_mediator">Обработчик сообщений.</param>
    [Route("identify")]
    [ApiController]
    public class IdentifyController(IMediator _mediator, IConfiguration _configuration) : ControllerBase
    {
        /// <summary>
        /// Возвращает конфигурацию авторизации.
        /// </summary>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Конфигурация авторизации.</returns>
        [HttpGet("config")]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        [ProducesResponseType(typeof(IdentifyConfiguration), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> GetConfig(CancellationToken cancellationToken)
        {
            var request = new GetAuthorizationConfigRequest();
            var result = await _mediator.Send(request, cancellationToken);
            return Ok(result);
        }

        /// <summary>
        /// Отправляет аутентификационный код в указанное место.
        /// </summary>
        /// <param name="destination">Точка назначения для кода авторизации.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        [HttpPost("code/send")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(string), StatusCodes.Status204NoContent)]
        [RateLimit(maxAttempts: 1, delaySeconds:50, uniqueParameter: nameof(destination))]
        public async Task<IActionResult> SendAuthentificationCode([FromQuery, BindRequired] string destination, CancellationToken cancellationToken)
        {
            var request = new SendAuthentificationCodeRequest(destination);
            await _mediator.Send(request, cancellationToken);
            return NoContent();
        }

        /// <summary>
        /// Аутентифицирует пользователя.
        /// </summary>
        /// <param name="loginData">Данные для аутентификации.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Токен доступа.</returns>
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginData loginData, CancellationToken cancellationToken)
        {
            var request = new LoginRequest(loginData);
            var result = await _mediator.Send(request, cancellationToken);
            var domain = _configuration["Domain"];
            CookieHelper.SetRefreshTokenCookie(result.RefreshToken, Response.Cookies, domain);
            CookieHelper.SetAccessTokenCookie(result.AccessToken, Response.Cookies, domain);

            return Ok(result.AccessToken);
        }


        /// <summary>
        /// Разлогин пользователя.
        /// </summary>
        /// <param name="cancellationToken">Токен отмены.</param>
        [HttpPost("logout")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(string), StatusCodes.Status204NoContent)]
        [Authorize]
        public async Task<IActionResult> Logout(CancellationToken cancellationToken)
        {
            var request = new LogoutRequest();
            await _mediator.Send(request, cancellationToken);
            CookieHelper.ClearCookie(CookieHelper.RefreshTokenCookieName, Response.Cookies);
            CookieHelper.ClearCookie(CookieHelper.AccessTokenCookieName, Response.Cookies);

            return NoContent();
        }

        /// <summary>
        /// Обновление токена
        /// </summary>
        [HttpGet("token/refresh")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken(CancellationToken cancellationToken)
        {
            Request.Cookies.TryGetValue(CookieHelper.RefreshTokenCookieName, out string? refreshToken);

            var result = await _mediator.Send(new RefreshTokenQuery(refreshToken), cancellationToken);
            var domain = _configuration["Domain"];
            CookieHelper.SetRefreshTokenCookie(result.RefreshToken, Response.Cookies, domain);
            CookieHelper.SetAccessTokenCookie(result.AccessToken, Response.Cookies, domain);

            return Ok(result.AccessToken);
        }
    }
}
