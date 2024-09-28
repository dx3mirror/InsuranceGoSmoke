using InsuranceGoSmoke.Common.Contracts.Exceptions.Common;

namespace InsuranceGoSmoke.Common.Test.UnitTests.Contracts.Exceptions
{
    /// <summary>
    /// Тесты на исключение, выбрасываемое когда не найдена сущность.
    /// </summary>
    internal class NotFoundExceptionTests
    {
        [Test(Description = "Переданный текст в исключение, должен сохраняться.")]
        public void NotFoundException_Message_MessageEquals()
        {
            // Act
            var exception = new NotFoundException("Сообщение");

            // Assert
            Assert.That(exception.Message, Is.EqualTo("Сообщение"));
        }
    }
}
