using InsuranceGoSmoke.Common.Contracts.Utilities.Helpers;

namespace InsuranceGoSmoke.Common.Test.UnitTests.Contracts.Utilities.Helpers
{
    /// <summary>
    /// Тесты helper'а работы с датами и временем.
    /// </summary>
    internal class DateTimeHelperTests
    {
        [Test(Description = "Если дата валидная, то результат должен быть определенным.")]
        public void ToDateTime_ValidDate_DateTimeIsNotNull()
        {
            // Arrange
            string date = "02-01-2000";

            // Act
            var result = DateTimeHelper.ToDateTime(date);

            // Assert
            var emptyDate = new DateTime(0, DateTimeKind.Unspecified);
            Assert.That(result, Is.Not.EqualTo(emptyDate));
        }

        [Test(Description = "Если дата валидная, то результат должен быть равен дате.")]
        public void ToDateTime_ValidDate_ResultEqualsOriginData()
        {
            // Arrange
            string date = "02-01-2000";

            // Act
            var result = DateTimeHelper.ToDateTime(date);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.Value.Year, Is.EqualTo(2000));
                Assert.That(result.Value.Month, Is.EqualTo(1));
                Assert.That(result.Value.Day, Is.EqualTo(2));
            });
        }

        [Test(Description = "Если дата валидная, то время у результата должно быть равно полночи.")]
        public void ToDateTime_ValidDate_DateTimeInMidnight()
        {
            // Arrange
            string date = "02-01-2000";

            // Act
            var result = DateTimeHelper.ToDateTime(date);
                
            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.Value.Hour, Is.EqualTo(0));
                Assert.That(result.Value.Minute, Is.EqualTo(0));
                Assert.That(result.Value.Second, Is.EqualTo(0));
            });
        }

        [Test(Description = "Если дата валидная, то время у результата должно быть в UTC.")]
        public void ToDateTime_ValidDate_DateTimeHasUtcKind()
        {
            // Arrange
            string date = "02-01-2000";

            // Act
            var result = DateTimeHelper.ToDateTime(date);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value.Kind, Is.EqualTo(DateTimeKind.Utc));
        }
    }
}
