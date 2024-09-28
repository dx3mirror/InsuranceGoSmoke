using InsuranceGoSmoke.Common.Contracts.Abstract;
using InsuranceGoSmoke.Common.Cqrs.Behaviors;
using InsuranceGoSmoke.Common.Cqrs.Behaviors.Transaction;
using InsuranceGoSmoke.Common.Infrastructures.DataAccess.Session;
using Moq;

namespace InsuranceGoSmoke.Common.Test.UnitTests.Cqrs.Behaviors
{
    /// <summary>
    /// Тесты поведения транзакций.
    /// </summary>
    internal class TransactionalBehaviorTests
    {
        [Test(Description = "Если запрос не является командой, то не должно быть никаких транзакций")]
        public async Task Handle_RequestIsNotCommand_NoTransactionbehavior()
        {
            // Arrange
            var dataSessionFactory = new Mock<IDataSessionFactory>();
            var session = new Mock<IDataSession>();
            session.Setup(s => s.HasActiveTransaction()).Returns(false);
            dataSessionFactory.Setup(f => f.Create()).Returns(session.Object);
            var behavior = new TransactionalBehavior<IMessage, object>(dataSessionFactory.Object);
            var request = new Mock<IMessage>();
            var response = new Mock<object>();

            //Act
            await behavior.Handle(request.Object, () => Task.FromResult(response.Object), CancellationToken.None);

            //Assert
            session.Verify(s => s.BeginTransactionAsync(CancellationToken.None), Times.Never);
            session.Verify(s => s.CommitTransaction(), Times.Never);
            session.Verify(s => s.RollbackTransaction(), Times.Never);
        }

        [Test(Description = "Если нет активной транзакции, то транзакция создается и только один раз")]
        public async Task Handle_HasNotActiveTransaction_BeginTransactionCalledOnce()
        {
            // Arrange
            var dataSessionFactory = new Mock<IDataSessionFactory>();
            var session = new Mock<IDataSession>();
            session.Setup(s => s.HasActiveTransaction()).Returns(false);
            dataSessionFactory.Setup(f => f.Create()).Returns(session.Object);
            var behavior = new TransactionalBehavior<IMessage, object>(dataSessionFactory.Object);
            var request = new Mock<CommandBase>(Guid.Parse("a183e7e0-6403-476d-a631-19bc8a6739c1"));
            var response = new Mock<object>();

            //Act
            await behavior.Handle(request.Object, () => Task.FromResult(response.Object), CancellationToken.None);

            //Assert
            session.Verify(s => s.BeginTransactionAsync(CancellationToken.None), Times.Once);
        }

        [Test(Description = "Если нет активной транзакции, то транзакция коммитится и только один раз")]
        public async Task Handle_HasNotActiveTransaction_CommitTransactionCalledOnce()
        {
            // Arrange
            var dataSessionFactory = new Mock<IDataSessionFactory>();
            var session = new Mock<IDataSession>();
            session.Setup(s => s.HasActiveTransaction()).Returns(false);
            dataSessionFactory.Setup(f => f.Create()).Returns(session.Object);
            var behavior = new TransactionalBehavior<IMessage, object>(dataSessionFactory.Object);
            var request = new Mock<CommandBase>(Guid.Parse("a183e7e0-6403-476d-a631-19bc8a6739c1"));
            var response = new Mock<object>();

            //Act
            await behavior.Handle(request.Object, () => Task.FromResult(response.Object), CancellationToken.None);

            //Assert
            session.Verify(s => s.CommitTransaction(), Times.Once);
        }

        [Test(Description = "Если нет активной транзакции и произошла ошибка при выполнении транзакции, то транзакция откатывается")]
        public void Handle_HasNotActiveTransactionAndCommitTransactionException_RollbackTransactionCalledOnce()
        {
            // Arrange
            var dataSessionFactory = new Mock<IDataSessionFactory>();
            var session = new Mock<IDataSession>();
            session.Setup(s => s.HasActiveTransaction()).Returns(false);
            session.Setup(s => s.CommitTransaction()).Throws(new InvalidOperationException());
            dataSessionFactory.Setup(f => f.Create()).Returns(session.Object);
            var behavior = new TransactionalBehavior<IMessage, object>(dataSessionFactory.Object);
            var request = new Mock<CommandBase>(Guid.Parse("a183e7e0-6403-476d-a631-19bc8a6739c1"));
            var response = new Mock<object>();

            //Act
            Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                await behavior.Handle(request.Object, () => Task.FromResult(response.Object), CancellationToken.None);
            });

            //Assert
            session.Verify(s => s.RollbackTransaction(), Times.Once);
        }

        [Test(Description = "Если нет активной транзакции и произошла ошибка при выполнении транзакции, то выбрасывается исключение")]
        public void Handle_HasNotActiveTransactionAndCommitTransactionException_ExceptionIsNotNull()
        {
            // Arrange
            var dataSessionFactory = new Mock<IDataSessionFactory>();
            var session = new Mock<IDataSession>();
            session.Setup(s => s.HasActiveTransaction()).Returns(false);
            session.Setup(s => s.CommitTransaction()).Throws(new InvalidOperationException());
            dataSessionFactory.Setup(f => f.Create()).Returns(session.Object);
            var behavior = new TransactionalBehavior<IMessage, object>(dataSessionFactory.Object);
            var request = new Mock<CommandBase>(Guid.Parse("a183e7e0-6403-476d-a631-19bc8a6739c1"));
            var response = new Mock<object>();

            //Act
            var exception = Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                await behavior.Handle(request.Object, () => Task.FromResult(response.Object), CancellationToken.None);
            });

            //Assert
            Assert.That(exception, Is.Not.Null);
        }

        [Test(Description = "Если нет активной транзакции и произошла ошибка при выполнении транзакции, то выбрасывается исключение")]
        public void Handle_RollbackTransactionException_ExceptionIsNotNull()
        {
            // Arrange
            var dataSessionFactory = new Mock<IDataSessionFactory>();
            var session = new Mock<IDataSession>();
            session.Setup(s => s.HasActiveTransaction()).Returns(false);
            session.Setup(s => s.CommitTransaction()).Throws(new InvalidOperationException());
            session.Setup(s => s.RollbackTransaction()).Throws(new InvalidOperationException());
            dataSessionFactory.Setup(f => f.Create()).Returns(session.Object);
            var behavior = new TransactionalBehavior<IMessage, object>(dataSessionFactory.Object);
            var request = new Mock<CommandBase>(Guid.Parse("a183e7e0-6403-476d-a631-19bc8a6739c1"));
            var response = new Mock<object>();

            //Act
            var exception = Assert.ThrowsAsync<AggregateException>(async () =>
            {
                await behavior.Handle(request.Object, () => Task.FromResult(response.Object), CancellationToken.None);
            });

            //Assert
            Assert.That(exception, Is.Not.Null);
        }

        [Test(Description = "Если транзакция уже активна, то транзакция не создается")]
        public async Task Handle_HasActiveTransaction__BeginTransactionCalledNever()
        {
            // Arrange
            var dataSessionFactory = new Mock<IDataSessionFactory>();
            var session = new Mock<IDataSession>();
            session.Setup(s => s.HasActiveTransaction()).Returns(true);
            dataSessionFactory.Setup(f => f.Create()).Returns(session.Object);
            var behavior = new TransactionalBehavior<IMessage, object>(dataSessionFactory.Object);
            var request = new Mock<CommandBase>(Guid.Parse("a183e7e0-6403-476d-a631-19bc8a6739c1"));
            var response = new Mock<object>();

            //Act
            await behavior.Handle(request.Object, () => Task.FromResult(response.Object), CancellationToken.None);

            //Assert
            session.Verify(s => s.BeginTransactionAsync(CancellationToken.None), Times.Never);
        }

        [Test(Description = "Если транзакция уже активна, то транзакция коммитится и только один раз")]
        public async Task Handle_HasActiveTransaction_CommitTransactionCalledOnce()
        {
            // Arrange
            var dataSessionFactory = new Mock<IDataSessionFactory>();
            var session = new Mock<IDataSession>();
            session.Setup(s => s.HasActiveTransaction()).Returns(true);
            dataSessionFactory.Setup(f => f.Create()).Returns(session.Object);
            var behavior = new TransactionalBehavior<IMessage, object>(dataSessionFactory.Object);
            var request = new Mock<CommandBase>(Guid.Parse("a183e7e0-6403-476d-a631-19bc8a6739c1"));
            var response = new Mock<object>();

            //Act
            await behavior.Handle(request.Object, () => Task.FromResult(response.Object), CancellationToken.None);

            //Assert
            session.Verify(s => s.CommitTransaction(), Times.Once);
        }

        [Test(Description = "Если транзакция уже активна и произошла ошибка при выполнении транзакции, то транзакция не откатывается")]
        public void Handle_HasActiveTransactionAndCommitTransactionException_RollbackTransactionCalledNever()
        {
            // Arrange
            var dataSessionFactory = new Mock<IDataSessionFactory>();
            var session = new Mock<IDataSession>();
            session.Setup(s => s.HasActiveTransaction()).Returns(true);
            session.Setup(s => s.CommitTransaction()).Throws(new InvalidOperationException());
            dataSessionFactory.Setup(f => f.Create()).Returns(session.Object);
            var behavior = new TransactionalBehavior<IMessage, object>(dataSessionFactory.Object);
            var request = new Mock<CommandBase>(Guid.Parse("a183e7e0-6403-476d-a631-19bc8a6739c1"));
            var response = new Mock<object>();

            //Act
            Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                await behavior.Handle(request.Object, () => Task.FromResult(response.Object), CancellationToken.None);
            });

            //Assert
            session.Verify(s => s.RollbackTransaction(), Times.Never);
        }

        [Test(Description = "Если транзакция уже активна и произошла ошибка при выполнении транзакции, то выбрасывается исключение")]
        public void Handle_HasActiveTransactionAndCommitTransactionException_ExceptionIsNotNull()
        {
            // Arrange
            var dataSessionFactory = new Mock<IDataSessionFactory>();
            var session = new Mock<IDataSession>();
            session.Setup(s => s.HasActiveTransaction()).Returns(true);
            session.Setup(s => s.CommitTransaction()).Throws(new InvalidOperationException());
            dataSessionFactory.Setup(f => f.Create()).Returns(session.Object);
            var behavior = new TransactionalBehavior<IMessage, object>(dataSessionFactory.Object);
            var request = new Mock<CommandBase>(Guid.Parse("a183e7e0-6403-476d-a631-19bc8a6739c1"));
            var response = new Mock<object>();

            //Act
            var exception = Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                await behavior.Handle(request.Object, () => Task.FromResult(response.Object), CancellationToken.None);
            });

            //Assert
            Assert.That(exception, Is.Not.Null);
        }
    }
}
