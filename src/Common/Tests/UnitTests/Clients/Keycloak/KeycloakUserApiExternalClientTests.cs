using InsuranceGoSmoke.Common.Clients.Keycloak;
using InsuranceGoSmoke.Common.Clients.Keycloak.Generated;
using InsuranceGoSmoke.Common.Clients.Keycloak.Models;
using InsuranceGoSmoke.Common.Contracts.Contracts.Access.Enums;
using InsuranceGoSmoke.Common.Contracts.Exceptions.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using static IdentityModel.ClaimComparer;
using System.Net.Http;
using Microsoft.Extensions.Caching.Memory;

namespace InsuranceGoSmoke.Common.Tests.UnitTests.Clients.Keycloak
{
    internal class KeycloakUserApiExternalClientTests
    {
        [Test(Description = "Если при получении пользователя проихошло исключение, то выбрасывается обернутое исключение.")]
        public void GetUserAsync_NotFoundException_KeycloakApiException()
        {
            // Arrange
            var config = new KeycloakAuthorizationOptions();
            config.Authority = @"http://localhost";
            var options = new Mock<IOptions<KeycloakAuthorizationOptions>>();
            options.Setup(o => o.Value).Returns(config);
            var externalApiClient = new Mock<IKeycloakGeneratedExternalApiClient>();
            var logger = new Mock<ILogger<KeycloakUserApiExternalClient>>();
            var httpClientFactory = new Mock<IHttpClientFactory>();
            var httpContextAccessor = new Mock<IHttpContextAccessor>();
            var cache = new Mock<IDistributedCache>();

            var client = new KeycloakUserApiExternalClient(options.Object, externalApiClient.Object, 
                logger.Object, httpClientFactory.Object, httpContextAccessor.Object, cache.Object);
            var userId = Guid.Parse("cf80f652-0d56-4d7c-b315-38eab5c3b0bc");
            externalApiClient.Setup(c => c.UsersGET2Async(It.IsAny<string>(), userId.ToString(), It.IsAny<bool?>(), CancellationToken.None))
                             .ThrowsAsync(new NotFoundException("Ошибка"));

            // Act
            var exception = Assert.ThrowsAsync<KeycloakApiException>(
                            async () => await client.GetUserAsync(userId, CancellationToken.None));

            // Assert
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.InnerException, Is.Not.Null);
            Assert.That(exception.InnerException, Is.TypeOf<NotFoundException>());
            Assert.That(exception.InnerException.Message, Is.EqualTo("Ошибка"));
        }

        [Test(Description = "Если пользователя нет, возвращается null.")]
        public async Task GetUserAsync_UserIsNotFound_ReturnNull()
        {
            // Arrange
            var config = new KeycloakAuthorizationOptions();
            config.Authority = @"http://localhost";
            var options = new Mock<IOptions<KeycloakAuthorizationOptions>>();
            options.Setup(o => o.Value).Returns(config);
            var externalApiClient = new Mock<IKeycloakGeneratedExternalApiClient>();
            var logger = new Mock<ILogger<KeycloakUserApiExternalClient>>();
            var httpClientFactory = new Mock<IHttpClientFactory>();
            var httpContextAccessor = new Mock<IHttpContextAccessor>();
            var cache = new Mock<IDistributedCache>();

            var client = new KeycloakUserApiExternalClient(options.Object, externalApiClient.Object,
                logger.Object, httpClientFactory.Object, httpContextAccessor.Object, cache.Object);
            var userId = Guid.Parse("cf80f652-0d56-4d7c-b315-38eab5c3b0bc");
            externalApiClient.Setup(c => c.UsersGET2Async(It.IsAny<string>(), userId.ToString(), It.IsAny<bool?>(), CancellationToken.None))
                             .ReturnsAsync((UserRepresentation?)null);

            // Act
            var result = await client.GetUserAsync(userId, CancellationToken.None);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test(Description = "При успешном получении пользователя, должны возвращаться пользовательские данные.")]
        public async Task GetUserAsync_User_ReturnUser()
        {
            // Arrange
            var config = new KeycloakAuthorizationOptions();
            config.Authority = @"http://localhost";
            var options = new Mock<IOptions<KeycloakAuthorizationOptions>>();
            options.Setup(o => o.Value).Returns(config);
            var externalApiClient = new Mock<IKeycloakGeneratedExternalApiClient>();
            var logger = new Mock<ILogger<KeycloakUserApiExternalClient>>();
            var httpClientFactory = new Mock<IHttpClientFactory>();
            var httpContextAccessor = new Mock<IHttpContextAccessor>();
            var cache = new Mock<IDistributedCache>();

            var client = new KeycloakUserApiExternalClient(options.Object, externalApiClient.Object,
                logger.Object, httpClientFactory.Object, httpContextAccessor.Object, cache.Object);
            var userId = Guid.Parse("cf80f652-0d56-4d7c-b315-38eab5c3b0bc");
            var user = new UserRepresentation() { Id = userId.ToString() };
            externalApiClient.Setup(c => c.UsersGET2Async(It.IsAny<string>(), user.Id, It.IsAny<bool?>(), CancellationToken.None))
                             .ReturnsAsync(user);
            externalApiClient.Setup(c => c.ClientsAll9Async(It.IsAny<string>(), user.Id, It.IsAny<string>(), CancellationToken.None))
                             .ReturnsAsync([]);

            // Act
            var result = await client.GetUserAsync(userId, CancellationToken.None);

            // Assert
            Assert.That(result, Is.Not.Null);
        }

        [Test(Description = "Если при получении пользователя выбрасилось исключение, оно должно быть обернуто.")]
        public void UpdateUserAsync_GetUserException_KeycloakApiException()
        {
            // Arrange
            var config = new KeycloakAuthorizationOptions();
            config.Authority = @"http://localhost";
            var options = new Mock<IOptions<KeycloakAuthorizationOptions>>();
            options.Setup(o => o.Value).Returns(config);
            var externalApiClient = new Mock<IKeycloakGeneratedExternalApiClient>();
            var logger = new Mock<ILogger<KeycloakUserApiExternalClient>>();
            var httpClientFactory = new Mock<IHttpClientFactory>();
            var httpContextAccessor = new Mock<IHttpContextAccessor>();
            var cache = new Mock<IDistributedCache>();

            var client = new KeycloakUserApiExternalClient(options.Object, externalApiClient.Object,
                logger.Object, httpClientFactory.Object, httpContextAccessor.Object, cache.Object);
            var userId = Guid.Parse("cf80f652-0d56-4d7c-b315-38eab5c3b0bc");
            var newData = new KeycloakUserData(userId) { PhoneNumber="79999999999", IsEnabled = true };
            externalApiClient.Setup(c => c.UsersGET2Async(It.IsAny<string>(), userId.ToString(), It.IsAny<bool?>(), CancellationToken.None))
                             .ThrowsAsync(new NotFoundException("Ошибка"));

            // Act
            var exception = Assert.ThrowsAsync<KeycloakApiException>(async () => await client.UpdateUserAsync(newData, CancellationToken.None));

            // Assert
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.InnerException, Is.Not.Null);
            Assert.That(exception.InnerException.Message, Is.EqualTo("Произошла ошибка при получении пользователя по идентификатору"));
        }

        [Test(Description = "Если при получении пользователя вернулось NULL, должно быть выбрашено исключение.")]
        public void UpdateUserAsync_GetUserNull_KeycloakApiException()
        {
            // Arrange
            var config = new KeycloakAuthorizationOptions();
            config.Authority = @"http://localhost";
            var options = new Mock<IOptions<KeycloakAuthorizationOptions>>();
            options.Setup(o => o.Value).Returns(config);
            var externalApiClient = new Mock<IKeycloakGeneratedExternalApiClient>();
            var logger = new Mock<ILogger<KeycloakUserApiExternalClient>>();
            var httpClientFactory = new Mock<IHttpClientFactory>();
            var httpContextAccessor = new Mock<IHttpContextAccessor>();
            var cache = new Mock<IDistributedCache>();

            var client = new KeycloakUserApiExternalClient(options.Object, externalApiClient.Object,
                logger.Object, httpClientFactory.Object, httpContextAccessor.Object, cache.Object);
            var userId = Guid.Parse("cf80f652-0d56-4d7c-b315-38eab5c3b0bc");
            var newData = new KeycloakUserData(userId) { PhoneNumber="79999999999", IsEnabled = true };
            externalApiClient.Setup(c => c.UsersGET2Async(It.IsAny<string>(), userId.ToString(), It.IsAny<bool?>(), CancellationToken.None))
                             .ReturnsAsync((UserRepresentation?)null);

            // Act
            var exception = Assert.ThrowsAsync<KeycloakApiException>(async () => await client.UpdateUserAsync(newData, CancellationToken.None));

            // Assert
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.InnerException, Is.Not.Null);
            Assert.That(exception.InnerException.Message, Is.EqualTo($"Пользователь '{userId}' не найден"));
        }

        [Test(Description = "Если при изменении пользователя выбрасилось исключение, оно должно быть обернуто.")]
        public void UpdateUserAsync_UpdateUserException_KeycloakApiException()
        {
            // Arrange
            var config = new KeycloakAuthorizationOptions();
            config.Authority = @"http://localhost";
            var options = new Mock<IOptions<KeycloakAuthorizationOptions>>();
            options.Setup(o => o.Value).Returns(config);
            var externalApiClient = new Mock<IKeycloakGeneratedExternalApiClient>();
            var logger = new Mock<ILogger<KeycloakUserApiExternalClient>>();
            var httpClientFactory = new Mock<IHttpClientFactory>();
            var httpContextAccessor = new Mock<IHttpContextAccessor>();
            var cache = new Mock<IDistributedCache>();

            var client = new KeycloakUserApiExternalClient(options.Object, externalApiClient.Object,
                logger.Object, httpClientFactory.Object, httpContextAccessor.Object, cache.Object);
            var userId = Guid.Parse("cf80f652-0d56-4d7c-b315-38eab5c3b0bc");
            var keycloakUser = new UserRepresentation() { Id = userId.ToString() };
            var newData = new KeycloakUserData(userId) { PhoneNumber="79999999999", IsEnabled = true };
            externalApiClient.Setup(c => c.UsersGET2Async(It.IsAny<string>(), userId.ToString(), It.IsAny<bool?>(), CancellationToken.None))
                             .ReturnsAsync(keycloakUser);
            externalApiClient.Setup(c => c.UsersPUTAsync(It.IsAny<string>(), userId.ToString(), It.IsAny<UserRepresentation>(), CancellationToken.None))
                             .ThrowsAsync(new NotFoundException("Ошибка"));

            // Act
            var exception = Assert.ThrowsAsync<KeycloakApiException>(async () => await client.UpdateUserAsync(newData, CancellationToken.None));

            // Assert
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.InnerException, Is.Not.Null);
            Assert.That(exception.InnerException.Message, Is.EqualTo("Произошла ошибка при изменении данных пользователя"));
        }

        [Test(Description = "При успешном редактировании пользователя, методы keycloak должны вызываться один раз.")]
        public async Task UpdateUserAsync_ValidData_NoException()
        {
            // Arrange
            var config = new KeycloakAuthorizationOptions();
            config.Authority = @"http://localhost";
            var options = new Mock<IOptions<KeycloakAuthorizationOptions>>();
            options.Setup(o => o.Value).Returns(config);
            var externalApiClient = new Mock<IKeycloakGeneratedExternalApiClient>();
            var logger = new Mock<ILogger<KeycloakUserApiExternalClient>>();
            var httpClientFactory = new Mock<IHttpClientFactory>();
            var httpContextAccessor = new Mock<IHttpContextAccessor>();
            var cache = new Mock<IDistributedCache>();

            var client = new KeycloakUserApiExternalClient(options.Object, externalApiClient.Object,
                logger.Object, httpClientFactory.Object, httpContextAccessor.Object, cache.Object);
            var userId = Guid.Parse("cf80f652-0d56-4d7c-b315-38eab5c3b0bc");
            var keycloakUser = new UserRepresentation() 
            { 
                Id = userId.ToString(), 
                Attributes = new Dictionary<string, ICollection<string>>()
                {
                    { "MiddleName", ["middleName"]}
                }
            };
            var newData = new KeycloakUserData(userId) { PhoneNumber = "79999999999", IsEnabled = true };
            externalApiClient.Setup(c => c.UsersGET2Async(It.IsAny<string>(), userId.ToString(), It.IsAny<bool?>(), CancellationToken.None))
                             .ReturnsAsync(keycloakUser);

            // Act
            await client.UpdateUserAsync(newData, CancellationToken.None);

            // Assert
            externalApiClient.Verify(c => c.UsersGET2Async(It.IsAny<string>(), userId.ToString(), It.IsAny<bool?>(), CancellationToken.None), Times.Once);
            externalApiClient.Verify(c => c.UsersPUTAsync(It.IsAny<string>(), userId.ToString(), It.IsAny<UserRepresentation>(), CancellationToken.None), Times.Once);
        }

        [Test(Description = "Если при попытке отправить смс падает ошибка 400, то должна возвращаться ошибка.")]
        public async Task TrySendSmsVerificationCodeAsync_BadRequest_ErrorResult()
        {
            // Arrange
            var mockHttp = new Mock<HttpMessageHandler>();
            mockHttp.Protected()
                    .Setup<Task<HttpResponseMessage>>(
                        "SendAsync",
                        ItExpr.Is<HttpRequestMessage>(m => m.Method == HttpMethod.Get),
                        ItExpr.IsAny<CancellationToken>())
                    .ReturnsAsync(new HttpResponseMessage() { StatusCode = System.Net.HttpStatusCode.BadRequest });
            var httpClient = new HttpClient(mockHttp.Object);

            var config = new KeycloakAuthorizationOptions();
            config.Authority = @"http://localhost";
            var options = new Mock<IOptions<KeycloakAuthorizationOptions>>();
            options.Setup(o => o.Value).Returns(config);
            var externalApiClient = new Mock<IKeycloakGeneratedExternalApiClient>();
            var logger = new Mock<ILogger<KeycloakUserApiExternalClient>>();
            var httpClientFactory = new Mock<IHttpClientFactory>();
            var httpContextAccessor = new Mock<IHttpContextAccessor>();
            var cache = new Mock<IDistributedCache>();

            var client = new KeycloakUserApiExternalClient(options.Object, externalApiClient.Object,
                logger.Object, httpClientFactory.Object, httpContextAccessor.Object, cache.Object);

            var phoneNumber = "123";
            httpClientFactory.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(httpClient);

            // Act
            var result = await client.TrySendSmsVerificationCodeAsync(phoneNumber, CancellationToken.None);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.IsError, Is.True);
                Assert.That(result.ErrorDescription, Does.Contain(System.Net.HttpStatusCode.BadRequest.ToString()));
            });
        }

        [Test(Description = "Если при попытке отправить смс падает исключение, то должна возвращаться ошибка.")]
        public async Task TrySendSmsVerificationCodeAsync_Exception_ErrorResult()
        {
            // Arrange
            var mockHttp = new Mock<HttpMessageHandler>();
            mockHttp.Protected()
                    .Setup<Task<HttpResponseMessage>>(
                        "SendAsync",
                        ItExpr.Is<HttpRequestMessage>(m => m.Method == HttpMethod.Get),
                        ItExpr.IsAny<CancellationToken>())
                    .Throws(new Exception("Ошибка"));
            var httpClient = new HttpClient(mockHttp.Object);

            var config = new KeycloakAuthorizationOptions();
            config.Authority = @"http://localhost";
            var options = new Mock<IOptions<KeycloakAuthorizationOptions>>();
            options.Setup(o => o.Value).Returns(config);
            var externalApiClient = new Mock<IKeycloakGeneratedExternalApiClient>();
            var logger = new Mock<ILogger<KeycloakUserApiExternalClient>>();
            var httpClientFactory = new Mock<IHttpClientFactory>();
            var httpContextAccessor = new Mock<IHttpContextAccessor>();
            var cache = new Mock<IDistributedCache>();

            var client = new KeycloakUserApiExternalClient(options.Object, externalApiClient.Object,
                logger.Object, httpClientFactory.Object, httpContextAccessor.Object, cache.Object);

            var phoneNumber = "123";
            httpClientFactory.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(httpClient);

            // Act
            var result = await client.TrySendSmsVerificationCodeAsync(phoneNumber, CancellationToken.None);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.IsError, Is.True);
                Assert.That(result.ErrorDescription, Does.Contain("Ошибка"));
            });
        }

        [Test(Description = "Если при попытке отправить смс нет ошибок, то успешный ответ.")]
        public async Task TrySendSmsVerificationCodeAsync_OkRequest_SuccesResult()
        {
            // Arrange
            var mockHttp = new Mock<HttpMessageHandler>();
            mockHttp.Protected()
                    .Setup<Task<HttpResponseMessage>>(
                        "SendAsync",
                        ItExpr.Is<HttpRequestMessage>(m => m.Method == HttpMethod.Get),
                        ItExpr.IsAny<CancellationToken>())
                    .ReturnsAsync(new HttpResponseMessage() { StatusCode = System.Net.HttpStatusCode.OK });
            var httpClient = new HttpClient(mockHttp.Object);

            var config = new KeycloakAuthorizationOptions();
            config.Authority = @"http://localhost";
            var options = new Mock<IOptions<KeycloakAuthorizationOptions>>();
            options.Setup(o => o.Value).Returns(config);
            var externalApiClient = new Mock<IKeycloakGeneratedExternalApiClient>();
            var logger = new Mock<ILogger<KeycloakUserApiExternalClient>>();
            var httpClientFactory = new Mock<IHttpClientFactory>();
            var httpContextAccessor = new Mock<IHttpContextAccessor>();
            var cache = new Mock<IDistributedCache>();

            var client = new KeycloakUserApiExternalClient(options.Object, externalApiClient.Object,
                logger.Object, httpClientFactory.Object, httpContextAccessor.Object, cache.Object);
            httpClientFactory.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(httpClient);

            var phoneNumber = "123";

            // Act
            var result = await client.TrySendSmsVerificationCodeAsync(phoneNumber, CancellationToken.None);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.IsError, Is.False);
                Assert.That(result.ErrorDescription, Is.Empty);
            });
        }

        [Test(Description = "Если при попытке получить доступные роли выбрасывается исключение, то оно оборачивается.")]
        public void ChangeUserRoleAsync_GetAvailableRolesThrowException_KeycloakApiException()
        {
            // Arrange
            var config = new KeycloakAuthorizationOptions();
            config.Authority = @"http://localhost";
            var options = new Mock<IOptions<KeycloakAuthorizationOptions>>();
            options.Setup(o => o.Value).Returns(config);
            var externalApiClient = new Mock<IKeycloakGeneratedExternalApiClient>();
            var httpClientFactory = new Mock<IHttpClientFactory>();
            var httpContextAccessor = new Mock<IHttpContextAccessor>();
            var logger = new Mock<ILogger<KeycloakUserApiExternalClient>>();
            var cache = new Mock<IDistributedCache>();

            var userId = Guid.Parse("cf80f652-0d56-4d7c-b315-38eab5c3b0bc");
            var client = new KeycloakUserApiExternalClient(options.Object, externalApiClient.Object,
                logger.Object, httpClientFactory.Object, httpContextAccessor.Object, cache.Object);

            externalApiClient.Setup(c => c.Available9Async(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new NotImplementedException("Ошибка"));

            // Act
            var exception = Assert.ThrowsAsync<KeycloakApiException>(async () => 
                await client.ChangeUserRoleAsync(userId, RoleTypes.Administrator.ToString(), CancellationToken.None));

            // Assert
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.InnerException, Is.TypeOf<NotImplementedException>());
        }

        [Test(Description = "Если при попытке получить доступные роли отсутствуют, то выбрасывается исключение.")]
        public void ChangeUserRoleAsync_AvailableRolesNone_KeycloakApiException()
        {
            // Arrange
            var config = new KeycloakAuthorizationOptions();
            config.Authority = @"http://localhost";
            var options = new Mock<IOptions<KeycloakAuthorizationOptions>>();
            options.Setup(o => o.Value).Returns(config);
            var externalApiClient = new Mock<IKeycloakGeneratedExternalApiClient>();
            var httpClientFactory = new Mock<IHttpClientFactory>();
            var httpContextAccessor = new Mock<IHttpContextAccessor>();
            var logger = new Mock<ILogger<KeycloakUserApiExternalClient>>();
            var cache = new Mock<IDistributedCache>();

            var userId = Guid.Parse("cf80f652-0d56-4d7c-b315-38eab5c3b0bc");
            var client = new KeycloakUserApiExternalClient(options.Object, externalApiClient.Object,
                logger.Object, httpClientFactory.Object, httpContextAccessor.Object, cache.Object);

            externalApiClient.Setup(c => c.Available9Async(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync([]);

            // Act
            var exception = Assert.ThrowsAsync<KeycloakApiException>(async () =>
                await client.ChangeUserRoleAsync(userId, RoleTypes.Administrator.ToString(), CancellationToken.None));

            // Assert
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.InnerException, Is.TypeOf<NotFoundException>());
        }

        [Test(Description = "Если при попытке получить роли пользователя выбрасывается исключение, то оно оборачивается.")]
        public void ChangeUserRoleAsync_GetUserRolesThrowException_KeycloakApiException()
        {
            // Arrange
            var config = new KeycloakAuthorizationOptions();
            config.Authority = @"http://localhost";
            var options = new Mock<IOptions<KeycloakAuthorizationOptions>>();
            options.Setup(o => o.Value).Returns(config);
            var externalApiClient = new Mock<IKeycloakGeneratedExternalApiClient>();
            var httpClientFactory = new Mock<IHttpClientFactory>();
            var httpContextAccessor = new Mock<IHttpContextAccessor>();
            var logger = new Mock<ILogger<KeycloakUserApiExternalClient>>();
            var cache = new Mock<IDistributedCache>();

            var userId = Guid.Parse("cf80f652-0d56-4d7c-b315-38eab5c3b0bc");
            var client = new KeycloakUserApiExternalClient(options.Object, externalApiClient.Object,
                logger.Object, httpClientFactory.Object, httpContextAccessor.Object, cache.Object);

            externalApiClient.Setup(c => c.Available9Async(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync([new RoleRepresentation() { Name = RoleTypes.Administrator.ToString() }]);
            externalApiClient.Setup(c => c.ClientsAll9Async(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new NotImplementedException("Ошибка"));

            // Act
            var exception = Assert.ThrowsAsync<KeycloakApiException>(async () =>
                await client.ChangeUserRoleAsync(userId, RoleTypes.Administrator.ToString(), CancellationToken.None));

            // Assert
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.InnerException, Is.TypeOf<NotImplementedException>());
        }

        [Test(Description = "Если при попытке удалить роли пользователя выбрасывается исключение, то оно оборачивается.")]
        public void ChangeUserRoleAsync_DeleteUserRolesThrowException_KeycloakApiException()
        {
            // Arrange
            var config = new KeycloakAuthorizationOptions();
            config.Authority = @"http://localhost";
            var options = new Mock<IOptions<KeycloakAuthorizationOptions>>();
            options.Setup(o => o.Value).Returns(config);
            var externalApiClient = new Mock<IKeycloakGeneratedExternalApiClient>();
            var httpClientFactory = new Mock<IHttpClientFactory>();
            var httpContextAccessor = new Mock<IHttpContextAccessor>();
            var logger = new Mock<ILogger<KeycloakUserApiExternalClient>>();
            var cache = new Mock<IDistributedCache>();

            var userId = Guid.Parse("cf80f652-0d56-4d7c-b315-38eab5c3b0bc");
            var client = new KeycloakUserApiExternalClient(options.Object, externalApiClient.Object,
                logger.Object, httpClientFactory.Object, httpContextAccessor.Object, cache.Object);

            externalApiClient.Setup(c => c.Available9Async(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync([new RoleRepresentation() { Name = RoleTypes.Administrator.ToString() }]);
            externalApiClient.Setup(c => c.ClientsAll9Async(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync([new RoleRepresentation()]);
            externalApiClient.Setup(c => c.ClientsPOST6Async(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                    It.IsAny<IEnumerable<RoleRepresentation>>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new NotImplementedException("Ошибка"));

            // Act
            var exception = Assert.ThrowsAsync<KeycloakApiException>(async () =>
                await client.ChangeUserRoleAsync(userId, RoleTypes.Administrator.ToString(), CancellationToken.None));

            // Assert
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.InnerException, Is.TypeOf<NotImplementedException>());
        }

        [Test(Description = "Если при попытке удалить кэш ролей пользователя выбрасывается исключение, то оно оборачивается.")]
        public void ChangeUserRoleAsync_DeleteCacheThrowException_KeycloakApiException()
        {
            // Arrange
            var config = new KeycloakAuthorizationOptions();
            config.Authority = @"http://localhost";
            var options = new Mock<IOptions<KeycloakAuthorizationOptions>>();
            options.Setup(o => o.Value).Returns(config);
            var externalApiClient = new Mock<IKeycloakGeneratedExternalApiClient>();
            var httpClientFactory = new Mock<IHttpClientFactory>();
            var httpContextAccessor = new Mock<IHttpContextAccessor>();
            var logger = new Mock<ILogger<KeycloakUserApiExternalClient>>();
            var cache = new Mock<IDistributedCache>();

            var userId = Guid.Parse("cf80f652-0d56-4d7c-b315-38eab5c3b0bc");
            var client = new KeycloakUserApiExternalClient(options.Object, externalApiClient.Object,
                logger.Object, httpClientFactory.Object, httpContextAccessor.Object, cache.Object);

            externalApiClient.Setup(c => c.Available9Async(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync([new RoleRepresentation() { Name = RoleTypes.Administrator.ToString() }]);
            externalApiClient.Setup(c => c.ClientsAll9Async(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync([new RoleRepresentation()]);
            cache.Setup(c => c.RemoveAsync(It.IsAny<string>(), CancellationToken.None))
                 .ThrowsAsync(new NotImplementedException("Ошибка"));

            // Act
            var exception = Assert.ThrowsAsync<KeycloakApiException>(async () =>
                await client.ChangeUserRoleAsync(userId, RoleTypes.Administrator.ToString(), CancellationToken.None));

            // Assert
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.InnerException, Is.Not.Null);
            Assert.That(exception.InnerException, Is.TypeOf<NotImplementedException>());
        }

        [Test(Description = "Если при попытке изменения роли не было ошибок, то роль меняется.")]
        public async Task ChangeUserRoleAsync_ValidData_RoleChanged()
        {
            // Arrange
            var config = new KeycloakAuthorizationOptions();
            config.Authority = @"http://localhost";
            var options = new Mock<IOptions<KeycloakAuthorizationOptions>>();
            options.Setup(o => o.Value).Returns(config);
            var externalApiClient = new Mock<IKeycloakGeneratedExternalApiClient>();
            var httpClientFactory = new Mock<IHttpClientFactory>();
            var httpContextAccessor = new Mock<IHttpContextAccessor>();
            var logger = new Mock<ILogger<KeycloakUserApiExternalClient>>();
            var cache = new Mock<IDistributedCache>();

            var userId = Guid.Parse("cf80f652-0d56-4d7c-b315-38eab5c3b0bc");
            var client = new KeycloakUserApiExternalClient(options.Object, externalApiClient.Object,
                logger.Object, httpClientFactory.Object, httpContextAccessor.Object, cache.Object);

            externalApiClient.Setup(c => c.Available9Async(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync([new RoleRepresentation() { Name = RoleTypes.Administrator.ToString() }]);
            externalApiClient.Setup(c => c.ClientsAll9Async(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync([new RoleRepresentation()]);

            // Act
            await client.ChangeUserRoleAsync(userId, RoleTypes.Administrator.ToString(), CancellationToken.None);

            // Assert
            externalApiClient.Verify(c => c.Available9Async(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
            externalApiClient.Verify(c => c.ClientsAll9Async(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
            externalApiClient.Verify(c => c.ClientsPOST6Async(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IEnumerable<RoleRepresentation>>(), It.IsAny<CancellationToken>()), Times.Once);
            externalApiClient.Verify(c => c.ClientsDELETE6Async(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IEnumerable<RoleRepresentation>>(), It.IsAny<CancellationToken>()), Times.Once);
            cache.Verify(c => c.RemoveAsync(It.IsAny<string>(), CancellationToken.None), Times.Once);
        }

        [Test(Description = "Если при изменение статуса пользователя, получение пользователя упало с ошибкой, то она оборачивается.")]
        public void SetStatusUserAsync_GetUserThrowException_KeycloakApiException()
        {
            // Arrange
            var config = new KeycloakAuthorizationOptions();
            config.Authority = @"http://localhost";
            var options = new Mock<IOptions<KeycloakAuthorizationOptions>>();
            options.Setup(o => o.Value).Returns(config);
            var externalApiClient = new Mock<IKeycloakGeneratedExternalApiClient>();
            var httpClientFactory = new Mock<IHttpClientFactory>();
            var httpContextAccessor = new Mock<IHttpContextAccessor>();
            var logger = new Mock<ILogger<KeycloakUserApiExternalClient>>();
            var cache = new Mock<IDistributedCache>();

            var userId = Guid.Parse("cf80f652-0d56-4d7c-b315-38eab5c3b0bc");
            var client = new KeycloakUserApiExternalClient(options.Object, externalApiClient.Object,
                logger.Object, httpClientFactory.Object, httpContextAccessor.Object, cache.Object);

            externalApiClient.Setup(c => c.UsersGET2Async(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool?>(), CancellationToken.None))
                .ThrowsAsync(new NotImplementedException("Ошибка"));

            // Act
            var exception = Assert.ThrowsAsync<KeycloakApiException>(async () =>
                await client.SetStatusUserAsync(userId, true, CancellationToken.None));

            // Assert
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.InnerException, Is.TypeOf<KeycloakApiException>());
        }

        [Test(Description = "Если при изменение статуса пользователя, получить пользователя не удалось, то выбрасывается исключение.")]
        public void SetStatusUserAsync_UserNotFound_KeycloakApiException()
        {
            // Arrange
            var config = new KeycloakAuthorizationOptions();
            config.Authority = @"http://localhost";
            var options = new Mock<IOptions<KeycloakAuthorizationOptions>>();
            options.Setup(o => o.Value).Returns(config);
            var externalApiClient = new Mock<IKeycloakGeneratedExternalApiClient>();
            var httpClientFactory = new Mock<IHttpClientFactory>();
            var httpContextAccessor = new Mock<IHttpContextAccessor>();
            var logger = new Mock<ILogger<KeycloakUserApiExternalClient>>();
            var cache = new Mock<IDistributedCache>();

            var userId = Guid.Parse("cf80f652-0d56-4d7c-b315-38eab5c3b0bc");
            var client = new KeycloakUserApiExternalClient(options.Object, externalApiClient.Object,
                logger.Object, httpClientFactory.Object, httpContextAccessor.Object, cache.Object);

            externalApiClient.Setup(c => c.UsersGET2Async(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool?>(), CancellationToken.None))
                .ReturnsAsync((UserRepresentation?)null);

            // Act
            var exception = Assert.ThrowsAsync<KeycloakApiException>(async () =>
                await client.SetStatusUserAsync(userId, true, CancellationToken.None));

            // Assert
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.InnerException, Is.TypeOf<KeycloakApiException>());
        }

        [Test(Description = "Если при изменение статуса пользователя, редактирование пользователя упало с ошибкой, то выбрасывается исключение.")]
        public void SetStatusUserAsync_UpdateUserThrowException_KeycloakApiException()
        {
            // Arrange
            var config = new KeycloakAuthorizationOptions();
            config.Authority = @"http://localhost";
            var options = new Mock<IOptions<KeycloakAuthorizationOptions>>();
            options.Setup(o => o.Value).Returns(config);
            var externalApiClient = new Mock<IKeycloakGeneratedExternalApiClient>();
            var httpClientFactory = new Mock<IHttpClientFactory>();
            var httpContextAccessor = new Mock<IHttpContextAccessor>();
            var logger = new Mock<ILogger<KeycloakUserApiExternalClient>>();
            var cache = new Mock<IDistributedCache>();

            var userId = Guid.Parse("cf80f652-0d56-4d7c-b315-38eab5c3b0bc");
            var client = new KeycloakUserApiExternalClient(options.Object, externalApiClient.Object,
                logger.Object, httpClientFactory.Object, httpContextAccessor.Object, cache.Object);

            externalApiClient.Setup(c => c.UsersGET2Async(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool?>(), CancellationToken.None))
                .ReturnsAsync(new UserRepresentation());
            externalApiClient.Setup(c => c.UsersPUTAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<UserRepresentation>(), CancellationToken.None))
                .ThrowsAsync(new NotImplementedException("Ошибка"));

            // Act
            var exception = Assert.ThrowsAsync<KeycloakApiException>(async () =>
                await client.SetStatusUserAsync(userId, true, CancellationToken.None));

            // Assert
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.InnerException, Is.TypeOf<NotImplementedException>());
        }

        [Test(Description = "Если при изменение статуса пользователя не было ошибок, то пользователь обновляется.")]
        public async Task SetStatusUserAsync_ValidData_SuccessUpdate()
        {
            // Arrange
            var config = new KeycloakAuthorizationOptions();
            config.Authority = @"http://localhost";
            var options = new Mock<IOptions<KeycloakAuthorizationOptions>>();
            options.Setup(o => o.Value).Returns(config);
            var externalApiClient = new Mock<IKeycloakGeneratedExternalApiClient>();
            var httpClientFactory = new Mock<IHttpClientFactory>();
            var httpContextAccessor = new Mock<IHttpContextAccessor>();
            var logger = new Mock<ILogger<KeycloakUserApiExternalClient>>();
            var cache = new Mock<IDistributedCache>();

            var userId = Guid.Parse("cf80f652-0d56-4d7c-b315-38eab5c3b0bc");
            var client = new KeycloakUserApiExternalClient(options.Object, externalApiClient.Object,
                logger.Object, httpClientFactory.Object, httpContextAccessor.Object, cache.Object);

            externalApiClient.Setup(c => c.UsersGET2Async(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool?>(), CancellationToken.None))
                .ReturnsAsync(new UserRepresentation());

            // Act
            await client.SetStatusUserAsync(userId, true, CancellationToken.None);

            // Assert
            externalApiClient.Verify(c => c.UsersPUTAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<UserRepresentation>(), CancellationToken.None), Times.Once);
        }

        [Test(Description = "Если при получении количества пользователей было выбрашено исключение, то оно оборачивается.")]
        public void GetUsersCountByFilterAsync_GetCountThrowException_KeycloakApiException()
        {
            // Arrange
            var config = new KeycloakAuthorizationOptions();
            config.Authority = @"http://localhost";
            var options = new Mock<IOptions<KeycloakAuthorizationOptions>>();
            options.Setup(o => o.Value).Returns(config);
            var externalApiClient = new Mock<IKeycloakGeneratedExternalApiClient>();
            var httpClientFactory = new Mock<IHttpClientFactory>();
            var httpContextAccessor = new Mock<IHttpContextAccessor>();
            var logger = new Mock<ILogger<KeycloakUserApiExternalClient>>();
            var cache = new Mock<IDistributedCache>();

            var client = new KeycloakUserApiExternalClient(options.Object, externalApiClient.Object,
                logger.Object, httpClientFactory.Object, httpContextAccessor.Object, cache.Object);

            externalApiClient.Setup(c => c.Count2Async(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool?>(), 
                    It.IsAny<bool?>(), It.IsAny<string?>(), It.IsAny<string?>(), It.IsAny<string?>(), It.IsAny<string?>(),
                    It.IsAny<string?>(), CancellationToken.None))
                .ThrowsAsync(new NotImplementedException());

            // Act
            var exception = Assert.ThrowsAsync<KeycloakApiException>(async () =>
                await client.GetUsersCountByFilterAsync(new UserFilter(), CancellationToken.None));

            // Assert
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.InnerException, Is.TypeOf<NotImplementedException>());
        }

        [Test(Description = "Если при получении количества пользователей ошибок не было, то возвращаются данные.")]
        public async Task GetUsersCountByFilterAsync_Return1_Return1()
        {
            // Arrange
            var config = new KeycloakAuthorizationOptions();
            config.Authority = @"http://localhost";
            var options = new Mock<IOptions<KeycloakAuthorizationOptions>>();
            options.Setup(o => o.Value).Returns(config);
            var externalApiClient = new Mock<IKeycloakGeneratedExternalApiClient>();
            var httpClientFactory = new Mock<IHttpClientFactory>();
            var httpContextAccessor = new Mock<IHttpContextAccessor>();
            var logger = new Mock<ILogger<KeycloakUserApiExternalClient>>();
            var cache = new Mock<IDistributedCache>();

            var userId = Guid.Parse("cf80f652-0d56-4d7c-b315-38eab5c3b0bc");
            var client = new KeycloakUserApiExternalClient(options.Object, externalApiClient.Object,
                logger.Object, httpClientFactory.Object, httpContextAccessor.Object, cache.Object);

            externalApiClient.Setup(c => c.Count2Async(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool?>(),
                    It.IsAny<bool?>(), It.IsAny<string?>(), It.IsAny<string?>(), It.IsAny<string?>(), It.IsAny<string?>(),
                    It.IsAny<string?>(), CancellationToken.None))
                .ReturnsAsync(1);

            // Act
            var result = await client.GetUsersCountByFilterAsync(new UserFilter(), CancellationToken.None);

            // Assert
            Assert.That(result, Is.EqualTo(1));
        }

        [Test(Description = "Если при получении пользователей было выбрашено исключение, то оно оборачивается.")]
        public void GetUsersByFilterAsync_GetUserThrowException_KeycloakApiException()
        {
            // Arrange
            var config = new KeycloakAuthorizationOptions();
            config.Authority = @"http://localhost";
            var options = new Mock<IOptions<KeycloakAuthorizationOptions>>();
            options.Setup(o => o.Value).Returns(config);
            var externalApiClient = new Mock<IKeycloakGeneratedExternalApiClient>();
            var httpClientFactory = new Mock<IHttpClientFactory>();
            var httpContextAccessor = new Mock<IHttpContextAccessor>();
            var logger = new Mock<ILogger<KeycloakUserApiExternalClient>>();
            var cache = new Mock<IDistributedCache>();

            var client = new KeycloakUserApiExternalClient(options.Object, externalApiClient.Object,
                logger.Object, httpClientFactory.Object, httpContextAccessor.Object, cache.Object);

            externalApiClient.Setup(c => c.UsersAll3Async(It.IsAny<string>(), It.IsAny<bool?>(), It.IsAny<string>(), It.IsAny<bool?>(),
                    It.IsAny<bool?>(), It.IsAny<bool?>(), It.IsAny<int?>(), It.IsAny<string?>(), It.IsAny<string?>(), It.IsAny<string?>(), 
                    It.IsAny<string?>(), It.IsAny<int?>(), It.IsAny<string?>(), It.IsAny<string?>(), It.IsAny<string?>(), CancellationToken.None))
                .ThrowsAsync(new NotImplementedException());

            // Act
            var exception = Assert.ThrowsAsync<KeycloakApiException>(async () =>
                await client.GetUsersByFilterAsync(new UserFilter(), CancellationToken.None));

            // Assert
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.InnerException, Is.TypeOf<NotImplementedException>());
        }

        [Test(Description = "Если при получении пользователей не удалось их найти, то возвращается пустой ответ.")]
        public async Task GetUsersByFilterAsync_UserListIsEmpty_Empty()
        {
            // Arrange
            var config = new KeycloakAuthorizationOptions();
            config.Authority = @"http://localhost";
            var options = new Mock<IOptions<KeycloakAuthorizationOptions>>();
            options.Setup(o => o.Value).Returns(config);
            var externalApiClient = new Mock<IKeycloakGeneratedExternalApiClient>();
            var httpClientFactory = new Mock<IHttpClientFactory>();
            var httpContextAccessor = new Mock<IHttpContextAccessor>();
            var logger = new Mock<ILogger<KeycloakUserApiExternalClient>>();
            var cache = new Mock<IDistributedCache>();

            var client = new KeycloakUserApiExternalClient(options.Object, externalApiClient.Object,
                logger.Object, httpClientFactory.Object, httpContextAccessor.Object, cache.Object);

            externalApiClient.Setup(c => c.UsersAll3Async(It.IsAny<string>(), It.IsAny<bool?>(), It.IsAny<string>(), It.IsAny<bool?>(),
                    It.IsAny<bool?>(), It.IsAny<bool?>(), It.IsAny<int?>(), It.IsAny<string?>(), It.IsAny<string?>(), It.IsAny<string?>(),
                    It.IsAny<string?>(), It.IsAny<int?>(), It.IsAny<string?>(), It.IsAny<string?>(), It.IsAny<string?>(), CancellationToken.None))
                .ReturnsAsync([]);

            // Act
            var result = await client.GetUsersByFilterAsync(new UserFilter(), CancellationToken.None);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Empty);
        }

        [Test(Description = "Если при получении ролей пользователей выбрасывается ошибка, то она оборачивается.")]
        public void GetUsersByFilterAsync_GetRolesThrowsException_KeycloakApiException()
        {
            // Arrange
            var config = new KeycloakAuthorizationOptions();
            config.Authority = @"http://localhost";
            var options = new Mock<IOptions<KeycloakAuthorizationOptions>>();
            options.Setup(o => o.Value).Returns(config);
            var externalApiClient = new Mock<IKeycloakGeneratedExternalApiClient>();
            var httpClientFactory = new Mock<IHttpClientFactory>();
            var httpContextAccessor = new Mock<IHttpContextAccessor>();
            var logger = new Mock<ILogger<KeycloakUserApiExternalClient>>();
            var cache = new Mock<IDistributedCache>();

            var client = new KeycloakUserApiExternalClient(options.Object, externalApiClient.Object,
                logger.Object, httpClientFactory.Object, httpContextAccessor.Object, cache.Object);

            externalApiClient.Setup(c => c.UsersAll3Async(It.IsAny<string>(), It.IsAny<bool?>(), It.IsAny<string>(), It.IsAny<bool?>(),
                    It.IsAny<bool?>(), It.IsAny<bool?>(), It.IsAny<int?>(), It.IsAny<string?>(), It.IsAny<string?>(), It.IsAny<string?>(),
                    It.IsAny<string?>(), It.IsAny<int?>(), It.IsAny<string?>(), It.IsAny<string?>(), It.IsAny<string?>(), CancellationToken.None))
                .ReturnsAsync([new UserRepresentation() { Id = "123" }]);
            externalApiClient.Setup(c => c.ClientsAll9Async(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new NotImplementedException("Ошибка"));

            // Act
            var exception = Assert.ThrowsAsync<KeycloakApiException>(async () =>
                await client.GetUsersByFilterAsync(new UserFilter(), CancellationToken.None));

            // Assert
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.InnerException, Is.TypeOf<NotImplementedException>());
        }

        [Test(Description = "Если при получении пользователей найден один пользователь, то возврщаются его данные.")]
        public async Task GetUsersByFilterAsync_OneUser_ReturnOneUser()
        {
            // Arrange
            var config = new KeycloakAuthorizationOptions();
            config.Authority = @"http://localhost";
            var options = new Mock<IOptions<KeycloakAuthorizationOptions>>();
            options.Setup(o => o.Value).Returns(config);
            var externalApiClient = new Mock<IKeycloakGeneratedExternalApiClient>();
            var httpClientFactory = new Mock<IHttpClientFactory>();
            var httpContextAccessor = new Mock<IHttpContextAccessor>();
            var logger = new Mock<ILogger<KeycloakUserApiExternalClient>>();
            var cache = new Mock<IDistributedCache>();

            var client = new KeycloakUserApiExternalClient(options.Object, externalApiClient.Object,
                logger.Object, httpClientFactory.Object, httpContextAccessor.Object, cache.Object);

            externalApiClient.Setup(c => c.UsersAll3Async(It.IsAny<string>(), It.IsAny<bool?>(), It.IsAny<string>(), It.IsAny<bool?>(),
                    It.IsAny<bool?>(), It.IsAny<bool?>(), It.IsAny<int?>(), It.IsAny<string?>(), It.IsAny<string?>(), It.IsAny<string?>(),
                    It.IsAny<string?>(), It.IsAny<int?>(), It.IsAny<string?>(), It.IsAny<string?>(), It.IsAny<string?>(), CancellationToken.None))
                .ReturnsAsync([new UserRepresentation() { Id = "cf80f652-0d56-4d7c-b315-38eab5c3b0bc" }]);
            externalApiClient.Setup(c => c.ClientsAll9Async(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync([]);

            // Act
            var result = await client.GetUsersByFilterAsync(new UserFilter(), CancellationToken.None);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Has.Count.EqualTo(1));
        }

        [Test(Description = "Если при получении ролей пользователя происходит ошибка, то исключение оборачивается.")]
        public void GetCachedUserRolesAsync_GetUserRoleeThrowException_KeycloakApiException()
        {
            // Arrange
            var config = new KeycloakAuthorizationOptions();
            config.Authority = @"http://localhost";
            var options = new Mock<IOptions<KeycloakAuthorizationOptions>>();
            options.Setup(o => o.Value).Returns(config);
            var externalApiClient = new Mock<IKeycloakGeneratedExternalApiClient>();
            var httpClientFactory = new Mock<IHttpClientFactory>();
            var httpContextAccessor = new Mock<IHttpContextAccessor>();
            var logger = new Mock<ILogger<KeycloakUserApiExternalClient>>();
            var cache = new Mock<IDistributedCache>();

            var userId = Guid.Parse("cf80f652-0d56-4d7c-b315-38eab5c3b0bc");
            var client = new KeycloakUserApiExternalClient(options.Object, externalApiClient.Object,
                logger.Object, httpClientFactory.Object, httpContextAccessor.Object, cache.Object);

            externalApiClient.Setup(c => c.ClientsAll9Async(It.IsAny<string>(), userId.ToString(), It.IsAny<string>(), CancellationToken.None))
                             .ThrowsAsync(new NotImplementedException());

            // Act
            var exception = Assert.ThrowsAsync<KeycloakApiException>(async () =>
                await client.GetCachedUserRolesAsync(userId, CancellationToken.None));

            // Assert
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.InnerException, Is.TypeOf<NotImplementedException>());
        }

        [Test(Description = "Если при получении роли не найдены, то результат пустой.")]
        public async Task GetCachedUserRolesAsync_RoleListIsEmpty_ResultIsEmpty()
        {
            // Arrange
            var config = new KeycloakAuthorizationOptions();
            config.Authority = @"http://localhost";
            var options = new Mock<IOptions<KeycloakAuthorizationOptions>>();
            options.Setup(o => o.Value).Returns(config);
            var externalApiClient = new Mock<IKeycloakGeneratedExternalApiClient>();
            var httpClientFactory = new Mock<IHttpClientFactory>();
            var httpContextAccessor = new Mock<IHttpContextAccessor>();
            var logger = new Mock<ILogger<KeycloakUserApiExternalClient>>();
            var cache = new Mock<IDistributedCache>();

            var userId = Guid.Parse("cf80f652-0d56-4d7c-b315-38eab5c3b0bc");
            var client = new KeycloakUserApiExternalClient(options.Object, externalApiClient.Object,
                logger.Object, httpClientFactory.Object, httpContextAccessor.Object, cache.Object);

            externalApiClient.Setup(c => c.ClientsAll9Async(It.IsAny<string>(), userId.ToString(), It.IsAny<string>(), CancellationToken.None))
                             .ReturnsAsync([]);

            // Act
            var result = await client.GetCachedUserRolesAsync(userId, CancellationToken.None);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Empty);
        }

        [Test(Description = "Если вызвать два раза метод, то из-за кэша, запрос в keycloak будет один.")]
        public async Task GetCachedUserRolesAsync_GetUserRoleeThrowException()
        {
            // Arrange
            var config = new KeycloakAuthorizationOptions();
            config.Authority = @"http://localhost";
            var options = new Mock<IOptions<KeycloakAuthorizationOptions>>();
            options.Setup(o => o.Value).Returns(config);
            var externalApiClient = new Mock<IKeycloakGeneratedExternalApiClient>();
            var httpClientFactory = new Mock<IHttpClientFactory>();
            var httpContextAccessor = new Mock<IHttpContextAccessor>();
            var logger = new Mock<ILogger<KeycloakUserApiExternalClient>>();
            var memoryOptions = Microsoft.Extensions.Options.Options.Create<MemoryDistributedCacheOptions>(new MemoryDistributedCacheOptions());
            var cache = new MemoryDistributedCache(memoryOptions);

            var userId = Guid.Parse("cf80f652-0d56-4d7c-b315-38eab5c3b0bc");
            var client = new KeycloakUserApiExternalClient(options.Object, externalApiClient.Object,
                logger.Object, httpClientFactory.Object, httpContextAccessor.Object, cache);

            externalApiClient.Setup(c => c.ClientsAll9Async(It.IsAny<string>(), userId.ToString(), It.IsAny<string>(), CancellationToken.None))
                             .ReturnsAsync([]);

            // Act
            await client.GetCachedUserRolesAsync(userId, CancellationToken.None);
            await client.GetCachedUserRolesAsync(userId, CancellationToken.None);

            // Assert
            externalApiClient.Verify(c => c.ClientsAll9Async(It.IsAny<string>(), userId.ToString(), It.IsAny<string>(), CancellationToken.None), Times.Once);
        }
    }
}
