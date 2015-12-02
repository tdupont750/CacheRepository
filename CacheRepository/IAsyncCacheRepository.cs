using System;
using System.Threading;
using System.Threading.Tasks;

namespace CacheRepository
{
    /// <summary>
    /// Generic Caching Mechanism
    /// </summary>
    public interface IAsyncCacheRepository
    {
        /// <summary>
        /// Get by key
        /// </summary>
        /// <typeparam name="T">Type of the cached object</typeparam>
        /// <param name="key">Cache key</param>
        /// <param name="cancelToken">Cancellation Token</param>
        /// <returns>Cached object, default if no record found</returns>
        Task<T> GetAsync<T>(string key, CancellationToken cancelToken = default(CancellationToken));

        /// <summary>
        /// Get or set by key.
        /// If key is not found, loader is invoked and the result is cached under the specified key.
        /// </summary>
        /// <typeparam name="T">Type of the cached object</typeparam>
        /// <param name="key">Cache key</param>
        /// <param name="loader">Delegate to invoke if cached item is not found</param>
        /// <param name="cancelToken">Cancellation Token</param>
        /// <returns>Cached object or result of loader</returns>
        Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> loader, CancellationToken cancelToken = default(CancellationToken));

        /// <summary>
        /// Get or set by key.
        /// If key is not found, loader is invoked and the result is cached under the specified key.
        /// </summary>
        /// <typeparam name="T">Type of the cached object</typeparam>
        /// <param name="key">Cache key</param>
        /// <param name="loader">Delegate to invoke if cached item is not found</param>
        /// <param name="expiration">Abosolute expiration to use if object is loaded and cached</param>
        /// <param name="cancelToken">Cancellation Token</param>
        /// <returns>Cached object or result of loader</returns>
        Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> loader, DateTime expiration, CancellationToken cancelToken = default(CancellationToken));

        /// <summary>
        /// Get or set by key.
        /// If key is not found, loader is invoked and the result is cached under the specified key.
        /// </summary>
        /// <typeparam name="T">Type of the cached object</typeparam>
        /// <param name="key">Cache key</param>
        /// <param name="loader">Delegate to invoke if cached item is not found</param>
        /// <param name="expiration">Abosolute expiration to use if object is loaded and cached</param>
        /// <param name="cancelToken">Cancellation Token</param>
        /// <returns>Cached object or result of loader</returns>
        Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> loader, Func<T, DateTime> expiration, CancellationToken cancelToken = default(CancellationToken));

        /// <summary>
        /// Get or set by key.
        /// If key is not found, loader is invoked and the result is cached under the specified key.
        /// </summary>
        /// <typeparam name="T">Type of the cached object</typeparam>
        /// <param name="key">Cache key</param>
        /// <param name="loader">Delegate to invoke if cached item is not found</param>
        /// <param name="sliding">Sliding expiration to use if object is loaded and cached</param>
        /// <param name="cancelToken">Cancellation Token</param>
        /// <returns>Cached object or result of loader</returns>
        Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> loader, TimeSpan sliding, CancellationToken cancelToken = default(CancellationToken));

        /// <summary>
        /// Get or set by key.
        /// If key is not found, loader is invoked and the result is cached under the specified key.
        /// </summary>
        /// <typeparam name="T">Type of the cached object</typeparam>
        /// <param name="key">Cache key</param>
        /// <param name="loader">Delegate to invoke if cached item is not found</param>
        /// <param name="sliding">Sliding expiration to use if object is loaded and cached</param>
        /// <param name="cancelToken">Cancellation Token</param>
        /// <returns>Cached object or result of loader</returns>
        Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> loader, Func<T, TimeSpan> sliding, CancellationToken cancelToken = default(CancellationToken));

        /// <summary>
        /// Get or set by key.
        /// If key is not found, loader is invoked and the result is cached under the specified key.
        /// Cache absolute expiration is taken from the enum value or override by application configuration. 
        /// </summary>
        /// <typeparam name="T">Type of the cached object</typeparam>
        /// <param name="key">Cache key</param>
        /// <param name="loader">Delegate to invoke if cached item is not found</param>
        /// <param name="expiration">Abosolute expiration to use if object is loaded and cached</param>
        /// <param name="cancelToken">Cancellation Token</param>
        /// <returns>Cached object or result of loader</returns>
        Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> loader, CacheExpiration expiration, CancellationToken cancelToken = default(CancellationToken));

        /// <summary>
        /// Get or set by key.
        /// If key is not found, loader is invoked and the result is cached under the specified key.
        /// Cache absolute expiration is taken from the enum value or override by application configuration. 
        /// </summary>
        /// <typeparam name="T">Type of the cached object</typeparam>
        /// <param name="key">Cache key</param>
        /// <param name="loader">Delegate to invoke if cached item is not found</param>
        /// <param name="expiration">Abosolute expiration to use if object is loaded and cached</param>
        /// <param name="cancelToken">Cancellation Token</param>
        /// <returns>Cached object or result of loader</returns>
        Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> loader, Func<T, CacheExpiration> expiration, CancellationToken cancelToken = default(CancellationToken));

        /// <summary>
        /// Get or set by key.
        /// If key is not found, loader is invoked and the result is cached under the specified key.
        /// Cache sliding expiration is taken from the enum value or override by application configuration. 
        /// </summary>
        /// <typeparam name="T">Type of the cached object</typeparam>
        /// <param name="key">Cache key</param>
        /// <param name="loader">Delegate to invoke if cached item is not found</param>
        /// <param name="sliding">Sliding expiration to use if object is loaded and cached</param>
        /// <param name="cancelToken">Cancellation Token</param>
        /// <returns>Cached object or result of loader</returns>
        Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> loader, CacheSliding sliding, CancellationToken cancelToken = default(CancellationToken));

        /// <summary>
        /// Get or set by key.
        /// If key is not found, loader is invoked and the result is cached under the specified key.
        /// Cache sliding expiration is taken from the enum value or override by application configuration. 
        /// </summary>
        /// <typeparam name="T">Type of the cached object</typeparam>
        /// <param name="key">Cache key</param>
        /// <param name="loader">Delegate to invoke if cached item is not found</param>
        /// <param name="sliding">Sliding expiration to use if object is loaded and cached</param>
        /// <param name="cancelToken">Cancellation Token</param>
        /// <returns>Cached object or result of loader</returns>
        Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> loader, Func<T, CacheSliding> sliding, CancellationToken cancelToken = default(CancellationToken));

        /// <summary>
        /// Set by key
        /// </summary>
        /// <param name="key">Cache key</param>
        /// <param name="value">Value to be cached</param>
        /// <param name="cancelToken">Cancellation Token</param>
        /// <returns>Task</returns>
        Task SetAsync<T>(string key, T value, CancellationToken cancelToken = default(CancellationToken));

        /// <summary>
        /// Set by key
        /// </summary>
        /// <param name="key">Cache key</param>
        /// <param name="value">Value to be cached</param>
        /// <param name="expiration">Abosolute expiration to use for cache</param>
        /// <param name="cancelToken">Cancellation Token</param>
        /// <returns>Task</returns>
        Task SetAsync<T>(string key, T value, DateTime expiration, CancellationToken cancelToken = default(CancellationToken));

        /// <summary>
        /// Set by key
        /// </summary>
        /// <param name="key">Cache key</param>
        /// <param name="value">Value to be cached</param>
        /// <param name="sliding">Sliding expiration to use for cache</param>
        /// <param name="cancelToken">Cancellation Token</param>
        /// <returns>Task</returns>
        Task SetAsync<T>(string key, T value, TimeSpan sliding, CancellationToken cancelToken = default(CancellationToken));

        /// <summary>
        /// Set by key
        /// Cache absolute expiration is taken from the enum value or override by application configuration. 
        /// </summary>
        /// <param name="key">Cache key</param>
        /// <param name="value">Value to be cached</param>
        /// <param name="expiration">Abosolute expiration to use for cache</param>
        /// <param name="cancelToken">Cancellation Token</param>
        /// <returns>Task</returns>
        Task SetAsync<T>(string key, T value, CacheExpiration expiration, CancellationToken cancelToken = default(CancellationToken));

        /// <summary>
        /// Set by key
        /// Cache sliding expiration is taken from the enum value or override by application configuration. 
        /// </summary>
        /// <param name="key">Cache key</param>
        /// <param name="value">Value to be cached</param>
        /// <param name="sliding">Sliding expiration to use for cache</param>
        /// <param name="cancelToken">Cancellation Token</param>
        /// <returns>Task</returns>
        Task SetAsync<T>(string key, T value, CacheSliding sliding, CancellationToken cancelToken = default(CancellationToken));

        /// <summary>
        /// Remove by key
        /// </summary>
        /// <param name="key">Cache key</param>
        /// <param name="cancelToken">Cancellation Token</param>
        /// <returns>Task</returns>
        Task RemoveAsync(string key, CancellationToken cancelToken = default(CancellationToken));

        /// <summary>
        /// Clear all cache
        /// </summary>
        /// <param name="cancelToken">Cancellation Token</param>
        /// <returns>Task</returns>
        Task ClearAllAsync(CancellationToken cancelToken = default(CancellationToken));
    }
}