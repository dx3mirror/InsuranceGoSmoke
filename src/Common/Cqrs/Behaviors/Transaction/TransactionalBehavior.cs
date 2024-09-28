using MediatR;
using InsuranceGoSmoke.Common.Contracts.Abstract;
using InsuranceGoSmoke.Common.Infrastructures.DataAccess.Session;

namespace InsuranceGoSmoke.Common.Cqrs.Behaviors.Transaction
{
    /// <summary>
    /// Обработчик пайплайна, добавляющий транзакционность.
    /// </summary>
    /// <typeparam name="TRequest">Тип сообщения.</typeparam>
    /// <typeparam name="TResult">Тип результата.</typeparam>
    public class TransactionalBehavior<TRequest, TResult>(IDataSessionFactory _dataSessionFactory)
        : IPipelineBehavior<TRequest, TResult>
        where TRequest : notnull, IMessage
    {
        /// <inheritdoc />
        public async Task<TResult> Handle(TRequest request, RequestHandlerDelegate<TResult> next, CancellationToken cancellationToken)
        {
            // Транзакции поддерживаются только для команд
            if (request is not CommandBase)
            {
                return await next();
            }

            using var session = _dataSessionFactory.Create();
            var isExternalTransactionOpened = session.HasActiveTransaction();

            try
            {
                await StartTransaction(session, isExternalTransactionOpened, cancellationToken);

                var result = await next();

                session.CommitTransaction();

                return result;
            }
            catch (Exception outer)
            {
                RollBackTransaction(session, isExternalTransactionOpened, outer);

                // Проброс исходного исключения.
                throw;
            }
        }

        private static async Task StartTransaction(IDataSession session, bool isExternalTransactionOpened, CancellationToken cancellationToken)
        {
            if (isExternalTransactionOpened)
            {
                return;
            }

            await session.BeginTransactionAsync(cancellationToken);
        }

        private static void RollBackTransaction(IDataSession session, bool isExternalTransactionOpened, Exception outer)
        {
            try
            {
                RollbackTransaction(session, isExternalTransactionOpened);
            }
            catch (Exception inner)
            {
                throw new AggregateException(inner, outer);
            }
        }

        private static void RollbackTransaction(IDataSession session, bool isExternalTransactionOpened)
        {
            if (isExternalTransactionOpened)
            {
                return;
            }

            session.RollbackTransaction();
        }
    }
}
