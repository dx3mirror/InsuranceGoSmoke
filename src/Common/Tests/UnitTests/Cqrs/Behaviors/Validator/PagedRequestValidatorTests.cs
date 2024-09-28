using InsuranceGoSmoke.Common.Cqrs.Behaviors.DataValidator;
using InsuranceGoSmoke.Common.Cqrs.Behaviors.Query;

namespace InsuranceGoSmoke.Common.Tests.UnitTests.Cqrs.Behaviors.Validator
{
    internal class PagedRequestValidatorTests
    {
        public class Request : IPagedQuery
        {
            public Request(int take, int? skip)
            {
                Take = take;
                Skip = skip;
            }

            public int Take { get; set; }

            public int? Skip { get; set; }
        }

        public class TestPagedRequestValidator : PagedRequestValidator<Request>
        {
        }

        [Test(Description = "При создании валидатора нет ошибок.")]
        public void TestPagedRequestValidator_ValidData_NoException()
        {
            //Act
            //Assert
            Assert.DoesNotThrow(() =>
            {
                new TestPagedRequestValidator();
            });
        }

        [Test(Description = "Если take = 0, то валидация не проходит.")]
        public void Validate_TakeEqualsZero_False()
        {
            // Arrange
            var request = new Request(0, 1);
            var handler = new TestPagedRequestValidator();

            //Act
            var result = handler.Validate(request);

            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.False);
        }

        [Test(Description = "Если take отрицательный, то валидация не проходит.")]
        public void Validate_TakeIsNegative_False()
        {
            // Arrange
            var request = new Request(-1, 1);
            var handler = new TestPagedRequestValidator();

            //Act
            var result = handler.Validate(request);

            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.False);
        }

        [Test(Description = "Если take слишком большой, то валидация не проходит.")]
        public void Validate_TakeIsVeryBig_False()
        {
            // Arrange
            var request = new Request(int.MaxValue, 1);
            var handler = new TestPagedRequestValidator();

            //Act
            var result = handler.Validate(request);

            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.False);
        }

        [Test(Description = "Если skip отрицательный, то валидация не проходит.")]
        public void Validate_SkipIsNegative_False()
        {
            // Arrange
            var request = new Request(1, -1);
            var handler = new TestPagedRequestValidator();

            //Act
            var result = handler.Validate(request);

            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.False);
        }
        [Test(Description = "Если валидные данные, то валидация проходит.")]
        public void Validate_ValidData_True()
        {
            // Arrange
            var request = new Request(10, 0);
            var handler = new TestPagedRequestValidator();

            //Act
            var result = handler.Validate(request);

            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
        }
    }
}
