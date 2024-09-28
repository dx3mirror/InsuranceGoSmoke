using MediatR;

namespace InsuranceGoSmoke.Common.Applications.Handlers.Abstract
{
    /// <summary>
    /// Обработчик валидации доступа.
    /// </summary>
    /// <typeparam name="TRequest">Тип запроса.</typeparam>
    public interface IAccessValidatorHandler<in TRequest>
        where TRequest : IBaseRequest
    {
        /// <summary>
        /// Проверяет доступ.
        /// </summary>
        /// <param name="request">Запрос.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        Task ValidateAsync(TRequest request, CancellationToken cancellationToken);
    }

    /// <summary>
    /// Обработчик валидации доступа.
    /// </summary>
    /// <typeparam name="TRequest">Тип запроса.</typeparam>
    /// <typeparam name="TResponse">Тип ответа.</typeparam>
    public interface IAccessValidatorHandler<in TRequest, in TResponse>
        where TRequest : IBaseRequest
    {
        /// <summary>
        /// Проверяет доступ.
        /// </summary>
        /// <param name="request">Запрос.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        Task ValidateAsync(TRequest request, CancellationToken cancellationToken);
    }
}
