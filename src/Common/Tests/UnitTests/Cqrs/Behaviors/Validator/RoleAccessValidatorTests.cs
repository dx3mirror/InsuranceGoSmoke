using InsuranceGoSmoke.Common.Contracts.Abstract;
using InsuranceGoSmoke.Common.Contracts.Contracts.Access.Enums;
using InsuranceGoSmoke.Common.Contracts.Exceptions.Authorization;
using InsuranceGoSmoke.Common.Cqrs.Behaviors.AccessValidator;
using MediatR;
using Moq;

namespace InsuranceGoSmoke.Common.Tests.UnitTests.Cqrs.Behaviors.AccessValidator
{
    internal class RoleAccessValidatorTests
    {
        public class Request : IMessage, IBaseRequest
        {
            public Guid CorrelationId { get; set; }
        }

        public class TestRoleAccessValidator : RoleAccessValidator<Request>
        {
            private readonly IReadOnlyCollection<RoleTypes> userRoles;
            private readonly RoleTypes role;

            public TestRoleAccessValidator(IReadOnlyCollection<RoleTypes> userRoles, RoleTypes role)
            {
                this.userRoles = userRoles;
                this.role = role;
            }

            public override Task ValidateAsync(Request request, CancellationToken cancellationToken)
            {
                ValidateRole(userRoles, "операция", role);
                return Task.CompletedTask;
            }
        }

        [Test(Description = "Если нет ролей, то выбрасывается исключение о нехватке доступа")]
        public void Validate_NoRoles_AccessDeniedException()
        {
            // Arrange
            var request = new Request();
            var handler = new TestRoleAccessValidator([], RoleTypes.Administrator);

            //Act
            var exception = Assert.ThrowsAsync<AccessDeniedException>(async () =>
            {
                await handler.ValidateAsync(request, CancellationToken.None);
            });

            //Assert
            Assert.That(exception, Is.Not.Null);
        }

        [Test(Description = "Если роль клиента, а требовалась роль администратора, то выбрасывается исключение о нехватке доступа")]
        public void Validate_ClientRoleRequiredAdministrator_AccessDeniedException()
        {
            // Arrange
            var request = new Request();
            var handler = new TestRoleAccessValidator([RoleTypes.Client], RoleTypes.Administrator);

            //Act
            var exception = Assert.ThrowsAsync<AccessDeniedException>(async () =>
            {
                await handler.ValidateAsync(request, CancellationToken.None);
            });

            //Assert
            Assert.That(exception, Is.Not.Null);
        }

        [Test(Description = "Если роль администратора и требовалась роль администратора, то ошибок нет")]
        public void Validate_ClientRoleRequiredAdministrator_NoException()
        {
            // Arrange
            var request = new Request();
            var handler = new TestRoleAccessValidator([RoleTypes.Administrator], RoleTypes.Administrator);

            //Act
            //Assert
            Assert.DoesNotThrowAsync(async () =>
            {
                await handler.ValidateAsync(request, CancellationToken.None);
            });
        }
    }
}
