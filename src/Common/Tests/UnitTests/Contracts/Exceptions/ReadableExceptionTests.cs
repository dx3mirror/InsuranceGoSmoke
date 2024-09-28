using InsuranceGoSmoke.Common.Contracts.Exceptions.Common;

namespace InsuranceGoSmoke.Common.Test.UnitTests.Contracts.Exceptions
{
    /// <summary>
    /// Тесты на читабельное исключение.
    /// </summary>
    internal class ReadableExceptionTests
    {
        [Test(Description = "Переданный текст в исключение должен сохраняться.")]
        public void ReadableException_Message_MessageEquals()
        {
            // Act
            var exception = new ReadableException("Сообщение");

            // Assert
            Assert.That(exception.Message, Is.EqualTo("Сообщение"));
        }

        [Test(Description = "Переданное описание в исключение должно сохраняться.")]
        public void ReadableException_Description_DescriptionEquals()
        {
            // Act
            var exception = new ReadableException("Сообщение", "Описание");

            // Assert
            Assert.That(exception.Description, Is.EqualTo("Описание"));
        }

        [Test(Description = "Переданное внутренее исключение должно сохраняться.")]
        public void ReadableException_InnerException_InnerExceptionTypeEquals()
        {
            // Act
            var innerException = new NotImplementedException();
            var exception = new ReadableException("Сообщение", innerException);

            // Assert
            Assert.That(exception.InnerException?.GetType(), Is.EqualTo(typeof(NotImplementedException)));
        }
    }
}
