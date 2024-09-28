using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Microsoft.AspNetCore.Routing;
using InsuranceGoSmoke.Common.Hosts.Api.Filters.RateLimit;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System.Net;
using InsuranceGoSmoke.Common.Contracts.Exceptions.Feature;

namespace InsuranceGoSmoke.Common.Tests.UnitTests.Hosts.Api.Filters.RateLimit
{
    internal class RateLimitAttributeTests
    {
        [Test(Description = "При отработке функциональности атрибута должен быть зарегистрирован Redis, иначе исключение")]
        public void OnActionExecutionAsync_IDistributedCacheNull_FeatureConfigurationException()
        {
            //Arrange
            var controller = new Controller();
            var modelState = new ModelStateDictionary();
            modelState.AddModelError("", "error");
            var httpContext = new DefaultHttpContext();
            var provider = new Mock<IServiceProvider>();
            httpContext.RequestServices = provider.Object;
            var context = new ActionExecutingContext(
                new ActionContext(
                    httpContext: httpContext,
                    routeData: new RouteData(),
                    actionDescriptor: new ActionDescriptor(),
                    modelState: modelState
                ),
                [],
                new Dictionary<string, object?>(),
                controller);
            var metadata = new List<IFilterMetadata>();

            var attr = new RateLimitAttribute(1, 1);
            ActionExecutionDelegate next = () => {
                var ctx = new ActionExecutedContext(context, metadata, controller);
                return Task.FromResult(ctx);
            };
            provider.Setup(c => c.GetService(typeof(IDistributedCache)))
                    .Returns((IDistributedCache?)null);

            //Act
            var exception = Assert.ThrowsAsync<FeatureConfigurationException>(async () => await attr.OnActionExecutionAsync(context, next));

            //Assert
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.EqualTo("Для ограничения попыток вызова методов, необходимо зарегистрировать Redis."));
        }

        [Test(Description = "При отработке функциональности атрибута должно добавляться значение в кэш")]
        public async Task OnActionExecutionAsync_AttemptDataNull_CacheIsNotEmpty()
        {
            //Arrange
            var controller = new Controller();
            var modelState = new ModelStateDictionary();
            modelState.AddModelError("", "error");
            var httpContext = new DefaultHttpContext();
            var provider = new Mock<IServiceProvider>();
            httpContext.RequestServices = provider.Object;
            var context = new ActionExecutingContext(
                new ActionContext(
                    httpContext: httpContext,
                    routeData: new RouteData(),
                    actionDescriptor: new ActionDescriptor(),
                    modelState: modelState
                ),
                new List<IFilterMetadata>(),
                new Dictionary<string, object?>(),
                controller);
            var metadata = new List<IFilterMetadata>();

            var attr = new RateLimitAttribute(1, 1);
            ActionExecutionDelegate next = () => {
                var ctx = new ActionExecutedContext(context, metadata, controller);
                return Task.FromResult(ctx);
            };
            var options = Options.Create(new MemoryDistributedCacheOptions());
            IDistributedCache cache = new MemoryDistributedCache(options);
            provider.Setup(c => c.GetService(typeof(IDistributedCache)))
                    .Returns(cache);

            //Act
            await attr.OnActionExecutionAsync(context, next);

            Assert.That(cache.GetString("unknown_ip:"), Is.EqualTo("1"));
        }


        [Test(Description = "При отработке функциональности атрибута если в кэше уже есть превышающее значение, то ответ - 429")]
        public async Task OnActionExecutionAsync_AttemptDataIsNotNull_TooManyRequestsResult()
        {
            //Arrange
            var controller = new Controller();
            var modelState = new ModelStateDictionary();
            modelState.AddModelError("", "error");
            var httpContext = new DefaultHttpContext();
            var provider = new Mock<IServiceProvider>();
            httpContext.RequestServices = provider.Object;
            var context = new ActionExecutingContext(
                new ActionContext(
                    httpContext: httpContext,
                    routeData: new RouteData(),
                    actionDescriptor: new ActionDescriptor(),
                    modelState: modelState
                ),
                new List<IFilterMetadata>(),
                new Dictionary<string, object?>(),
                controller);
            var metadata = new List<IFilterMetadata>();

            var attr = new RateLimitAttribute(1, 1);
            ActionExecutionDelegate next = () => {
                var ctx = new ActionExecutedContext(context, metadata, controller);
                return Task.FromResult(ctx);
            };
            var options = Options.Create(new MemoryDistributedCacheOptions());
            IDistributedCache cache = new MemoryDistributedCache(options);
            await cache.SetStringAsync("unknown_ip:", "1");
            provider.Setup(c => c.GetService(typeof(IDistributedCache)))
                    .Returns(cache);

            //Act
            await attr.OnActionExecutionAsync(context, next);
            Assert.Multiple(() =>
            {
                Assert.That(context.Result, Is.TypeOf<ContentResult>());
                Assert.That((context.Result as ContentResult)?.StatusCode, Is.EqualTo((int)HttpStatusCode.TooManyRequests));
            });
        }

        public class Controller: ControllerBase
        {
        }
    }
}
