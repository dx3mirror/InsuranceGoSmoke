using MediatR;
using InsuranceGoSmoke.Common.Applications.Handlers.Abstract;
using InsuranceGoSmoke.Common.Contracts.Abstract;
using InsuranceGoSmoke.Common.Cqrs.Behaviors.Diagnostic;
using Moq;
using System.Diagnostics;

namespace InsuranceGoSmoke.Common.Tests.UnitTests.Cqrs.Behaviors
{
    /// <summary>
    /// Тесты поведения диагностики.
    /// </summary>
    internal class DiagnosticBehaviorTests
    {
        public class Request : IMessage, IBaseRequest
        {
            public Exception Exception { get; set; }
            public Guid CorrelationId { get; set; }
        }

        public class RequestResultDiagnosticHandler : IDiagnosticHandler<Request, object>
        {
            public Activity? PreHandle(Request request)
            {
                return new Activity("operationName");
            }

            public void PostHandle(Activity? actiity, Request request, object response)
            {
                if(request.Exception is not null)
                {
                    throw request.Exception;
                }
            }
        }

        public class RequestDiagnosticHandler : IDiagnosticHandler<Request>
        {
            public Activity? PreHandle(Request request)
            {
                return new Activity("operationName");
            }

            public void PostHandle(Activity? actiity, Request request)
            {
            }
        }

        [Test(Description = "При обработке валидного запроса, результат не должен быть неопределенным")]
        public async Task Handle_ValidRequest_ResultIsNotNull()
        {
            // Arrange
            var handlersWithResult = new RequestResultDiagnosticHandler();
            var handlersWithoutResult = new RequestDiagnosticHandler();
            var behaivor = new DiagnosticBehavior<Request, object>([handlersWithResult], [handlersWithoutResult]);
            var request = new Request();
            var response = new Mock<object>();

            // Act
            var result =  await behaivor.Handle(request, () => Task.FromResult(response.Object), CancellationToken.None);

            // Assert
            Assert.That(result, Is.Not.Null);
        }

        [Test(Description = "При обработке запроса выбрасывается исключение, исключение не должно оборачиваться")]
        public void Handle_NotImplementedException_Exception()
        {
            // Arrange
            var handlersWithResult = new RequestResultDiagnosticHandler();
            var handlersWithoutResult = new RequestDiagnosticHandler();
            var behaivor = new DiagnosticBehavior<Request, object>([handlersWithResult], [handlersWithoutResult]);
            var request = new Request() { Exception = new NotImplementedException() };
            var response = new Mock<object>();

            // Act
            var exception = Assert.ThrowsAsync<NotImplementedException>(async () => 
                                await behaivor.Handle(request, () => Task.FromResult(response.Object), CancellationToken.None));

            // Assert
            Assert.That(exception, Is.Not.Null);
        }
    }
}
