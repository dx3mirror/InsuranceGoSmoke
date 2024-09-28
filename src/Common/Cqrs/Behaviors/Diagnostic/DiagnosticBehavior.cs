using FluentValidation;
using MediatR;
using InsuranceGoSmoke.Common.Applications.Handlers.Abstract;
using InsuranceGoSmoke.Common.Contracts.Abstract;
using OpenTelemetry.Trace;
using System.Diagnostics;

namespace InsuranceGoSmoke.Common.Cqrs.Behaviors.Diagnostic
{
    /// <summary>
    /// Поведение диагностики запросов.
    /// </summary>
    /// <typeparam name="TRequest">.Тип запроса.</typeparam>
    /// <typeparam name="TResponse">Тип ответа.</typeparam>
    public class DiagnosticBehavior<TRequest, TResponse>(
        IEnumerable<IDiagnosticHandler<TRequest, TResponse>> _handlersWithResult,
        IEnumerable<IDiagnosticHandler<TRequest>> _handlersWithoutResult)
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull, IMessage, IBaseRequest
    {
        /// <summary>
        /// Обработка.
        /// </summary>
        /// <param name="request">Запрос.</param>
        /// <param name="next">Следующая обработка.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Возвращает результат обработки.</returns>
        /// <exception cref="ValidationException">Исключение при ошибках валидации.</exception>
        public async Task<TResponse> Handle(TRequest request,
            RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            TResponse response;
            var activities = new List<Activity?>();

            try
            {
                PreHandle(request, activities);

                response = await next();

                PostHandle(request, response, activities);
            }
            catch (Exception ex)
            {
                foreach (var activity in activities)
                {
                    activity?.RecordException(ex);
                    activity?.SetStatus(ActivityStatusCode.Error);
                }
                throw;
            }
            finally
            {
                foreach (var activity in activities)
                {
                    activity?.Dispose();
                }
            }

            return response;
        }

        private void PreHandle(TRequest request, List<Activity?> activities)
        {
            foreach (var handler in _handlersWithResult)
            {
                var activity = handler.PreHandle(request);
                activities.Add(activity);
            }

            foreach (var handler in _handlersWithoutResult)
            {
                var activity = handler.PreHandle(request);
                activities.Add(activity);
            }
        }
        private void PostHandle(TRequest request, TResponse response, List<Activity?> activities)
        {
            foreach (var (index, handler) in _handlersWithResult.Select((handler, index) => (index, handler)))
            {
                var activity = activities[index];
                handler?.PostHandle(activity, request, response);
            }

            var indexForWithoutResult = _handlersWithResult.Count();
            foreach (var (index, handler) in _handlersWithoutResult.Select((handler, index) => (index, handler)))
            {
                var activity = activities[indexForWithoutResult + index];
                handler?.PostHandle(activity, request);
            }
        }

    }
}
