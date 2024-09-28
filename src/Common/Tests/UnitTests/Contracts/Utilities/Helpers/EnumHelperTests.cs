using InsuranceGoSmoke.Common.Contracts.Utilities.Helpers;

namespace InsuranceGoSmoke.Common.Test.UnitTests.Contracts.Utilities.Helpers
{
    /// <summary>
    /// Тесты helper'а перечислений.
    /// </summary>
    internal class EnumHelperTests
    {
        [Test(Description = "Если у типа нет атрибута, то должно вернуться название типа.")]
        public void GetEnumDescription_TypeWithoutAttribute_ReturnNameType()
        {
            // Arrange
            var type = Type.Undefined;

            // Act
            var result = EnumHelper.GetEnumDescription(type);

            // Assert
            Assert.That(result, Is.EqualTo("Undefined"));
        }

        [Test(Description = "Если у типа есть атрибут, то должно вернуться значение этого атрибута.")]
        public void GetEnumDescription_TypeWithAttribute_ReturnDescriptionValue()
        {
            // Arrange
            var type = Type.Described;

            // Act
            var result = EnumHelper.GetEnumDescription(type);

            // Assert
            Assert.That(result, Is.EqualTo("Описание"));
        }

        [Test(Description = "Если у типа нету атрибута, то должно вернуться неопределенное значение.")]
        public void GetEnumDescription_TypeWithoutAttribute_ReturnNullType()
        {
            // Arrange
            var type = Type.Undefined;

            // Act
            var result = EnumHelper.GetEnumAttribuite<Type, System.ComponentModel.DescriptionAttribute>(type);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test(Description = "Если у типа есть атрибут, то должен вернуться атрибут.")]
        public void GetEnumAttribuite_TypeWithAttribute_ReturnDescriptionAtributeValue()
        {
            // Arrange
            var type = Type.Described;

            // Act
            var result = EnumHelper.GetEnumAttribuite<Type, System.ComponentModel.DescriptionAttribute>(type);

            // Assert
            Assert.That(result, Is.Not.Null);
        }
    }

    internal enum Type
    {
        Undefined,

        [System.ComponentModel.Description("Описание")]
        Described
    }
}
