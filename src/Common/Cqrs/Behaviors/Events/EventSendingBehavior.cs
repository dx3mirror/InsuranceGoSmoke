using MediatR;
using InsuranceGoSmoke.Common.Applications.Handlers.Abstract;
using InsuranceGoSmoke.Common.Contracts.Abstract;
using InsuranceGoSmoke.Common.Contracts.Exceptions.Common;
using Microsoft.Extensions.Logging;

namespace InsuranceGoSmoke.Common.Cqrs.Behaviors.Events
{
    /// <summary>
    /// Поведение отвественное за отправку событий.
    /// </summary>
    /// <typeparam name="TRequest">Тип запроса.</typeparam>
    /// <typeparam name="TResult">Тип ответа.</typeparam>
    /// <param name="_eventMessageProvider">Провайдер событий.</param>
    /// <param name="_provider">Провайдер сервисов.</param>
    /// <param name="_logger">Логгер.</param>
    public class EventSendingBehavior<TRequest, TResult>(
        IEventMessageProvider _eventMessageProvider,
        IServiceProvider _provider,
        ILogger<EventSendingBehavior<TRequest, TResult>> _logger)
        : IPipelineBehavior<TRequest, TResult>
        where TRequest : notnull, IMessage
    {
        /// <inheritdoc />
        public async Task<TResult> Handle(TRequest request, RequestHandlerDelegate<TResult> next, CancellationToken cancellationToken)
        {
            var result = await next();

            // События вне транзакций только для команд, т.к. транзакции только в командах.
            if (request is not CommandBase)
            {
                return result;
            }

            try
            {
                var @event = _eventMessageProvider.Get();
                while (@event != null)
                {
                    Type genericType = typeof(Applications.Handlers.Abstract.EventHandler<>).MakeGenericType(@event.GetType());
                    if (_provider.GetService(genericType) is IEventHandler handler)
                    {
                        await handler.HandleAsync(@event, cancellationToken);
                    }
                    @event = _eventMessageProvider.Get();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Произошла ошибка при отправке события {Request}", typeof(TRequest));
                throw new ReadableException("Произошла ошибка при отправке события после выполнения операции.", ex);
            }

            return result;
        }
    }
}
