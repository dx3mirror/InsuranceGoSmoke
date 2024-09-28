namespace InsuranceGoSmoke.Common.Tests.UnitTests.AppServices.Services.DateTimeProvider
{
    /// <summary>
    /// Тесты провайдера даты и времени.
    /// </summary>
    internal class DateTimeProviderTests
    {
        [Test]
        public void UtcNow_Get_DateTimeIsNotMinValue()
        {
            // Arrange
            var provider = new Applications.AppServices.Contexts.Common.Services.DateTimeProvider.DateTimeProvider();

            // Act
            var dateTime = provider.UtcNow;

            // Assert
            Assert.That(dateTime, Is.Not.EqualTo(DateTime.MinValue));
        }
    }
}
