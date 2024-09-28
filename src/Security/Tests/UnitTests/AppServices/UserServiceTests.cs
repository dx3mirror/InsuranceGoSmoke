using Kortros.Security.ExternalClients.Keycloak;
using Microsoft.Extensions.Logging;
using Moq;
using Microsoft.Extensions.Caching.Distributed;
using Kortros.Common.Contracts.Exceptions.Common;
using Kortros.Common.Clients.Keycloak.Models;
using Kortros.Security.Applications.AppServices.Contexts.Users.Services;
using Kortros.Security.Applications.AppServices.Contexts.Users.Models;
using Kortros.Common.Clients.Keycloak.Models.Responses;
using Kortros.Common.Contracts.Contracts.Access.Enums;
using Kortros.Security.Contracts.Users.Enums;
using Kortros.Common.Contracts.Contracts.Authorization;

namespace Kortros.Security.Tests.UnitTests.AppServices
{
    internal class UserServiceTests
    {
        [Test(Description = "Если при вызове метода отправки выбрасилось исключение, оно должно прокидываться.")]
        public void SendVerificationCodeAsync_SendVerificationCodeThrowException_KeycloakApiException()
        {
            // Arrange
            var distributedCache = new Mock<IDistributedCache>();
            var keycloakApiClient = new Mock<IKeycloakUserApiClient>();
            var logger = new Mock<ILogger<UserService>>();
            var authorizationData = new Mock<IAuthorizationData>();
            var service = new UserService(distributedCache.Object, keycloakApiClient.Object,
                logger.Object, authorizationData.Object);

            var phoneNumber = "123";
            keycloakApiClient.Setup(c => c.SendVerificationCodeAsync(phoneNumber, CancellationToken.None))
                             .ThrowsAsync(new KeycloakApiException("Ошибка"));
            // Act
            var exception = Assert.ThrowsAsync<KeycloakApiException>(async () =>
                                await service.SendVerificationCodeAsync(phoneNumber, CancellationToken.None));

            // Assert
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.EqualTo("Ошибка"));
        }

        [Test(Description = "Если при отправке возвращается ошибка, то выбрасывается исключение.")]
        public void SendVerificationCodeAsync_ReturnError_ReadableException()
        {
            // Arrange
            var distributedCache = new Mock<IDistributedCache>();
            var keycloakApiClient = new Mock<IKeycloakUserApiClient>();
            var logger = new Mock<ILogger<UserService>>();
            var authorizationData = new Mock<IAuthorizationData>();
            var service = new UserService(distributedCache.Object, keycloakApiClient.Object,
                logger.Object, authorizationData.Object);

            var phoneNumber = "123";
            keycloakApiClient.Setup(c => c.SendVerificationCodeAsync(phoneNumber, CancellationToken.None))
                             .ReturnsAsync(Common.Clients.Keycloak.Models.Responses.KeycloakResponse.Fail("Ошибка", "Описание"));
            // Act
            var exception = Assert.ThrowsAsync<ReadableException>(async () =>
                                await service.SendVerificationCodeAsync(phoneNumber, CancellationToken.None));

            // Assert
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.EqualTo("Произошла ошибка при отправке кода верификации."));
        }

        [Test(Description = "Если при отправке нет ошибок, то нет и исключений.")]
        public void SendVerificationCodeAsync_ValidData_NoException()
        {
            // Arrange
            var distributedCache = new Mock<IDistributedCache>();
            var keycloakApiClient = new Mock<IKeycloakUserApiClient>();
            var logger = new Mock<ILogger<UserService>>();
            var authorizationData = new Mock<IAuthorizationData>();
            var service = new UserService(distributedCache.Object, keycloakApiClient.Object,
                logger.Object, authorizationData.Object);

            var phoneNumber = "123";
            keycloakApiClient.Setup(c => c.SendVerificationCodeAsync(phoneNumber, CancellationToken.None))
                             .ReturnsAsync(Common.Clients.Keycloak.Models.Responses.KeycloakResponse.Success());
            // Act
            // Assert
            Assert.DoesNotThrowAsync(async () =>
                await service.SendVerificationCodeAsync(phoneNumber, CancellationToken.None));

            keycloakApiClient.Verify(c => c.SendVerificationCodeAsync(phoneNumber, CancellationToken.None), Times.Once);
        }

        [Test(Description = "Если при проверке кода выбрасилось исключение, оно должно прокидываться.")]
        public void ChangePhoneNumberAsync_VerifyCodeThrowException_KeycloakApiException()
        {
            // Arrange
            var distributedCache = new Mock<IDistributedCache>();
            var keycloakApiClient = new Mock<IKeycloakUserApiClient>();
            var logger = new Mock<ILogger<UserService>>();
            var authorizationData = new Mock<IAuthorizationData>();
            var service = new UserService(distributedCache.Object, keycloakApiClient.Object,
                logger.Object, authorizationData.Object);

            var phoneNumber = "123";
            var code = "123";
            var userId = Guid.Parse("051c1298-b93d-4ebb-a764-6df765188b0e");
            keycloakApiClient.Setup(c => c.VerifyCodeAsync(phoneNumber, code, CancellationToken.None))
                             .ThrowsAsync(new KeycloakApiException("Ошибка"));
            // Act
            var exception = Assert.ThrowsAsync<KeycloakApiException>(async () =>
                                await service.UpdatePhoneNumberAsync(userId, phoneNumber, CancellationToken.None));

            // Assert
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.EqualTo("Ошибка"));
        }

        [Test(Description = "Если при проверке возвращается ошибка, то выбрасывается исключение.")]
        public void ChangePhoneNumberAsync_ReturnError_ReadableException()
        {
            // Arrange
            var distributedCache = new Mock<IDistributedCache>();
            var keycloakApiClient = new Mock<IKeycloakUserApiClient>();
            var logger = new Mock<ILogger<UserService>>();
            var authorizationData = new Mock<IAuthorizationData>();
            var service = new UserService(distributedCache.Object, keycloakApiClient.Object,
                logger.Object, authorizationData.Object);

            var phoneNumber = "123";
            var code = "123";
            var userId = Guid.Parse("051c1298-b93d-4ebb-a764-6df765188b0e");
            keycloakApiClient.Setup(c => c.VerifyCodeAsync(phoneNumber, code, CancellationToken.None))
                             .ReturnsAsync(Common.Clients.Keycloak.Models.Responses.KeycloakResponse.Fail("Ошибка", "Описание"));
            // Act
            var exception = Assert.ThrowsAsync<ReadableException>(async () =>
                                await service.UpdatePhoneNumberAsync(userId, phoneNumber, CancellationToken.None));

            // Assert
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.EqualTo("Номер телефона не прошел верификацию."));
        }

        [Test(Description = "Если при обновлении номера выбрасилось исключение, оно должно прокидываться.")]
        public void ChangePhoneNumberAsync_UpdatePhoneNumberThrowException_KeycloakApiException()
        {
            // Arrange
            var distributedCache = new Mock<IDistributedCache>();
            var keycloakApiClient = new Mock<IKeycloakUserApiClient>();
            var logger = new Mock<ILogger<UserService>>();
            var authorizationData = new Mock<IAuthorizationData>();
            var service = new UserService(distributedCache.Object, keycloakApiClient.Object,
                logger.Object, authorizationData.Object);

            var userId = Guid.Parse("cf80f652-0d56-4d7c-b315-38eab5c3b0bc");
            var phoneNumber = "123";
            var code = "123";
            keycloakApiClient.Setup(c => c.VerifyCodeAsync(phoneNumber, code, CancellationToken.None))
                             .ReturnsAsync(KeycloakResponse.Success());
            keycloakApiClient.Setup(c => c.UpdatePhoneNumberAsync(userId, phoneNumber, CancellationToken.None))
                             .ThrowsAsync(new KeycloakApiException("Ошибка"));
            // Act
            var exception = Assert.ThrowsAsync<KeycloakApiException>(async () =>
                                await service.UpdatePhoneNumberAsync(userId, phoneNumber, CancellationToken.None));

            // Assert
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.EqualTo("Ошибка"));
        }

        [Test(Description = "Если при отправке нет ошибок, то нет и исключений.")]
        public void ChangePhoneNumberAsync_ValidData_NoException()
        {
            // Arrange
            var distributedCache = new Mock<IDistributedCache>();
            var keycloakApiClient = new Mock<IKeycloakUserApiClient>();
            var logger = new Mock<ILogger<UserService>>();
            var authorizationData = new Mock<IAuthorizationData>();
            var service = new UserService(distributedCache.Object, keycloakApiClient.Object,
                logger.Object, authorizationData.Object);

            var userId = Guid.Parse("cf80f652-0d56-4d7c-b315-38eab5c3b0bc");
            var phoneNumber = "123";
            var code = "123";
            keycloakApiClient.Setup(c => c.VerifyCodeAsync(phoneNumber, code, CancellationToken.None))
                             .ReturnsAsync(KeycloakResponse.Success());
            // Act
            // Assert
            Assert.DoesNotThrowAsync(async () =>
                await service.UpdatePhoneNumberAsync(userId, phoneNumber, CancellationToken.None));

            keycloakApiClient.Verify(c => c.VerifyCodeAsync(phoneNumber, code, CancellationToken.None), Times.Once);
            keycloakApiClient.Verify(c => c.UpdatePhoneNumberAsync(userId, phoneNumber, CancellationToken.None), Times.Once);
        }

        [Test(Description = "Если пользователь не найден, то выбрасывается исключение")]
        public void GetUserAsync_UserNotFound_ReadableException()
        {
            // Arrange
            var distributedCache = new Mock<IDistributedCache>();
            var keycloakApiClient = new Mock<IKeycloakUserApiClient>();
            var logger = new Mock<ILogger<UserService>>();
            var authorizationData = new Mock<IAuthorizationData>();
            var service = new UserService(distributedCache.Object, keycloakApiClient.Object,
                logger.Object, authorizationData.Object);

            var userId = Guid.Parse("cf80f652-0d56-4d7c-b315-38eab5c3b0bc");

            keycloakApiClient.Setup(c => c.GetUserAsync(userId, CancellationToken.None))
                             .ReturnsAsync((KeycloakUserData?)null);

            // Act
            var exception = Assert.ThrowsAsync<ReadableException>(
                                async () => await service.GetUserAsync(userId, CancellationToken.None));

            // Assert
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception, Is.TypeOf<ReadableException>());
            Assert.That(exception.Message, Is.EqualTo("Пользователь не найден"));
        }

        [Test(Description = "При успешном получении пользователя должны возвращаться его данные")]
        public async Task GetUserAsync_UserFound_ReturnUser()
        {
            // Arrange
            var distributedCache = new Mock<IDistributedCache>();
            var keycloakApiClient = new Mock<IKeycloakUserApiClient>();
            var logger = new Mock<ILogger<UserService>>();
            var authorizationData = new Mock<IAuthorizationData>();
            var service = new UserService(distributedCache.Object, keycloakApiClient.Object, 
                logger.Object, authorizationData.Object);

            var userId = Guid.Parse("cf80f652-0d56-4d7c-b315-38eab5c3b0bc");
            var user = new KeycloakUserData(Guid.Parse("351e765a-89ad-4eb5-9b82-ab04b573e185"))
            {
                Email = "test@email.com",
                FirstName = "FirstName",
                LastName = "LastName",
                Sex = Common.Clients.Keycloak.Models.Enums.KeycloakSexType.Man,
                IsEmailVerified = true,
                PhoneNumber = "79999999999",
                BirthDate = new DateTime(2020, 1, 2),
                IsEnabled = true,
                Roles = [new KeycloakUserRoleData(RoleTypes.Client.ToString()) { Name = RoleTypes.Client.ToString() }]
            };
            keycloakApiClient.Setup(c => c.GetUserAsync(userId, CancellationToken.None))
                             .ReturnsAsync((KeycloakUserData?)user);

            // Act
            var result = await service.GetUserAsync(userId, CancellationToken.None);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.IsEmailVerified, Is.True);
                Assert.That(result.Email, Is.EqualTo(user.Email));
                Assert.That(result.PhoneNumber, Is.EqualTo(user.PhoneNumber));
                Assert.That(result.LastName, Is.EqualTo(user.LastName));
                Assert.That(result.FirstName, Is.EqualTo(user.FirstName));
                Assert.That(result.BirthDate, Is.EqualTo(user.BirthDate));
                Assert.That(result.Sex, Is.EqualTo((SexType?)user.Sex));
            });
        }

        [Test(Description = "Если при изменении пользователя выбрашено исключение, оно не должно обрабатываться")]
        public void UpdateUserAsync_NotFoundException_NotFoundException()
        {
            // Arrange
            var distributedCache = new Mock<IDistributedCache>();
            var keycloakApiClient = new Mock<IKeycloakUserApiClient>();
            var logger = new Mock<ILogger<UserService>>();
            var authorizationData = new Mock<IAuthorizationData>();
            var service = new UserService(distributedCache.Object, keycloakApiClient.Object, 
                logger.Object, authorizationData.Object);

            var userId = Guid.Parse("cf80f652-0d56-4d7c-b315-38eab5c3b0bc");
            var user = new UserModel(userId);
            keycloakApiClient.Setup(c => c.UpdateUserAsync(It.IsAny<KeycloakUserData>(), CancellationToken.None))
                             .ThrowsAsync(new NotFoundException("Ошибка"));

            // Act
            var exception = Assert.ThrowsAsync<NotFoundException>(async () =>
                                await service.UpdateUserAsync(user, CancellationToken.None));

            // Assert
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.EqualTo("Ошибка"));
        }

        [Test(Description = "Если при изменении пользователя не было ошибок, то при изменении не должно быть исключений")]
        public void UpdateUserAsync_ValidData_NoException()
        {
            // Arrange
            var distributedCache = new Mock<IDistributedCache>();
            var keycloakApiClient = new Mock<IKeycloakUserApiClient>();
            var logger = new Mock<ILogger<UserService>>();
            var authorizationData = new Mock<IAuthorizationData>();
            var service = new UserService(distributedCache.Object, keycloakApiClient.Object, 
                logger.Object, authorizationData.Object);

            var userId = Guid.Parse("cf80f652-0d56-4d7c-b315-38eab5c3b0bc");
            var user = new UserModel(userId);

            // Act
            // Assert
            Assert.DoesNotThrowAsync(async () => await service.UpdateUserAsync(user, CancellationToken.None));
        }

        [Test(Description = "Если пользователей нет, то возвращается пустые данные")]
        public async Task GetPagedUsersAsync_UsersNotFound_EmpltyData()
        {
            // Arrange
            var distributedCache = new Mock<IDistributedCache>();
            var keycloakApiClient = new Mock<IKeycloakUserApiClient>();
            var logger = new Mock<ILogger<UserService>>();
            var authorizationData = new Mock<IAuthorizationData>();
            var service = new UserService(distributedCache.Object, keycloakApiClient.Object,
                logger.Object, authorizationData.Object);

            keycloakApiClient.Setup(c => c.GetUsersCountByFilterAsync(It.IsAny<UserFilter>(), CancellationToken.None))
                             .ReturnsAsync(0);
            var filter = new GetPagedUsersModel(10, 0);

            // Act
            var result = await service.GetPagedUsersAsync(filter, CancellationToken.None);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Page, Has.Count.Zero);
            Assert.That(result.Total, Is.Zero);
        }

        [Test(Description = "Если пользователи есть, то возвращаются их данные")]
        public async Task GetPagedUsersAsync_Usersound_FullData()
        {
            // Arrange
            var distributedCache = new Mock<IDistributedCache>();
            var keycloakApiClient = new Mock<IKeycloakUserApiClient>();
            var logger = new Mock<ILogger<UserService>>();
            var authorizationData = new Mock<IAuthorizationData>();
            var service = new UserService(distributedCache.Object, keycloakApiClient.Object,
                logger.Object, authorizationData.Object);

            keycloakApiClient.Setup(c => c.GetUsersCountByFilterAsync(It.IsAny<UserFilter>(), CancellationToken.None))
                             .ReturnsAsync(1);
            keycloakApiClient.Setup(c => c.GetUsersByFilterAsync(It.IsAny<UserFilter>(), CancellationToken.None))
                             .ReturnsAsync([new KeycloakUsersListItemData(Guid.Parse("2e5e581c-0ff7-430a-9180-9d2f26572a3e")) 
                                { 
                                    Roles = [new KeycloakUserRoleData(RoleTypes.Client.ToString()) { Name = RoleTypes.Client.ToString() }]
                                }]);
            var filter = new GetPagedUsersModel(10, 0);

            // Act
            var result = await service.GetPagedUsersAsync(filter, CancellationToken.None);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Page, Has.Count.EqualTo(1));
            Assert.That(result.Total, Is.EqualTo(1));
        }

        [Test(Description = "Проверяется вызов метода смены статуса")]
        public async Task SetStatusUserAsync_ValidData_SetStatusCalled()
        {
            // Arrange
            var distributedCache = new Mock<IDistributedCache>();
            var keycloakApiClient = new Mock<IKeycloakUserApiClient>();
            var logger = new Mock<ILogger<UserService>>();
            var authorizationData = new Mock<IAuthorizationData>();
            var service = new UserService(distributedCache.Object, keycloakApiClient.Object,
                logger.Object, authorizationData.Object);
            var userId = Guid.Parse("2e5e581c-0ff7-430a-9180-9d2f26572a3e");

            // Act
            await service.SetStatusUserAsync(userId, true, CancellationToken.None);

            // Assert
            keycloakApiClient.Verify(c => c.SetStatusUserAsync(userId, true, CancellationToken.None), Times.Once);
        }

        [Test(Description = "Проверяется вызов метода смены роли")]
        public async Task ChangeUserRoleAsync_ValidData_SetStatusCalled()
        {
            // Arrange
            var distributedCache = new Mock<IDistributedCache>();
            var keycloakApiClient = new Mock<IKeycloakUserApiClient>();
            var logger = new Mock<ILogger<UserService>>();
            var authorizationData = new Mock<IAuthorizationData>();
            var service = new UserService(distributedCache.Object, keycloakApiClient.Object,
                logger.Object, authorizationData.Object);
            var userId = Guid.Parse("2e5e581c-0ff7-430a-9180-9d2f26572a3e");

            // Act
            await service.ChangeUserRoleAsync(userId, RoleTypes.Administrator, CancellationToken.None);

            // Assert
            keycloakApiClient.Verify(c => c.ChangeUserRoleAsync(userId, RoleTypes.Administrator, CancellationToken.None), Times.Once);
        }
    }
}
