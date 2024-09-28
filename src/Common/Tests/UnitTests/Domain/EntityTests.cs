using InsuranceGoSmoke.Common.Domain;

namespace InsuranceGoSmoke.Common.Tests.UnitTests.Domain
{
    /// <summary>
    /// Тесты на доменные сущности.
    /// </summary>
    internal class EntityTests
    {
        internal class TestEntity : Entity<int>
        {
        }

        internal class TestEntityRowVersion : EntityRowVersion<int>
        {
        }

        [Test(Description = "При создании доменной сущности идентификатор должен быть равен 0")]
        public void Entity_SetId_IdIsNotDefault()
        {
            // Arrange
            // Act
            var entity = new TestEntity();

            // Assert
            Assert.That(entity.Id, Is.Zero);
        }

        [Test(Description = "При создании доменной сущности версия должна быть пустая")]
        public void Entity_Create_RowVersionIsEmpty()
        {
            // Arrange
            // Act
            var entity = new TestEntityRowVersion();

            // Assert
            Assert.That(entity.RowVersion, Is.Empty);
        }
    }
}
