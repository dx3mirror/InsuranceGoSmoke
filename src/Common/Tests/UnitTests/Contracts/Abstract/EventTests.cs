using InsuranceGoSmoke.Common.Contracts.Abstract;
using Moq;

namespace InsuranceGoSmoke.Common.Tests.UnitTests.Contracts.Abstract
{
    /// <summary>
    /// Тесты событий.
    /// </summary>
    internal class EventTests
    {
        internal class EventTest : Event
        {
        }

        [Test(Description = "При создании команды и заполнении идентификатора, идентификатор должен заполняться")]
        public void Event_CorrelationGuid_CorrelationGuidIsNotEmpty()
        {
            // Arrange
            var guid = new Guid("0dd794cd-0103-470c-858f-0dcd6a9bc261");

            // Act
            var command = new Mock<Event>(guid) { CallBase = true };

            // Assert
            Assert.That(command.Object.CorrelationId, Is.EqualTo(guid));
        }

        [Test(Description = "При создании команды не должно быть исключений")]
        public void Event_Constructor_NoException()
        {
            // Arrange

            // Act
            // Assert
            Assert.DoesNotThrow(() => new EventTest());
        }
    }
}
