using InsuranceGoSmoke.Common.Contracts.Abstract;
using Moq;

namespace InsuranceGoSmoke.Common.Tests.UnitTests.Contracts.Abstract
{
    /// <summary>
    /// Тесты на команду.
    /// </summary>
    internal class CommandTests
    {
        internal class CommandTest : Command
        {
        }

        internal class CommandTest<T> : Command<T>
        {
        }

        [Test(Description = "При создании команды и заполнении идентификатора, идентификатор должен заполняться")]
        public void CommandBase_CorrelationGuid_CorrelationGuidIsNotEmpty()
        {
            // Arrange
            var guid = new Guid("0dd794cd-0103-470c-858f-0dcd6a9bc261");

            // Act
            var command = new Mock<CommandBase>(guid) { CallBase = true };

            // Assert
            Assert.That(command.Object.CorrelationId, Is.EqualTo(guid));
        }

        [Test(Description = "При создании команды не должно быть исключений")]
        public void CommandBase_Constructor_NoException()
        {
            // Arrange

            // Act
            // Assert
            Assert.DoesNotThrow(() =>
                new Mock<CommandBase>() { CallBase = true });
        }

        [Test(Description = "При создании команды и заполнении идентификатора, идентификатор должен заполняться")]
        public void Command_CorrelationGuid_CorrelationGuidIsNotEmpty()
        {
            // Arrange
            var guid = new Guid("0dd794cd-0103-470c-858f-0dcd6a9bc261");

            // Act
            var command = new Mock<Command>(guid) { CallBase = true };

            // Assert
            Assert.That(command.Object.CorrelationId, Is.EqualTo(guid));
        }

        [Test(Description = "При создании команды не должно быть исключений")]
        public void Command_Constructor_NoException()
        {
            // Arrange

            // Act
            // Assert
            Assert.DoesNotThrow(() => new CommandTest());
        }

        [Test(Description = "При создании команды и заполнении идентификатора, идентификатор должен заполняться")]
        public void CommandGeneric_CorrelationGuid_CorrelationGuidIsNotEmpty()
        {
            // Arrange
            var guid = new Guid("0dd794cd-0103-470c-858f-0dcd6a9bc261");

            // Act
            var command = new Mock<Command<object>>(guid) { CallBase = true };

            // Assert
            Assert.That(command.Object.CorrelationId, Is.EqualTo(guid));
        }

        [Test(Description = "При создании команды не должно быть исключений")]
        public void CommandGeneric_Constructor_NoException()
        {
            // Arrange

            // Act
            // Assert
            Assert.DoesNotThrow(() => new CommandTest<object>());
        }

    }
}
