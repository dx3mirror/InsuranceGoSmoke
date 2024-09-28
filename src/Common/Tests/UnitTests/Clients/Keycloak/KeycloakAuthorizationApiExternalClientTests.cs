using InsuranceGoSmoke.Common.Clients.Keycloak;
using InsuranceGoSmoke.Common.Clients.Keycloak.Generated;
using InsuranceGoSmoke.Common.Clients.Keycloak.Models;
using InsuranceGoSmoke.Common.Contracts.Contracts.Access.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using System.Threading;

namespace InsuranceGoSmoke.Common.Tests.UnitTests.Clients.Keycloak
{
    internal class KeycloakAuthorizationApiExternalClientTests
    {
        [Test(Description = "Если при попытке отправить смс падает ошибка 400, то должна возвращаться ошибка.")]
        public async Task TrySendSmsCodeAsync_BadRequest_ErrorResult()
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
            var factory = new Mock<IHttpClientFactory>();
            var logger = new Mock<ILogger<KeycloakAuthorizationApiExternalClient>>();
            var apiClient = new Mock<IKeycloakGeneratedExternalApiClient>();
            var client = new KeycloakAuthorizationApiExternalClient(options.Object, factory.Object, logger.Object, apiClient.Object);
            var phoneNumber = "123";
            factory.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(httpClient);

            // Act
            var result = await client.TrySendSmsAuthenticationCodeAsync(phoneNumber, CancellationToken.None);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.IsError, Is.True);
                Assert.That(result.ErrorDescription, Does.Contain(System.Net.HttpStatusCode.BadRequest.ToString()));
            });
        }

        [Test(Description = "Если при попытке отправить смс падает исключение, то должна возвращаться ошибка.")]
        public async Task TrySendSmsCodeAsync_Exception_ErrorResult()
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
            var factory = new Mock<IHttpClientFactory>();
            var logger = new Mock<ILogger<KeycloakAuthorizationApiExternalClient>>();
            var apiClient = new Mock<IKeycloakGeneratedExternalApiClient>();
            var client = new KeycloakAuthorizationApiExternalClient(options.Object, factory.Object, logger.Object, apiClient.Object);
            var phoneNumber = "123";
            factory.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(httpClient);

            // Act
            var result = await client.TrySendSmsAuthenticationCodeAsync(phoneNumber, CancellationToken.None);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.IsError, Is.True);
                Assert.That(result.ErrorDescription, Does.Contain("Ошибка"));
            });
        }

        [Test(Description = "Если при попытке отправить смс нет ошибок, то успешный ответ.")]
        public async Task TrySendSmsCodeAsync_OkRequest_SuccesResult()
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
            var factory = new Mock<IHttpClientFactory>();
            var logger = new Mock<ILogger<KeycloakAuthorizationApiExternalClient>>();
            var apiClient = new Mock<IKeycloakGeneratedExternalApiClient>();
            var client = new KeycloakAuthorizationApiExternalClient(options.Object, factory.Object, logger.Object, apiClient.Object);
            var phoneNumber = "123";
            factory.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(httpClient);

            // Act
            var result = await client.TrySendSmsAuthenticationCodeAsync(phoneNumber, CancellationToken.None);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.IsError, Is.False);
                Assert.That(result.ErrorDescription, Is.Empty);
            });
        }

        [Test(Description = "Если при попытке авторизации падает ошибка 400, то должна возвращаться ошибка.")]
        public void AuthorizationAsync_DiscoveryDocumentBadRequest_KeycloakApiException()
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
            httpClient.BaseAddress = new Uri(@"http://localhost");

            var config = new KeycloakAuthorizationOptions();
            config.Authority = @"http://localhost";
            var options = new Mock<IOptions<KeycloakAuthorizationOptions>>();
            options.Setup(o => o.Value).Returns(config);
            var factory = new Mock<IHttpClientFactory>();
            var logger = new Mock<ILogger<KeycloakAuthorizationApiExternalClient>>();
            var apiClient = new Mock<IKeycloakGeneratedExternalApiClient>();
            var client = new KeycloakAuthorizationApiExternalClient(options.Object, factory.Object, logger.Object, apiClient.Object);
            var username = "123";
            var code = "123";
            factory.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(httpClient);

            // Act
            var exception = Assert.ThrowsAsync<KeycloakApiException>(async () =>
                                await client.AuthorizationAsync(username, code, CancellationToken.None));

            // Assert
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.InnerException, Is.Not.Null);
            Assert.That(exception.InnerException.Message, Does.Contain("Не удалось получить данные о ссылках для авторизации"));
        }

        [Test(Description = "Если при попытке авторизации падает ошибка 401, то должна возвращаться ошибка.")]
        public void AuthorizationAsync_RequestPasswordTokenUnauthorized_KeycloakApiException()
        {
            // Arrange
            string json;
            using (StreamReader r = new("UnitTests\\Clients\\Keycloak\\openid-configuration.json"))
            {
                json = r.ReadToEnd();
            }

            var mockHttp = new Mock<HttpMessageHandler>();
            mockHttp.Protected()
                    .Setup<Task<HttpResponseMessage>>(
                        "SendAsync",
                        ItExpr.Is<HttpRequestMessage>(m => m.Method == HttpMethod.Get),
                        ItExpr.IsAny<CancellationToken>())
                    .ReturnsAsync(new HttpResponseMessage() { Content = new StringContent(json) });
            mockHttp.Protected()
                    .Setup<Task<HttpResponseMessage>>(
                        "SendAsync",
                        ItExpr.Is<HttpRequestMessage>(m => m.Method == HttpMethod.Post),
                        ItExpr.IsAny<CancellationToken>())
                    .ReturnsAsync(new HttpResponseMessage() { StatusCode = System.Net.HttpStatusCode.Unauthorized });
            var httpClient = new HttpClient(mockHttp.Object);
            httpClient.BaseAddress = new Uri("http://localhost:8090/realms/kortros");

            var config = new KeycloakAuthorizationOptions();
            config.Authority = "http://localhost:8090/realms/kortros";
            var options = new Mock<IOptions<KeycloakAuthorizationOptions>>();
            options.Setup(o => o.Value).Returns(config);
            var factory = new Mock<IHttpClientFactory>();
            var logger = new Mock<ILogger<KeycloakAuthorizationApiExternalClient>>();
            var apiClient = new Mock<IKeycloakGeneratedExternalApiClient>();
            var client = new KeycloakAuthorizationApiExternalClient(options.Object, factory.Object, logger.Object, apiClient.Object);
            var username = "123";
            var code = "123";
            factory.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(httpClient);

            // Act
            var exception = Assert.ThrowsAsync<KeycloakApiException>(async () =>
                                await client.AuthorizationAsync(username, code, CancellationToken.None));

            // Assert
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.InnerException, Is.Not.Null);
            Assert.That(exception.InnerException.Message, Does.Contain("Введен неверный логин или пароль."));
        }

        [Test(Description = "Если при попытке авторизации падает ошибка 400, то должна возвращаться ошибка.")]
        public void AuthorizationAsync_RequestPasswordTokenBadRequest_KeycloakApiException()
        {
            // Arrange
            string json;
            using (StreamReader r = new("UnitTests\\Clients\\Keycloak\\openid-configuration.json"))
            {
                json = r.ReadToEnd();
            }

            var mockHttp = new Mock<HttpMessageHandler>();
            mockHttp.Protected()
                    .Setup<Task<HttpResponseMessage>>(
                        "SendAsync",
                        ItExpr.Is<HttpRequestMessage>(m => m.Method == HttpMethod.Get),
                        ItExpr.IsAny<CancellationToken>())
                    .ReturnsAsync(new HttpResponseMessage() { Content = new StringContent(json) });
            mockHttp.Protected()
                    .Setup<Task<HttpResponseMessage>>(
                        "SendAsync",
                        ItExpr.Is<HttpRequestMessage>(m => m.Method == HttpMethod.Post),
                        ItExpr.IsAny<CancellationToken>())
                    .ReturnsAsync(new HttpResponseMessage() { StatusCode = System.Net.HttpStatusCode.BadRequest });
            var httpClient = new HttpClient(mockHttp.Object);
            httpClient.BaseAddress = new Uri("http://localhost:8090/realms/kortros");

            var config = new KeycloakAuthorizationOptions();
            config.Authority = "http://localhost:8090/realms/kortros";
            var options = new Mock<IOptions<KeycloakAuthorizationOptions>>();
            options.Setup(o => o.Value).Returns(config);
            var factory = new Mock<IHttpClientFactory>();
            var logger = new Mock<ILogger<KeycloakAuthorizationApiExternalClient>>();
            var apiClient = new Mock<IKeycloakGeneratedExternalApiClient>();
            var client = new KeycloakAuthorizationApiExternalClient(options.Object, factory.Object, logger.Object, apiClient.Object);
            var username = "123";
            var code = "123";
            factory.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(httpClient);

            // Act
            var exception = Assert.ThrowsAsync<KeycloakApiException>(async () =>
                                await client.AuthorizationAsync(username, code, CancellationToken.None));

            // Assert
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.InnerException, Is.Not.Null);
            Assert.That(exception.InnerException.Message, Does.Contain("Не удалось авторизовать пользователя: 123."));
        }

        [Test(Description = "Если при попытке авторизации нет ошибок, то должен возвращаться токен.")]
        public async Task AuthorizationAsync_ValidData_Token()
        {
            // Arrange
            string json;
            using (StreamReader r = new("UnitTests\\Clients\\Keycloak\\openid-configuration.json"))
            {
                json = r.ReadToEnd();
            }

            var mockHttp = new Mock<HttpMessageHandler>();
            mockHttp.Protected()
                    .Setup<Task<HttpResponseMessage>>(
                        "SendAsync",
                        ItExpr.Is<HttpRequestMessage>(m => m.Method == HttpMethod.Get),
                        ItExpr.IsAny<CancellationToken>())
                    .ReturnsAsync(new HttpResponseMessage() { Content = new StringContent(json) });
            mockHttp.Protected()
                    .Setup<Task<HttpResponseMessage>>(
                        "SendAsync",
                        ItExpr.Is<HttpRequestMessage>(m => m.Method == HttpMethod.Post),
                        ItExpr.IsAny<CancellationToken>())
                    .ReturnsAsync(new HttpResponseMessage() { StatusCode = System.Net.HttpStatusCode.OK, Content = new StringContent("{ \"access_token\": \"123\" }") });
            var httpClient = new HttpClient(mockHttp.Object);
            httpClient.BaseAddress = new Uri("http://localhost:8090/realms/kortros");

            var config = new KeycloakAuthorizationOptions();
            config.Authority = "http://localhost:8090/realms/kortros";
            var options = new Mock<IOptions<KeycloakAuthorizationOptions>>();
            options.Setup(o => o.Value).Returns(config);
            var factory = new Mock<IHttpClientFactory>();
            var logger = new Mock<ILogger<KeycloakAuthorizationApiExternalClient>>();
            var apiClient = new Mock<IKeycloakGeneratedExternalApiClient>();
            var client = new KeycloakAuthorizationApiExternalClient(options.Object, factory.Object, logger.Object, apiClient.Object);
            var username = "123";
            var code = "123";
            factory.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(httpClient);
            apiClient.Setup(c => c.UsersAll3Async(It.IsAny<string>(), It.IsAny<bool?>(), It.IsAny<string?>(), It.IsAny<bool?>(),
                                /* enabled */ true, It.IsAny<bool?>(), It.IsAny<int?>(), It.IsAny<string?>(), It.IsAny<string?>(), It.IsAny<string?>(),
                                It.IsAny<string?>(), /* max */ 1, It.IsAny<string?>(), It.IsAny<string?>(), username, CancellationToken.None))
                        .ReturnsAsync([]);

            // Act
            var result = await client.AuthorizationAsync(username, code, CancellationToken.None);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.AccessToken, Is.EqualTo("123"));
            apiClient.Verify(c => c.ClientsPOST6Async(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), 
                It.IsAny<IEnumerable<RoleRepresentation>?>(), CancellationToken.None), Times.Never);
        }

        [Test(Description = "Если при попытке авторизации у пользователя есть роли, роли не устанавливаются.")]
        public async Task AuthorizationAsync_UserRoleFound_NoSetDefaultRole()
        {
            // Arrange
            string json;
            using (StreamReader r = new("UnitTests\\Clients\\Keycloak\\openid-configuration.json"))
            {
                json = r.ReadToEnd();
            }

            var mockHttp = new Mock<HttpMessageHandler>();
            mockHttp.Protected()
                    .Setup<Task<HttpResponseMessage>>(
                        "SendAsync",
                        ItExpr.Is<HttpRequestMessage>(m => m.Method == HttpMethod.Get),
                        ItExpr.IsAny<CancellationToken>())
                    .ReturnsAsync(new HttpResponseMessage() { Content = new StringContent(json) });
            mockHttp.Protected()
                    .Setup<Task<HttpResponseMessage>>(
                        "SendAsync",
                        ItExpr.Is<HttpRequestMessage>(m => m.Method == HttpMethod.Post),
                        ItExpr.IsAny<CancellationToken>())
                    .ReturnsAsync(new HttpResponseMessage() { StatusCode = System.Net.HttpStatusCode.OK, Content = new StringContent("{ \"access_token\": \"123\" }") });
            var httpClient = new HttpClient(mockHttp.Object);
            httpClient.BaseAddress = new Uri("http://localhost:8090/realms/kortros");

            var config = new KeycloakAuthorizationOptions();
            config.Authority = "http://localhost:8090/realms/kortros";
            var options = new Mock<IOptions<KeycloakAuthorizationOptions>>();
            options.Setup(o => o.Value).Returns(config);
            var factory = new Mock<IHttpClientFactory>();
            var logger = new Mock<ILogger<KeycloakAuthorizationApiExternalClient>>();
            var apiClient = new Mock<IKeycloakGeneratedExternalApiClient>();
            var client = new KeycloakAuthorizationApiExternalClient(options.Object, factory.Object, logger.Object, apiClient.Object);
            var username = "123";
            var code = "123";
            var userId = "1234";
            factory.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(httpClient);
            apiClient.Setup(c => c.UsersAll3Async(It.IsAny<string>(), It.IsAny<bool?>(), It.IsAny<string?>(), It.IsAny<bool?>(),
                                /* enabled */ true, It.IsAny<bool?>(), It.IsAny<int?>(), It.IsAny<string?>(), It.IsAny<string?>(), It.IsAny<string?>(),
                                It.IsAny<string?>(), /* max */ 1, It.IsAny<string?>(), It.IsAny<string?>(), username, CancellationToken.None))
                     .ReturnsAsync([new UserRepresentation { Id = userId }]);
            apiClient.Setup(c => c.ClientsAll9Async(It.IsAny<string>(), userId, It.IsAny<string>(), CancellationToken.None))
                     .ReturnsAsync([new RoleRepresentation()]);

            // Act
            var result = await client.AuthorizationAsync(username, code, CancellationToken.None);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.AccessToken, Is.EqualTo("123"));
            apiClient.Verify(c => c.ClientsPOST6Async(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<IEnumerable<RoleRepresentation>?>(), CancellationToken.None), Times.Never);
        }

        [Test(Description = "Если при попытке авторизации нет у пользователя нет ролей и не найдена default роль, роли не устанавливаются и ошибки нет.")]
        public async Task AuthorizationAsync_RoleNotFoundAndDefaultRoleNotFound_NoSetDefaultRole()
        {
            // Arrange
            string json;
            using (StreamReader r = new("UnitTests\\Clients\\Keycloak\\openid-configuration.json"))
            {
                json = r.ReadToEnd();
            }

            var mockHttp = new Mock<HttpMessageHandler>();
            mockHttp.Protected()
                    .Setup<Task<HttpResponseMessage>>(
                        "SendAsync",
                        ItExpr.Is<HttpRequestMessage>(m => m.Method == HttpMethod.Get),
                        ItExpr.IsAny<CancellationToken>())
                    .ReturnsAsync(new HttpResponseMessage() { Content = new StringContent(json) });
            mockHttp.Protected()
                    .Setup<Task<HttpResponseMessage>>(
                        "SendAsync",
                        ItExpr.Is<HttpRequestMessage>(m => m.Method == HttpMethod.Post),
                        ItExpr.IsAny<CancellationToken>())
                    .ReturnsAsync(new HttpResponseMessage() { StatusCode = System.Net.HttpStatusCode.OK, Content = new StringContent("{ \"access_token\": \"123\" }") });
            var httpClient = new HttpClient(mockHttp.Object);
            httpClient.BaseAddress = new Uri("http://localhost:8090/realms/kortros");

            var config = new KeycloakAuthorizationOptions();
            config.Authority = "http://localhost:8090/realms/kortros";
            var options = new Mock<IOptions<KeycloakAuthorizationOptions>>();
            options.Setup(o => o.Value).Returns(config);
            var factory = new Mock<IHttpClientFactory>();
            var logger = new Mock<ILogger<KeycloakAuthorizationApiExternalClient>>();
            var apiClient = new Mock<IKeycloakGeneratedExternalApiClient>();
            var client = new KeycloakAuthorizationApiExternalClient(options.Object, factory.Object, logger.Object, apiClient.Object);
            var username = "123";
            var code = "123";
            var userId = "1234";
            factory.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(httpClient);
            apiClient.Setup(c => c.UsersAll3Async(It.IsAny<string>(), It.IsAny<bool?>(), It.IsAny<string?>(), It.IsAny<bool?>(),
                                /* enabled */ true, It.IsAny<bool?>(), It.IsAny<int?>(), It.IsAny<string?>(), It.IsAny<string?>(), It.IsAny<string?>(),
                                It.IsAny<string?>(), /* max */ 1, It.IsAny<string?>(), It.IsAny<string?>(), username, CancellationToken.None))
                     .ReturnsAsync([new UserRepresentation { Id = userId }]);
            apiClient.Setup(c => c.ClientsAll9Async(It.IsAny<string>(), userId, It.IsAny<string>(), CancellationToken.None))
                     .ReturnsAsync([]);
            apiClient.Setup(c => c.Available9Async(It.IsAny<string>(), userId, It.IsAny<string>(), CancellationToken.None))
                     .ReturnsAsync([]);

            // Act
            var result = await client.AuthorizationAsync(username, code, CancellationToken.None);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.AccessToken, Is.EqualTo("123"));
            apiClient.Verify(c => c.ClientsPOST6Async(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<IEnumerable<RoleRepresentation>?>(), CancellationToken.None), Times.Never);
        }

        [Test(Description = "Если при попытке авторизации нет у пользователя нет ролей, должна добавиться роль Клиента.")]
        public async Task AuthorizationAsync_RoleNotFound_SetDefaultRole()
        {
            // Arrange
            string json;
            using (StreamReader r = new("UnitTests\\Clients\\Keycloak\\openid-configuration.json"))
            {
                json = r.ReadToEnd();
            }

            var mockHttp = new Mock<HttpMessageHandler>();
            mockHttp.Protected()
                    .Setup<Task<HttpResponseMessage>>(
                        "SendAsync",
                        ItExpr.Is<HttpRequestMessage>(m => m.Method == HttpMethod.Get),
                        ItExpr.IsAny<CancellationToken>())
                    .ReturnsAsync(new HttpResponseMessage() { Content = new StringContent(json) });
            mockHttp.Protected()
                    .Setup<Task<HttpResponseMessage>>(
                        "SendAsync",
                        ItExpr.Is<HttpRequestMessage>(m => m.Method == HttpMethod.Post),
                        ItExpr.IsAny<CancellationToken>())
                    .ReturnsAsync(new HttpResponseMessage() { StatusCode = System.Net.HttpStatusCode.OK, Content = new StringContent("{ \"access_token\": \"123\" }") });
            var httpClient = new HttpClient(mockHttp.Object);
            httpClient.BaseAddress = new Uri("http://localhost:8090/realms/kortros");

            var config = new KeycloakAuthorizationOptions();
            config.Authority = "http://localhost:8090/realms/kortros";
            var options = new Mock<IOptions<KeycloakAuthorizationOptions>>();
            options.Setup(o => o.Value).Returns(config);
            var factory = new Mock<IHttpClientFactory>();
            var logger = new Mock<ILogger<KeycloakAuthorizationApiExternalClient>>();
            var apiClient = new Mock<IKeycloakGeneratedExternalApiClient>();
            var client = new KeycloakAuthorizationApiExternalClient(options.Object, factory.Object, logger.Object, apiClient.Object);
            var username = "123";
            var code = "123";
            var userId = "1234";
            factory.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(httpClient);
            apiClient.Setup(c => c.UsersAll3Async(It.IsAny<string>(), It.IsAny<bool?>(), It.IsAny<string?>(), It.IsAny<bool?>(),
                                /* enabled */ true, It.IsAny<bool?>(), It.IsAny<int?>(), It.IsAny<string?>(), It.IsAny<string?>(), It.IsAny<string?>(),
                                It.IsAny<string?>(), /* max */ 1, It.IsAny<string?>(), It.IsAny<string?>(), username, CancellationToken.None))
                     .ReturnsAsync([new UserRepresentation { Id = userId }]);
            apiClient.Setup(c => c.ClientsAll9Async(It.IsAny<string>(), userId, It.IsAny<string>(), CancellationToken.None))
                     .ReturnsAsync([]);
            var role = new RoleRepresentation() { Name = RoleTypes.Client.ToString() };
            var roles = new[] { role };
            apiClient.Setup(c => c.Available9Async(It.IsAny<string>(), userId, It.IsAny<string>(), CancellationToken.None))
                     .ReturnsAsync(roles);

            // Act
            var result = await client.AuthorizationAsync(username, code, CancellationToken.None);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.AccessToken, Is.EqualTo("123"));
            apiClient.Verify(c => c.ClientsPOST6Async(It.IsAny<string>(), userId, It.IsAny<string>(), roles, CancellationToken.None), Times.Once);
        }

        [Test(Description = "Если при попытке авторизации по телефону падает ошибка 400, то должна возвращаться ошибка.")]
        public void AuthorizationByPhoneAsync_DiscoveryDocumentBadRequest_KeycloakApiException()
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
            httpClient.BaseAddress = new Uri(@"http://localhost");

            var config = new KeycloakAuthorizationOptions();
            config.Authority = @"http://localhost";
            var options = new Mock<IOptions<KeycloakAuthorizationOptions>>();
            options.Setup(o => o.Value).Returns(config);
            var factory = new Mock<IHttpClientFactory>();
            var logger = new Mock<ILogger<KeycloakAuthorizationApiExternalClient>>();
            var apiClient = new Mock<IKeycloakGeneratedExternalApiClient>();
            var client = new KeycloakAuthorizationApiExternalClient(options.Object, factory.Object, logger.Object, apiClient.Object);
            var phoneNumber = "123";
            var code = "123";
            factory.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(httpClient);

            // Act
            var exception = Assert.ThrowsAsync<KeycloakApiException>(async () =>
                                await client.AuthorizationByPhoneAsync(phoneNumber, code, CancellationToken.None));

            // Assert
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.InnerException, Is.Not.Null);
            Assert.That(exception.InnerException.Message, Does.Contain("Не удалось получить данные о ссылках для авторизации"));
        }

        [Test(Description = "Если при попытке авторизации по телефону падает ошибка 401, то должна возвращаться ошибка.")]
        public void AuthorizationByPhoneAsync_RequestTokenUnauthorized_KeycloakApiException()
        {
            // Arrange
            string json;
            using (StreamReader r = new("UnitTests\\Clients\\Keycloak\\openid-configuration.json"))
            {
                json = r.ReadToEnd();
            }

            var mockHttp = new Mock<HttpMessageHandler>();
            mockHttp.Protected()
                    .Setup<Task<HttpResponseMessage>>(
                        "SendAsync",
                        ItExpr.Is<HttpRequestMessage>(m => m.Method == HttpMethod.Get),
                        ItExpr.IsAny<CancellationToken>())
                    .ReturnsAsync(new HttpResponseMessage() { Content = new StringContent(json) });
            mockHttp.Protected()
                    .Setup<Task<HttpResponseMessage>>(
                        "SendAsync",
                        ItExpr.Is<HttpRequestMessage>(m => m.Method == HttpMethod.Post),
                        ItExpr.IsAny<CancellationToken>())
                    .ReturnsAsync(new HttpResponseMessage() { StatusCode = System.Net.HttpStatusCode.Unauthorized });
            var httpClient = new HttpClient(mockHttp.Object);
            httpClient.BaseAddress = new Uri("http://localhost:8090/realms/kortros");

            var config = new KeycloakAuthorizationOptions();
            config.Authority = "http://localhost:8090/realms/kortros";
            var options = new Mock<IOptions<KeycloakAuthorizationOptions>>();
            options.Setup(o => o.Value).Returns(config);
            var factory = new Mock<IHttpClientFactory>();
            var logger = new Mock<ILogger<KeycloakAuthorizationApiExternalClient>>();
            var apiClient = new Mock<IKeycloakGeneratedExternalApiClient>();
            var client = new KeycloakAuthorizationApiExternalClient(options.Object, factory.Object, logger.Object, apiClient.Object);
            var phoneNumber = "123";
            var code = "123";
            factory.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(httpClient);

            // Act
            var exception = Assert.ThrowsAsync<KeycloakApiException>(async () =>
                                await client.AuthorizationByPhoneAsync(phoneNumber, code, CancellationToken.None));

            // Assert
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Does.Contain("При авторизации по телефону 123 произошла ошибка."));
        }

        [Test(Description = "Если при попытке авторизации по телефону падает ошибка 400, то должна возвращаться ошибка.")]
        public void AuthorizationByPhoneAsync_RequestTokenBadRequest_KeycloakApiException()
        {
            // Arrange
            string json;
            using (StreamReader r = new("UnitTests\\Clients\\Keycloak\\openid-configuration.json"))
            {
                json = r.ReadToEnd();
            }

            var mockHttp = new Mock<HttpMessageHandler>();
            mockHttp.Protected()
                    .Setup<Task<HttpResponseMessage>>(
                        "SendAsync",
                        ItExpr.Is<HttpRequestMessage>(m => m.Method == HttpMethod.Get),
                        ItExpr.IsAny<CancellationToken>())
                    .ReturnsAsync(new HttpResponseMessage() { Content = new StringContent(json) });
            mockHttp.Protected()
                    .Setup<Task<HttpResponseMessage>>(
                        "SendAsync",
                        ItExpr.Is<HttpRequestMessage>(m => m.Method == HttpMethod.Post),
                        ItExpr.IsAny<CancellationToken>())
                    .ReturnsAsync(new HttpResponseMessage() { StatusCode = System.Net.HttpStatusCode.BadRequest });
            var httpClient = new HttpClient(mockHttp.Object);
            httpClient.BaseAddress = new Uri("http://localhost:8090/realms/kortros");

            var config = new KeycloakAuthorizationOptions();
            config.Authority = "http://localhost:8090/realms/kortros";
            var options = new Mock<IOptions<KeycloakAuthorizationOptions>>();
            options.Setup(o => o.Value).Returns(config);
            var factory = new Mock<IHttpClientFactory>();
            var logger = new Mock<ILogger<KeycloakAuthorizationApiExternalClient>>();
            var apiClient = new Mock<IKeycloakGeneratedExternalApiClient>();
            var client = new KeycloakAuthorizationApiExternalClient(options.Object, factory.Object, logger.Object, apiClient.Object);
            var phoneNumber = "123";
            var code = "123";
            factory.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(httpClient);

            // Act
            var exception = Assert.ThrowsAsync<KeycloakApiException>(async () =>
                                await client.AuthorizationByPhoneAsync(phoneNumber, code, CancellationToken.None));

            // Assert
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Does.Contain("При авторизации по телефону 123 произошла ошибка."));
        }

        [Test(Description = "Если при попытке авторизации по телефону нет ошибок, то должен возвращаться токен.")]
        public async Task AuthorizationByPhoneAsync_ValidData_Token()
        {
            // Arrange
            string json;
            using (StreamReader r = new("UnitTests\\Clients\\Keycloak\\openid-configuration.json"))
            {
                json = r.ReadToEnd();
            }

            var mockHttp = new Mock<HttpMessageHandler>();
            mockHttp.Protected()
                    .Setup<Task<HttpResponseMessage>>(
                        "SendAsync",
                        ItExpr.Is<HttpRequestMessage>(m => m.Method == HttpMethod.Get),
                        ItExpr.IsAny<CancellationToken>())
                    .ReturnsAsync(new HttpResponseMessage() { Content = new StringContent(json) });
            mockHttp.Protected()
                    .Setup<Task<HttpResponseMessage>>(
                        "SendAsync",
                        ItExpr.Is<HttpRequestMessage>(m => m.Method == HttpMethod.Post),
                        ItExpr.IsAny<CancellationToken>())
                    .ReturnsAsync(new HttpResponseMessage() { StatusCode = System.Net.HttpStatusCode.OK, Content = new StringContent("{ \"access_token\": \"123\" }") });
            var httpClient = new HttpClient(mockHttp.Object);
            httpClient.BaseAddress = new Uri("http://localhost:8090/realms/kortros");

            var config = new KeycloakAuthorizationOptions();
            config.Authority = "http://localhost:8090/realms/kortros";
            var options = new Mock<IOptions<KeycloakAuthorizationOptions>>();
            options.Setup(o => o.Value).Returns(config);
            var factory = new Mock<IHttpClientFactory>();
            var logger = new Mock<ILogger<KeycloakAuthorizationApiExternalClient>>();
            var apiClient = new Mock<IKeycloakGeneratedExternalApiClient>();
            var client = new KeycloakAuthorizationApiExternalClient(options.Object, factory.Object, logger.Object, apiClient.Object);
            var phoneNumber = "123";
            var code = "123";
            factory.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(httpClient);
            apiClient.Setup(c => c.UsersAll3Async(It.IsAny<string>(), It.IsAny<bool?>(), It.IsAny<string?>(), It.IsAny<bool?>(),
                                /* enabled */ true, It.IsAny<bool?>(), It.IsAny<int?>(), It.IsAny<string?>(), It.IsAny<string?>(), It.IsAny<string?>(),
                                It.IsAny<string?>(), /* max */ 1, It.IsAny<string?>(), It.IsAny<string?>(), phoneNumber, CancellationToken.None))
                        .ReturnsAsync([]);

            // Act
            var result = await client.AuthorizationByPhoneAsync(phoneNumber, code, CancellationToken.None);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.AccessToken, Is.EqualTo("123"));
            apiClient.Verify(c => c.ClientsPOST6Async(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<IEnumerable<RoleRepresentation>?>(), CancellationToken.None), Times.Never);
        }

        [Test(Description = "Если при попытке авторизации по телефону у пользователя есть роли, роли не устанавливаются.")]
        public async Task AuthorizationByPhoneAsync_UserRoleFound_NoSetDefaultRole()
        {
            // Arrange
            string json;
            using (StreamReader r = new("UnitTests\\Clients\\Keycloak\\openid-configuration.json"))
            {
                json = r.ReadToEnd();
            }

            var mockHttp = new Mock<HttpMessageHandler>();
            mockHttp.Protected()
                    .Setup<Task<HttpResponseMessage>>(
                        "SendAsync",
                        ItExpr.Is<HttpRequestMessage>(m => m.Method == HttpMethod.Get),
                        ItExpr.IsAny<CancellationToken>())
                    .ReturnsAsync(new HttpResponseMessage() { Content = new StringContent(json) });
            mockHttp.Protected()
                    .Setup<Task<HttpResponseMessage>>(
                        "SendAsync",
                        ItExpr.Is<HttpRequestMessage>(m => m.Method == HttpMethod.Post),
                        ItExpr.IsAny<CancellationToken>())
                    .ReturnsAsync(new HttpResponseMessage() { StatusCode = System.Net.HttpStatusCode.OK, Content = new StringContent("{ \"access_token\": \"123\" }") });
            var httpClient = new HttpClient(mockHttp.Object);
            httpClient.BaseAddress = new Uri("http://localhost:8090/realms/kortros");

            var config = new KeycloakAuthorizationOptions();
            config.Authority = "http://localhost:8090/realms/kortros";
            var options = new Mock<IOptions<KeycloakAuthorizationOptions>>();
            options.Setup(o => o.Value).Returns(config);
            var factory = new Mock<IHttpClientFactory>();
            var logger = new Mock<ILogger<KeycloakAuthorizationApiExternalClient>>();
            var apiClient = new Mock<IKeycloakGeneratedExternalApiClient>();
            var client = new KeycloakAuthorizationApiExternalClient(options.Object, factory.Object, logger.Object, apiClient.Object);
            var phoneNumber = "123";
            var code = "123";
            var userId = "1234";
            factory.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(httpClient);
            apiClient.Setup(c => c.UsersAll3Async(It.IsAny<string>(), It.IsAny<bool?>(), It.IsAny<string?>(), It.IsAny<bool?>(),
                                /* enabled */ true, It.IsAny<bool?>(), It.IsAny<int?>(), It.IsAny<string?>(), It.IsAny<string?>(), It.IsAny<string?>(),
                                It.IsAny<string?>(), /* max */ 1, It.IsAny<string?>(), It.IsAny<string?>(), phoneNumber, CancellationToken.None))
                     .ReturnsAsync([new UserRepresentation { Id = userId }]);
            apiClient.Setup(c => c.ClientsAll9Async(It.IsAny<string>(), userId, It.IsAny<string>(), CancellationToken.None))
                     .ReturnsAsync([new RoleRepresentation()]);

            // Act
            var result = await client.AuthorizationByPhoneAsync(phoneNumber, code, CancellationToken.None);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.AccessToken, Is.EqualTo("123"));
            apiClient.Verify(c => c.ClientsPOST6Async(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<IEnumerable<RoleRepresentation>?>(), CancellationToken.None), Times.Never);
        }

        [Test(Description = "Если при попытке авторизации по телефону нет у пользователя нет ролей и не найдена default роль, роли не устанавливаются и ошибки нет.")]
        public async Task AuthorizationByPhoneAsync_RoleNotFoundAndDefaultRoleNotFound_NoSetDefaultRole()
        {
            // Arrange
            string json;
            using (StreamReader r = new("UnitTests\\Clients\\Keycloak\\openid-configuration.json"))
            {
                json = r.ReadToEnd();
            }

            var mockHttp = new Mock<HttpMessageHandler>();
            mockHttp.Protected()
                    .Setup<Task<HttpResponseMessage>>(
                        "SendAsync",
                        ItExpr.Is<HttpRequestMessage>(m => m.Method == HttpMethod.Get),
                        ItExpr.IsAny<CancellationToken>())
                    .ReturnsAsync(new HttpResponseMessage() { Content = new StringContent(json) });
            mockHttp.Protected()
                    .Setup<Task<HttpResponseMessage>>(
                        "SendAsync",
                        ItExpr.Is<HttpRequestMessage>(m => m.Method == HttpMethod.Post),
                        ItExpr.IsAny<CancellationToken>())
                    .ReturnsAsync(new HttpResponseMessage() { StatusCode = System.Net.HttpStatusCode.OK, Content = new StringContent("{ \"access_token\": \"123\" }") });
            var httpClient = new HttpClient(mockHttp.Object);
            httpClient.BaseAddress = new Uri("http://localhost:8090/realms/kortros");

            var config = new KeycloakAuthorizationOptions();
            config.Authority = "http://localhost:8090/realms/kortros";
            var options = new Mock<IOptions<KeycloakAuthorizationOptions>>();
            options.Setup(o => o.Value).Returns(config);
            var factory = new Mock<IHttpClientFactory>();
            var logger = new Mock<ILogger<KeycloakAuthorizationApiExternalClient>>();
            var apiClient = new Mock<IKeycloakGeneratedExternalApiClient>();
            var client = new KeycloakAuthorizationApiExternalClient(options.Object, factory.Object, logger.Object, apiClient.Object);
            var phoneNumber = "123";
            var code = "123";
            var userId = "1234";
            factory.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(httpClient);
            apiClient.Setup(c => c.UsersAll3Async(It.IsAny<string>(), It.IsAny<bool?>(), It.IsAny<string?>(), It.IsAny<bool?>(),
                                /* enabled */ true, It.IsAny<bool?>(), It.IsAny<int?>(), It.IsAny<string?>(), It.IsAny<string?>(), It.IsAny<string?>(),
                                It.IsAny<string?>(), /* max */ 1, It.IsAny<string?>(), It.IsAny<string?>(), phoneNumber, CancellationToken.None))
                     .ReturnsAsync([new UserRepresentation { Id = userId }]);
            apiClient.Setup(c => c.ClientsAll9Async(It.IsAny<string>(), userId, It.IsAny<string>(), CancellationToken.None))
                     .ReturnsAsync([]);
            apiClient.Setup(c => c.Available9Async(It.IsAny<string>(), userId, It.IsAny<string>(), CancellationToken.None))
                     .ReturnsAsync([]);

            // Act
            var result = await client.AuthorizationByPhoneAsync(phoneNumber, code, CancellationToken.None);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.AccessToken, Is.EqualTo("123"));
            apiClient.Verify(c => c.ClientsPOST6Async(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<IEnumerable<RoleRepresentation>?>(), CancellationToken.None), Times.Never);
        }

        [Test(Description = "Если при попытке авторизации по телефону нет у пользователя нет ролей, должна добавиться роль Клиента.")]
        public async Task AuthorizationByPhoneAsync_RoleNotFound_SetDefaultRole()
        {
            // Arrange
            string json;
            using (StreamReader r = new("UnitTests\\Clients\\Keycloak\\openid-configuration.json"))
            {
                json = r.ReadToEnd();
            }

            var mockHttp = new Mock<HttpMessageHandler>();
            mockHttp.Protected()
                    .Setup<Task<HttpResponseMessage>>(
                        "SendAsync",
                        ItExpr.Is<HttpRequestMessage>(m => m.Method == HttpMethod.Get),
                        ItExpr.IsAny<CancellationToken>())
                    .ReturnsAsync(new HttpResponseMessage() { Content = new StringContent(json) });
            mockHttp.Protected()
                    .Setup<Task<HttpResponseMessage>>(
                        "SendAsync",
                        ItExpr.Is<HttpRequestMessage>(m => m.Method == HttpMethod.Post),
                        ItExpr.IsAny<CancellationToken>())
                    .ReturnsAsync(new HttpResponseMessage() { StatusCode = System.Net.HttpStatusCode.OK, Content = new StringContent("{ \"access_token\": \"123\" }") });
            var httpClient = new HttpClient(mockHttp.Object);
            httpClient.BaseAddress = new Uri("http://localhost:8090/realms/kortros");

            var config = new KeycloakAuthorizationOptions();
            config.Authority = "http://localhost:8090/realms/kortros";
            var options = new Mock<IOptions<KeycloakAuthorizationOptions>>();
            options.Setup(o => o.Value).Returns(config);
            var factory = new Mock<IHttpClientFactory>();
            var logger = new Mock<ILogger<KeycloakAuthorizationApiExternalClient>>();
            var apiClient = new Mock<IKeycloakGeneratedExternalApiClient>();
            var client = new KeycloakAuthorizationApiExternalClient(options.Object, factory.Object, logger.Object, apiClient.Object);
            var phoneNumber = "123";
            var code = "123";
            var userId = "1234";
            factory.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(httpClient);
            apiClient.Setup(c => c.UsersAll3Async(It.IsAny<string>(), It.IsAny<bool?>(), It.IsAny<string?>(), It.IsAny<bool?>(),
                                /* enabled */ true, It.IsAny<bool?>(), It.IsAny<int?>(), It.IsAny<string?>(), It.IsAny<string?>(), It.IsAny<string?>(),
                                It.IsAny<string?>(), /* max */ 1, It.IsAny<string?>(), It.IsAny<string?>(), phoneNumber, CancellationToken.None))
                     .ReturnsAsync([new UserRepresentation { Id = userId }]);
            apiClient.Setup(c => c.ClientsAll9Async(It.IsAny<string>(), userId, It.IsAny<string>(), CancellationToken.None))
                     .ReturnsAsync([]);
            var role = new RoleRepresentation() { Name = RoleTypes.Client.ToString() };
            var roles = new[] { role };
            apiClient.Setup(c => c.Available9Async(It.IsAny<string>(), userId, It.IsAny<string>(), CancellationToken.None))
                     .ReturnsAsync(roles);

            // Act
            var result = await client.AuthorizationByPhoneAsync(phoneNumber, code, CancellationToken.None);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.AccessToken, Is.EqualTo("123"));
            apiClient.Verify(c => c.ClientsPOST6Async(It.IsAny<string>(), userId, It.IsAny<string>(), roles, CancellationToken.None), Times.Once);
        }
    }
}
