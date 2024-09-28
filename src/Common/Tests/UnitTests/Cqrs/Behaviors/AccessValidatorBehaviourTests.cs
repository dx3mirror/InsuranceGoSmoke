using InsuranceGoSmoke.Common.Applications.Handlers.Abstract;
using InsuranceGoSmoke.Common.Contracts.Abstract;
using InsuranceGoSmoke.Common.Contracts.Exceptions.Authorization;
using InsuranceGoSmoke.Common.Cqrs.Behaviors.AccessValidator;
using MediatR;
using Moq;
using System.Diagnostics;

namespace InsuranceGoSmoke.Common.Tests.UnitTests.Cqrs.Behaviors
{
    /// <summary>
    /// Тесты поведения проверки доступа.
    /// </summary>
    public class AccessValidatorBehaviourTests
    {
        public class Request : IMessage, IBaseRequest
        {
            public Guid CorrelationId { get; set; }
        }

        [Test(Description = "Если список валидаторов пустой, то не должно быть никаких исключений")]
        public void Handle_ValidatorsIsEmpty_NoExceptions()
        {
            // Arrange
            IEnumerable<RoleAccessValidator<Request>> validators = [];
            var behavior = new AccessValidatorBehaviour<Request, object>(validators);
            var request = new Request();
            var response = new Mock<object>();

            //Act
            //Assert
            Assert.DoesNotThrowAsync(async () =>
            {
                await behavior.Handle(request, () => Task.FromResult(response.Object), CancellationToken.None);
            });
        }

        [Test(Description = "Если валидатор выбрасывает исключение, то исключение должно пробрасываться")]
        public void Handle_ValidatorThrowException_AccessDeniedException()
        {
            // Arrange
            var request = new Request();
            var handler = new Mock<IAccessValidatorHandler<Request>>();
            handler.Setup(h => h.ValidateAsync(request, CancellationToken.None))
                   .ThrowsAsync(new AccessDeniedException("Ошибка"));
            IEnumerable<IAccessValidatorHandler<Request>> validators = [handler.Object];
            var behavior = new AccessValidatorBehaviour<Request, object>(validators);
            var response = new Mock<object>();

            //Act
            var exception = Assert.ThrowsAsync<AccessDeniedException>(async () =>
            {
                await behavior.Handle(request, () => Task.FromResult(response.Object), CancellationToken.None);
            });

            //Assert
            Assert.That(exception, Is.Not.Null);
        }

        [Test(Description = "Если валидатор завершает без ошибок, то не должно быть исключений")]
        public void Handle_ValidatorCompleteSuccessed_NoException()
        {
            // Arrange
            var request = new Request();
            var handler = new Mock<IAccessValidatorHandler<Request>>();
            IEnumerable<IAccessValidatorHandler<Request>> validators = [handler.Object];
            var behavior = new AccessValidatorBehaviour<Request, object>(validators);
            var response = new Mock<object>();

            //Act
            //Assert
            Assert.DoesNotThrowAsync(async () =>
            {
                await behavior.Handle(request, () => Task.FromResult(response.Object), CancellationToken.None);
            });
        }
    }
}
