using MediatR;
using FluentValidation;
using FluentValidation.Results;
using InsuranceGoSmoke.Common.Contracts.Abstract;

namespace InsuranceGoSmoke.Common.Cqrs.Behaviors.DataValidator
{
    /// <summary>
    /// Поведение валидации запросов.
    /// </summary>
    /// <typeparam name="TRequest">.Тип запроса.</typeparam>
    /// <typeparam name="TResult">Тип ответа.</typeparam>
    public class ValidationRequestBehavior<TRequest, TResult>(IEnumerable<IValidator<TRequest>> _validators)
        : IPipelineBehavior<TRequest, TResult>
        where TRequest : notnull, IMessage
    {
        /// <summary>
        /// Обработка.
        /// </summary>
        /// <param name="request">Запрос.</param>
        /// <param name="next">Следующая обработка.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Возвращает результат обработки.</returns>
        /// <exception cref="ValidationException">Исключение при ошибках валидации.</exception>
        public async Task<TResult> Handle(TRequest request,
            RequestHandlerDelegate<TResult> next, CancellationToken cancellationToken)
        {
            var context = new ValidationContext<TRequest>(request);
            var tasks = _validators.Select(validator => validator.ValidateAsync(context, cancellationToken));
            var validationFailures = await Task.WhenAll(tasks);

            var errors = validationFailures
                            .Where(validationResult => !validationResult.IsValid)
                            .SelectMany(validationResult => validationResult.Errors)
                            .Select(validationFailure => new ValidationFailure(
                                validationFailure.PropertyName,
                                validationFailure.ErrorMessage))
                            .ToList();

            if (errors.Count != 0)
            {
                var message = string.Join(Environment.NewLine, errors.Select(e => $"Поле '{e.PropertyName}': {e.ErrorMessage}"));
                throw new ValidationException(message);
            }

            return await next();
        }
    }
}
