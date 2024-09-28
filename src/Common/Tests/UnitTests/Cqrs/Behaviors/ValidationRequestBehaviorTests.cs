using FluentValidation;
using InsuranceGoSmoke.Common.Contracts.Abstract;
using InsuranceGoSmoke.Common.Cqrs.Behaviors;
using InsuranceGoSmoke.Common.Cqrs.Behaviors.DataValidator;
using Moq;

namespace InsuranceGoSmoke.Common.Test.UnitTests.Cqrs.Behaviors
{
    /// <summary>
    /// Тесты поведения валидации запросов.
    /// </summary>
    internal class ValidationRequestBehaviorTests
    {
        [Test(Description = "Если список валидаторов пустой, то не должно быть никаких исключений")]
        public void Handle_ValidatorsIsEmpty_NoExceptions()
        {
            // Arrange
            IEnumerable<IValidator<object>> validators = [];
            var behavior = new ValidationRequestBehavior<IMessage, object>(validators);
            var request = new Mock<IMessage>();
            var response = new Mock<object>();

            //Act
            //Assert
            Assert.DoesNotThrowAsync(async () =>
            {
                await behavior.Handle(request.Object, () => Task.FromResult(response.Object), CancellationToken.None);
            });
        }

        [Test(Description = "Если список валидаторов не пустой, то валидация должна вызваться один раз")]
        public async Task Handle_ValidatorsIsNotEmpty_ValidateCalledOnce()
        {
            // Arrange
            var validator = new Mock<IValidator<CommandBase>>();
            validator
                .Setup(v => v.ValidateAsync(It.IsAny<IValidationContext>(), CancellationToken.None))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult());
            IEnumerable<IValidator<CommandBase>> validators = [validator.Object];
            var behavior = new ValidationRequestBehavior<CommandBase, object>(validators);
            var request = new Mock<CommandBase>(Guid.Parse("a183e7e0-6403-476d-a631-19bc8a6739c1"));
            var response = new Mock<object>();

            //Act
            await behavior.Handle(request.Object, () => Task.FromResult(response.Object), CancellationToken.None);

            //Assert
            validator.Verify(v => v.ValidateAsync(It.IsAny<IValidationContext>(), CancellationToken.None), Times.Once);
        }


        [Test(Description = "Если значение невалидное, то должно быть исключение")]
        public void Handle_ValueNotValid_ValidationExceptionThrowed()
        {
            // Arrange
            var error = new FluentValidation.Results.ValidationFailure() {  PropertyName = "name", ErrorMessage = "empty" };
            var validator = new Mock<IValidator<CommandBase>>();
            validator
                .Setup(v => v.ValidateAsync(It.IsAny<IValidationContext>(), CancellationToken.None))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult() {  Errors = new List<FluentValidation.Results.ValidationFailure> { error } });
            IEnumerable<IValidator<CommandBase>> validators = [validator.Object];
            var behavior = new ValidationRequestBehavior<CommandBase, object>(validators);
            var request = new Mock<CommandBase>(Guid.Parse("a183e7e0-6403-476d-a631-19bc8a6739c1"));
            var response = new Mock<object>();

            //Act
            var exception = Assert.ThrowsAsync<FluentValidation.ValidationException>(async () =>
            {
                await behavior.Handle(request.Object, () => Task.FromResult(response.Object), CancellationToken.None);
            });

            //Assert
            Assert.That(exception.Message, Is.EqualTo("Поле 'name': empty"));
        }
    }
}
