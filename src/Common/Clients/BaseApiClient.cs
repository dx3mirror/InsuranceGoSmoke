using InsuranceGoSmoke.Common.Contracts.Exceptions.Common;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Text.Json;

namespace InsuranceGoSmoke.Common.Clients
{
    /// <summary>
    /// Базовый класс RestApi.
    /// </summary>
    public abstract class BaseApiClient
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="httpClient">HTTP клиент.</param>
        protected BaseApiClient(HttpClient httpClient)
        {
            HttpClient = httpClient;
        }

        /// <summary>
        /// HTTP-клиент.
        /// </summary>
        protected HttpClient HttpClient { get; }

        /// <summary>
        /// Выполняет запрос на удаление.
        /// </summary>
        /// <param name="url">URL.</param>
        /// <param name="body">Данные запроса.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <exception cref="ReadableException">Исключение при ошибках выполнения запроса.</exception>
        protected async Task DeleteAsync(string url, object body, CancellationToken cancellationToken)
        {
            try
            {
                using var response = await HttpClient.SendAsync(CreateDeleteRequest(url, body), cancellationToken);
                if (!response.IsSuccessStatusCode)
                {
                    var errorMessage = await response.Content.ReadAsStringAsync(cancellationToken);
                    throw new ReadableException($"Произошла ошибка при запросе {url}. Ответ: {response.StatusCode}: {errorMessage}");
                }
            }
            catch (Exception ex)
            {
                throw new ReadableException($"Произошла ошибка при запросе {url}", ex);
            }
        }
        /// <summary>
        /// Выполняет запрос GET.
        /// </summary>
        /// <param name="url">URL.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <typeparam name="T">Тип данных, которые возвращаются в ответе.</typeparam>
        /// <returns>Десериализованные данные типа T.</returns>
        /// <exception cref="ReadableException">Исключение при ошибках выполнения запроса.</exception>
        protected async Task<T> GetAsync<T>(string url, CancellationToken cancellationToken)
        {
           try
           {
              using var response = await HttpClient.SendAsync(CreateGetRequest(url), cancellationToken);
              if (!response.IsSuccessStatusCode)
              {
                var errorMessage = await response.Content.ReadAsStringAsync(cancellationToken);
                throw new ReadableException($"Произошла ошибка при запросе {url}. Ответ: {response.StatusCode}: {errorMessage}");
              }

             var content = await response.Content.ReadAsStringAsync(cancellationToken);
             var result = JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions
             {
               PropertyNameCaseInsensitive = true
             });

             if (result == null)
             {
               throw new ReadableException($"Не удалось десериализовать ответ от {url}.");
             }

             return result;
            }
            catch (Exception ex)
            {
              throw new ReadableException($"Произошла ошибка при запросе {url}", ex);
            }
        }

        /// <summary>
        /// Выполняет POST запрос.
        /// </summary>
        /// <param name="url">URL.</param>
        /// <param name="body">Данные запроса.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <exception cref="ReadableException">Исключение при ошибках выполнения запроса.</exception>
        protected async Task PostAsync(string url, object body, CancellationToken cancellationToken)
        {
            try
            {
                using var response = await HttpClient.SendAsync(CreatePostRequest(url, body), cancellationToken);
                if (!response.IsSuccessStatusCode)
                {
                    var errorMessage = await response.Content.ReadAsStringAsync(cancellationToken);
                    throw new ReadableException($"Произошла ошибка при запросе {url}. Ответ: {response.StatusCode}: {errorMessage}");
                }
            }
            catch (Exception ex)
            {
                throw new ReadableException($"Произошла ошибка при запросе {url}", ex);
            }
        }

        /// <summary>
        /// Создает запрос с типом POST.
        /// </summary>
        /// <param name="url">URL.</param>
        /// <param name="body">Данные запроса.</param>
        /// <returns>Запрос</returns>
        protected HttpRequestMessage CreatePostRequest(string url, object body)
        {
            var message = CreateRequest(url, body, HttpMethod.Post);
            return message;
        }

        /// <summary>
        /// Создает запрос с типом DELETE.
        /// </summary>
        /// <param name="url">URL.</param>
        /// <param name="body">Данные запроса.</param>
        /// <returns>Запрос</returns>
        protected HttpRequestMessage CreateDeleteRequest(string url, object body)
        {
            var message = CreateRequest(url, body, HttpMethod.Delete);
            return message;
        }

        /// <summary>
        /// Создаёт авторизационный заголовок
        /// </summary>
        /// <returns>Значение заголовка авторизации</returns>
        protected virtual AuthenticationHeaderValue? CreateAuthorizationHeader()
        {
            return null;
        }

        /// <summary>
        /// Создает запрос.
        /// </summary>
        /// <param name="url">URL.</param>
        /// <param name="body">Данные запроса.</param>
        /// <param name="method">Глагол запроса.</param>
        /// <returns>Сообщение запроса.</returns>
        protected HttpRequestMessage CreateRequest(string url, object body, HttpMethod method)
        {
            var json = JsonSerializer.Serialize(body);
            var content = new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json);
            var message = new HttpRequestMessage(method, url)
            {
                Content = content
            };

            FillRequest(message);

            return message;
        }

        private void FillRequest(HttpRequestMessage message)
        {
            var auth = CreateAuthorizationHeader();
            if (auth != null)
            {
                message.Headers.Authorization = auth;
            }
        }

        /// <summary>
        /// Создает запрос GET.
        /// </summary>
        /// <param name="url">URL.</param>
        /// <returns>Запрос GET.</returns>
        protected HttpRequestMessage CreateGetRequest(string url)
        {
           var message = new HttpRequestMessage(HttpMethod.Get, url);
           FillRequest(message);
           return message;
        }
    }
}
