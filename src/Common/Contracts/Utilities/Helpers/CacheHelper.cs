using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace InsuranceGoSmoke.Common.Contracts.Utilities.Helpers
{
    /// <summary>
    /// Helper работы с кэшем.
    /// </summary>
    public static class CacheHelper
    {
        /// <summary>
        /// Возвращает данныеиз кэша.
        /// </summary>
        /// <typeparam name="T">Тип данных.</typeparam>
        /// <param name="key">Ключ.</param>
        /// <param name="action">Действие, если в кэше нет значений.</param>
        /// <param name="options">Настройки.</param>
        /// <param name="distributedCache">Сервис работы с кэшем.</param>
        /// <param name="logger">Логгер.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Данные.</returns>
        public static async Task<T?> GetCachedData<T>(string key, Func<Task<T>> action, DistributedCacheEntryOptions options,
            IDistributedCache distributedCache, ILogger logger, CancellationToken cancellationToken)
        {
            T? result;

            var json = await distributedCache.GetStringAsync(key, cancellationToken);
            if (string.IsNullOrEmpty(json))
            {
                result = await action();
                await distributedCache.SetStringAsync(key, JsonSerializer.Serialize(result), options, cancellationToken);
                return result;
            }

            try
            {
                result = JsonSerializer.Deserialize<T>(json);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Не удалось распарсить данные из кэша с ключом: {Key}", key);
                return await action();
            }

            return result;
        }
    }
}
