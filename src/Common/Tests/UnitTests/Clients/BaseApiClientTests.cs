using InsuranceGoSmoke.Common.Clients;
using Moq.Protected;
using Moq;
using InsuranceGoSmoke.Common.Contracts.Exceptions.Common;

namespace InsuranceGoSmoke.Common.Tests.UnitTests.Clients
{
    internal class BaseApiClientTests
    {
        [Test(Description = "Если при запросе на удаление ошибка 400, то выбрасывается исключение.")]
        public void DeleteAsync_BadRequest_ReadableException()
        {
            // Arrange
            var mockHttp = new Mock<HttpMessageHandler>();
            mockHttp.Protected()
                    .Setup<Task<HttpResponseMessage>>(
                        "SendAsync",
                        ItExpr.Is<HttpRequestMessage>(m => m.Method == HttpMethod.Delete),
                        ItExpr.IsAny<CancellationToken>())
                    .ReturnsAsync(new HttpResponseMessage() { StatusCode = System.Net.HttpStatusCode.BadRequest });
            var httpClient = new HttpClient(mockHttp.Object);
            var client = new TestApiClient(httpClient);
            var url = @"http:\\localhost";

            // Act
            var exception = Assert.ThrowsAsync<ReadableException>(async () => 
                                await client.TestDeleteAsync(url, new { }, CancellationToken.None));

            // Assert
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.InnerException, Is.Not.Null);
            Assert.That(exception.InnerException, Is.TypeOf<ReadableException>());
        }

        [Test(Description = "Если при запросе на удаление ответ URL невалидный, то выбрасывается исключение.")]
        public void DeleteAsync_UrlIsInvalid_InvalidOperationException()
        {
            // Arrange
            var mockHttp = new Mock<HttpMessageHandler>();
            mockHttp.Protected()
                    .Setup<Task<HttpResponseMessage>>(
                        "SendAsync",
                        ItExpr.Is<HttpRequestMessage>(m => m.Method == HttpMethod.Delete),
                        ItExpr.IsAny<CancellationToken>())
                    .ReturnsAsync(new HttpResponseMessage() { StatusCode = System.Net.HttpStatusCode.BadRequest });
            var httpClient = new HttpClient(mockHttp.Object);
            var client = new TestApiClient(httpClient);
            var url = string.Empty;

            // Act
            var exception = Assert.ThrowsAsync<ReadableException>(async () =>
                                await client.TestDeleteAsync(url, new { }, CancellationToken.None));

            // Assert
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.InnerException, Is.TypeOf<InvalidOperationException>());
        }

        [Test(Description = "Если при запросе на удаление ответ 200, то нет исключений.")]
        public void DeleteAsync_ResponseOk_NoException()
        {
            // Arrange
            var mockHttp = new Mock<HttpMessageHandler>();
            mockHttp.Protected()
                    .Setup<Task<HttpResponseMessage>>(
                        "SendAsync",
                        ItExpr.Is<HttpRequestMessage>(m => m.Method == HttpMethod.Delete),
                        ItExpr.IsAny<CancellationToken>())
                    .ReturnsAsync(new HttpResponseMessage() { StatusCode = System.Net.HttpStatusCode.OK });
            var httpClient = new HttpClient(mockHttp.Object);
            var client = new TestApiClient(httpClient);
            var url = @"http:\\localhost";

            // Act
            // Assert
            Assert.DoesNotThrowAsync(async () =>
                        await client.TestDeleteAsync(url, new { }, CancellationToken.None));
        }

        [Test(Description = "Если при запросе на удаление ошибка 400, то выбрасывается исключение.")]
        public void PostAsync_BadRequest_ReadableException()
        {
            // Arrange
            var mockHttp = new Mock<HttpMessageHandler>();
            mockHttp.Protected()
                    .Setup<Task<HttpResponseMessage>>(
                        "SendAsync",
                        ItExpr.Is<HttpRequestMessage>(m => m.Method == HttpMethod.Post),
                        ItExpr.IsAny<CancellationToken>())
                    .ReturnsAsync(new HttpResponseMessage() { StatusCode = System.Net.HttpStatusCode.BadRequest });
            var httpClient = new HttpClient(mockHttp.Object);
            var client = new TestApiClient(httpClient);
            var url = @"http:\\localhost";

            // Act
            var exception = Assert.ThrowsAsync<ReadableException>(async () =>
                                await client.TestPostAsync(url, new { }, CancellationToken.None));

            // Assert
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.InnerException, Is.Not.Null);
            Assert.That(exception.InnerException, Is.TypeOf<ReadableException>());
        }

        [Test(Description = "Если при запросе на удаление ответ URL невалидный, то выбрасывается исключение.")]
        public void PostAsyn_UrlIsInvalid_InvalidOperationException()
        {
            // Arrange
            var mockHttp = new Mock<HttpMessageHandler>();
            mockHttp.Protected()
                    .Setup<Task<HttpResponseMessage>>(
                        "SendAsync",
                        ItExpr.Is<HttpRequestMessage>(m => m.Method == HttpMethod.Post),
                        ItExpr.IsAny<CancellationToken>())
                    .ReturnsAsync(new HttpResponseMessage() { StatusCode = System.Net.HttpStatusCode.BadRequest });
            var httpClient = new HttpClient(mockHttp.Object);
            var client = new TestApiClient(httpClient);
            var url = string.Empty;

            // Act
            var exception = Assert.ThrowsAsync<ReadableException>(async () =>
                                await client.TestPostAsync(url, new { }, CancellationToken.None));

            // Assert
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.InnerException, Is.TypeOf<InvalidOperationException>());
        }

        [Test(Description = "Если при запросе на удаление ответ 200, то нет исключений.")]
        public void PostAsyn_ResponseOk_NoException()
        {
            // Arrange
            var mockHttp = new Mock<HttpMessageHandler>();
            mockHttp.Protected()
                    .Setup<Task<HttpResponseMessage>>(
                        "SendAsync",
                        ItExpr.Is<HttpRequestMessage>(m => m.Method == HttpMethod.Post),
                        ItExpr.IsAny<CancellationToken>())
                    .ReturnsAsync(new HttpResponseMessage() { StatusCode = System.Net.HttpStatusCode.OK });
            var httpClient = new HttpClient(mockHttp.Object);
            var client = new TestApiClient(httpClient);
            var url = @"http:\\localhost";

            // Act
            // Assert
            Assert.DoesNotThrowAsync(async () =>
                        await client.TestPostAsync(url, new { }, CancellationToken.None));
        }

        private class TestApiClient(HttpClient httpClient): BaseApiClient(httpClient)
        {
            public Task TestDeleteAsync(string url, object body, CancellationToken cancellationToken)
            {
                return DeleteAsync(url, body, cancellationToken);
            }
            public Task TestPostAsync(string url, object body, CancellationToken cancellationToken)
            {
                return PostAsync(url, body, cancellationToken);
            }
        }
    }
}
