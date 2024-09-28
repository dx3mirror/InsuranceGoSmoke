using InsuranceGoSmoke.Common.Contracts.Abstract;
using InsuranceGoSmoke.Common.Contracts.Exceptions.Common;
using InsuranceGoSmoke.Common.Cqrs.Behaviors.Events;
using Microsoft.Extensions.Logging;
using Moq;

namespace InsuranceGoSmoke.Common.Tests.UnitTests.Cqrs.Behaviors
{
    /// <summary>
    /// Тесты поведения отправки событий.
    /// </summary>
    internal class EventSendingBehaviorTests
    {
        internal class TestEvent : IEvent
        {
            public Guid CorrelationId { get; set; }
        }

        internal class TestEventHandler : Applications.Handlers.Abstract.EventHandler<TestEvent>
        {
            private bool isCompleted;
            public bool IsCompleted => isCompleted;

            public override Task HandleAsync(TestEvent @event, CancellationToken cancellationToken)
            {
                isCompleted = true;
                return Task.CompletedTask;
            }
        }

        internal class ExceptionEventHandler : Applications.Handlers.Abstract.EventHandler<TestEvent>
        {
            public override Task HandleAsync(TestEvent @event, CancellationToken cancellationToken)
            {
                throw new ReadableException("Произошла ошибка при отправке события после выполнения операции.");
            }
        }

        [Test(Description = "Если запрос не является командой, то не должно быть никаких вызовов")]
        public async Task Handle_RequestIsNotCommand_PublishWillBeCalledNever()
        {
            // Arrange
            var eventMessageProvider = new Mock<IEventMessageProvider>();
            var logger = new Mock<ILogger<EventSendingBehavior<IMessage, object>>>();
            var serviceProvider = new Mock<IServiceProvider>();
            var behavior = new EventSendingBehavior<IMessage, object>(eventMessageProvider.Object, serviceProvider.Object, logger.Object);
            var request = new Mock<IMessage>();
            var response = new Mock<object>();

            //Act
            await behavior.Handle(request.Object, () => Task.FromResult(response.Object), CancellationToken.None);

            //Assert
            eventMessageProvider.Verify(s => s.Get(), Times.Never);
        }

        [Test(Description = "Если событий нет, то не должно быть никаких вызовов")]
        public void Handle_EventsIsEmpty_PublishWillBeCalledNever()
        {
            // Arrange
            var eventMessageProvider = new Mock<IEventMessageProvider>();
            var logger = new Mock<ILogger<EventSendingBehavior<IMessage, object>>>();
            var serviceProvider = new Mock<IServiceProvider>();
            var behavior = new EventSendingBehavior<IMessage, object>(eventMessageProvider.Object, serviceProvider.Object, logger.Object);
            var request = new Mock<CommandBase>(Guid.Parse("a183e7e0-6403-476d-a631-19bc8a6739c1"));
            var response = new Mock<object>();

            //Act
            //Assert
            Assert.DoesNotThrowAsync(async () =>
                await behavior.Handle(request.Object, () => Task.FromResult(response.Object), CancellationToken.None));
            eventMessageProvider.Verify(s => s.Get(), Times.Once);
        }

        [Test(Description = "Если есть события, но нет обработчиков, то не должно быть никаких вызовов")]
        public void Handle_HandlersIsEmpty_NoExceptions()
        {
            // Arrange
            var eventMessageProvider = new EventMessageProvider();
            eventMessageProvider.Add(new Mock<IEvent>().Object);
            var logger = new Mock<ILogger<EventSendingBehavior<IMessage, object>>>();
            var serviceProvider = new Mock<IServiceProvider>();
            var behavior = new EventSendingBehavior<IMessage, object>(eventMessageProvider, serviceProvider.Object, logger.Object);
            var request = new Mock<CommandBase>(Guid.Parse("a183e7e0-6403-476d-a631-19bc8a6739c1"));
            var response = new Mock<object>();

            //Act
            //Assert
            Assert.DoesNotThrowAsync(async () =>
                await behavior.Handle(request.Object, () => Task.FromResult(response.Object), CancellationToken.None));
        }

        [Test(Description = "Если есть события и обработчики, то должна быть отправка")]
        public async Task Handle_EventsIsNotEmpty_PublishWillBeCalled()
        {
            // Arrange
            var eventMessageProvider = new EventMessageProvider();
            eventMessageProvider.Add(new TestEvent());
            var logger = new Mock<ILogger<EventSendingBehavior<IMessage, object>>>();
            var serviceProvider = new Mock<IServiceProvider>();
            var handler = new TestEventHandler();
            serviceProvider.Setup(sp => sp.GetService(It.IsAny<Type>()))
                           .Returns(handler);
            var behavior = new EventSendingBehavior<IMessage, object>(eventMessageProvider, serviceProvider.Object, logger.Object);
            var request = new Mock<CommandBase>(Guid.Parse("a183e7e0-6403-476d-a631-19bc8a6739c1"));
            var response = new Mock<object>();

            //Act
            await behavior.Handle(request.Object, () => Task.FromResult(response.Object), CancellationToken.None);

            //Assert
            Assert.That(handler.IsCompleted, Is.True);
        }

        [Test(Description = "Если произошло исключение, то должен быть текст ошибки")]
        public void Handle_PublishThrowException_ReturnReadableException()
        {
            // Arrange
            var eventMessageProvider = new EventMessageProvider();
            eventMessageProvider.Add(new TestEvent());
            var logger = new Mock<ILogger<EventSendingBehavior<IMessage, object>>>();
            var serviceProvider = new Mock<IServiceProvider>();
            var handler = new ExceptionEventHandler();
            serviceProvider.Setup(sp => sp.GetService(It.IsAny<Type>()))
                           .Returns(handler);
            var behavior = new EventSendingBehavior<IMessage, object>(eventMessageProvider, serviceProvider.Object, logger.Object);
            var request = new Mock<CommandBase>(Guid.Parse("a183e7e0-6403-476d-a631-19bc8a6739c1"));
            var response = new Mock<object>();

            //Act
            var exception = Assert.ThrowsAsync<ReadableException>(async () => 
                await behavior.Handle(request.Object, () => Task.FromResult(response.Object), CancellationToken.None));

            //Assert
            Assert.That(exception?.Message, Is.EqualTo("Произошла ошибка при отправке события после выполнения операции."));
        }

    }
}
