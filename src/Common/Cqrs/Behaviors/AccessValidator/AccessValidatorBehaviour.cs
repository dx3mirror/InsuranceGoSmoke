using InsuranceGoSmoke.Common.Applications.Handlers.Abstract;
using InsuranceGoSmoke.Common.Contracts.Abstract;
using MediatR;

namespace InsuranceGoSmoke.Common.Cqrs.Behaviors.AccessValidator
{
    /// <summary>
    /// Обработчик пайплайна, добавляющий транзакционность.
    /// </summary>
    /// <typeparam name="TRequest">Тип сообщения.</typeparam>
    /// <typeparam name="TResult">Тип результата.</typeparam>
    public class AccessValidatorBehaviour<TRequest, TResult>(IEnumerable<IAccessValidatorHandler<TRequest>> _validators)
        : IPipelineBehavior<TRequest, TResult>
        where TRequest : notnull, IMessage, IBaseRequest
    {
        /// <inheritdoc />
        public async Task<TResult> Handle(TRequest request, RequestHandlerDelegate<TResult> next, CancellationToken cancellationToken)
        {
            var tasks = _validators.Select(validator => validator.ValidateAsync(request, cancellationToken));

            await Task.WhenAll(tasks);
            return await next();
        }
    }
}
