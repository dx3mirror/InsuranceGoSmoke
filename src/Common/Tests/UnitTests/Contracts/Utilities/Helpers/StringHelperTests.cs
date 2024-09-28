using InsuranceGoSmoke.Common.Contracts.Utilities.Helpers;

namespace InsuranceGoSmoke.Common.Tests.UnitTests.Contracts.Utilities.Helpers
{
    internal class StringHelperTests
    {
        [Test(Description = "Если передана пустая строка, то парсится как NULL")]
        public void ToNullBool_EmptyString_Null()
        {
            // Arrange
            var @string = string.Empty;

            // Act
            var result = StringHelper.ToNullBool(@string);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test(Description = "Если передана строка, которая не может распарсится в bool, то парсится как NULL")]
        public void ToNullBool_InvalidString_Null()
        {
            // Arrange
            var @string = "123";

            // Act
            var result = StringHelper.ToNullBool(@string);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test(Description = "Если передан NULL, то парсится как NULL")]
        public void ToNullBool_Null_Null()
        {
            // Arrange
            string? @string = null;

            // Act
            var result = StringHelper.ToNullBool(@string);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test(Description ="Если передана false, то парсится как False")]
        public void ToNullBool_falseString_FalseBoolean()
        {
            // Arrange
            var @string = "false";

            // Act
            var result = StringHelper.ToNullBool(@string);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test(Description = "Если передана False, то парсится как False")]
        public void ToNullBool_FalseString_FalseBoolean()
        {
            // Arrange
            var @string = "False";

            // Act
            var result = StringHelper.ToNullBool(@string);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test(Description = "Если передана true, то парсится как True")]
        public void ToNullBool_trueString_TrueBoolean()
        {
            // Arrange
            var @string = "true";

            // Act
            var result = StringHelper.ToNullBool(@string);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test(Description = "Если передана True, то парсится как True")]
        public void ToNullBool_TrueString_TrueBoolean()
        {
            // Arrange
            var @string = "True";

            // Act
            var result = StringHelper.ToNullBool(@string);

            // Assert
            Assert.That(result, Is.True);
        }
    }
}
