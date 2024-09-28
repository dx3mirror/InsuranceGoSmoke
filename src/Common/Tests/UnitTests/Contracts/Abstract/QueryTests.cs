using InsuranceGoSmoke.Common.Contracts.Abstract;
using Moq;

namespace InsuranceGoSmoke.Common.Tests.UnitTests.Contracts.Abstract
{
    internal class QueryTests
    {
        internal class QueryTest<T> : Query<T>
        {
        }

        [Test(Description = "При создании команды и заполнении идентификатора, идентификатор должен заполняться")]
        public void Query_CorrelationGuid_CorrelationGuidIsNotEmpty()
        {
            // Arrange
            var guid = new Guid("0dd794cd-0103-470c-858f-0dcd6a9bc261");

            // Act
            var command = new Mock<Query<object>>(guid) { CallBase = true };

            // Assert
            Assert.That(command.Object.CorrelationId, Is.EqualTo(guid));
        }

        [Test(Description = "При создании команды не должно быть исключений")]
        public void Query_Constructor_NoException()
        {
            // Arrange

            // Act
            // Assert
            Assert.DoesNotThrow(() => new QueryTest<object>());
        }
    }
}
