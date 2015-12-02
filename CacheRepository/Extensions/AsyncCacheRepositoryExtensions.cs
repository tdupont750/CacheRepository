using System;
using System.Threading;
using System.Threading.Tasks;

namespace CacheRepository
{
    /// <summary>
    /// Extensions for IAsyncCacheRepository
    /// </summary>
    public static class AsyncCacheRepositoryExtensions
    {
        /// <summary>
        /// Get or set by type.
        /// Cache key is automagically created from object type and identify.
        /// Example: repo.GetOrSetByType{User}(1, LoadUserByIdFromDb(1)); // Get or load and set User 1
        /// </summary>
        /// <typeparam name="T">Type of cached object</typeparam>
        /// <param name="repo">IAsyncCacheRepository</param>
        /// <param name="identifier">Type specific unique identify for object</param>
        /// <param name="loader">Delegate to invoke if cached item is not found</param>
        /// <param name="cancelToken">Cancellation Token</param>
        /// <returns>Cached object or result of loader</returns>
        public static Task<T> GetOrSetByTypeAsync<T>(this IAsyncCacheRepository repo, object identifier, Func<Task<T>> loader, CancellationToken cancelToken = default(CancellationToken))
        {
            var key = CreateKey<T>(identifier);
            return repo.GetOrSetAsync(key, loader, cancelToken);
        }

        /// <summary>
        /// Get or set by type.
        /// Cache key is automagically created from object type and identify.
        /// Example: repo.GetOrSetByType{User}(1, LoadUserByIdFromDb(1)); // Get or load and set User 1
        /// </summary>
        /// <typeparam name="T">Type of cached object</typeparam>
        /// <param name="repo">IAsyncCacheRepository</param>
        /// <param name="identifier">Type specific unique identify for object</param>
        /// <param name="loader">Delegate to invoke if cached item is not found</param>
        /// <param name="expiration">Abosolute expiration to use if object is loaded and cached</param>
        /// <param name="cancelToken">Cancellation Token</param>
        /// <returns>Cached object or result of loader</returns>
        public static Task<T> GetOrSetByTypeAsync<T>(this IAsyncCacheRepository repo, object identifier, Func<Task<T>> loader, DateTime expiration, CancellationToken cancelToken = default(CancellationToken))
        {
            var key = CreateKey<T>(identifier);
            return repo.GetOrSetAsync(key, loader, expiration, cancelToken);
        }

        /// <summary>
        /// Get or set by type.
        /// Cache key is automagically created from object type and identify.
        /// Example: repo.GetOrSetByType{User}(1, LoadUserByIdFromDb(1)); // Get or load and set User 1
        /// </summary>
        /// <typeparam name="T">Type of cached object</typeparam>
        /// <param name="repo">IAsyncCacheRepository</param>
        /// <param name="identifier">Type specific unique identify for object</param>
        /// <param name="loader">Delegate to invoke if cached item is not found</param>
        /// <param name="expiration">Abosolute expiration to use if object is loaded and cached</param>
        /// <param name="cancelToken">Cancellation Token</param>
        /// <returns>Cached object or result of loader</returns>
        public static Task<T> GetOrSetByTypeAsync<T>(this IAsyncCacheRepository repo, object identifier, Func<Task<T>> loader, Func<T, DateTime> expiration, CancellationToken cancelToken = default(CancellationToken))
        {
            var key = CreateKey<T>(identifier);
            return repo.GetOrSetAsync(key, loader, expiration, cancelToken);
        }

        /// <summary>
        /// Get or set by type.
        /// Cache key is automagically created from object type and identify.
        /// Example: repo.GetOrSetByType{User}(1, LoadUserByIdFromDb(1)); // Get or load and set User 1
        /// </summary>
        /// <typeparam name="T">Type of cached object</typeparam>
        /// <param name="repo">IAsyncCacheRepository</param>
        /// <param name="identifier">Type specific unique identify for object</param>
        /// <param name="loader">Delegate to invoke if cached item is not found</param>
        /// <param name="sliding">Sliding expiration to use if object is loaded and cached</param>
        /// <param name="cancelToken">Cancellation Token</param>
        /// <returns>Cached object or result of loader</returns>
        public static Task<T> GetOrSetByTypeAsync<T>(this IAsyncCacheRepository repo, object identifier, Func<Task<T>> loader, TimeSpan sliding, CancellationToken cancelToken = default(CancellationToken))
        {
            var key = CreateKey<T>(identifier);
            return repo.GetOrSetAsync(key, loader, sliding, cancelToken);
        }

        /// <summary>
        /// Get or set by type.
        /// Cache key is automagically created from object type and identify.
        /// Example: repo.GetOrSetByType{User}(1, LoadUserByIdFromDb(1)); // Get or load and set User 1
        /// </summary>
        /// <typeparam name="T">Type of cached object</typeparam>
        /// <param name="repo">IAsyncCacheRepository</param>
        /// <param name="identifier">Type specific unique identify for object</param>
        /// <param name="loader">Delegate to invoke if cached item is not found</param>
        /// <param name="sliding">Sliding expiration to use if object is loaded and cached</param>
        /// <param name="cancelToken">Cancellation Token</param>
        /// <returns>Cached object or result of loader</returns>
        public static Task<T> GetOrSetByTypeAsync<T>(this IAsyncCacheRepository repo, object identifier, Func<Task<T>> loader, Func<T, TimeSpan> sliding, CancellationToken cancelToken = default(CancellationToken))
        {
            var key = CreateKey<T>(identifier);
            return repo.GetOrSetAsync(key, loader, sliding, cancelToken);
        }

        /// <summary>
        /// Get or set by type.
        /// Cache key is automagically created from object type and identify.
        /// Cache absolute expiration is taken from the enum value or override by application configuration. 
        /// Example: repo.GetOrSetByType{User}(1, LoadUserByIdFromDb(1)); // Get or load and set User 1
        /// </summary>
        /// <typeparam name="T">Type of cached object</typeparam>
        /// <param name="repo">IAsyncCacheRepository</param>
        /// <param name="identifier">Type specific unique identify for object</param>
        /// <param name="loader">Delegate to invoke if cached item is not found</param>
        /// <param name="expiration">Abosolute expiration to use if object is loaded and cached</param>
        /// <param name="cancelToken">Cancellation Token</param>
        /// <returns>Cached object or result of loader</returns>
        public static Task<T> GetOrSetByTypeAsync<T>(this IAsyncCacheRepository repo, object identifier, Func<Task<T>> loader, CacheExpiration expiration, CancellationToken cancelToken = default(CancellationToken))
        {
            var key = CreateKey<T>(identifier);
            return repo.GetOrSetAsync(key, loader, expiration, cancelToken);
        }

        /// <summary>
        /// Get or set by type.
        /// Cache key is automagically created from object type and identify.
        /// Cache absolute expiration is taken from the enum value or override by application configuration. 
        /// Example: repo.GetOrSetByType{User}(1, LoadUserByIdFromDb(1)); // Get or load and set User 1
        /// </summary>
        /// <typeparam name="T">Type of cached object</typeparam>
        /// <param name="repo">IAsyncCacheRepository</param>
        /// <param name="identifier">Type specific unique identify for object</param>
        /// <param name="loader">Delegate to invoke if cached item is not found</param>
        /// <param name="expiration">Abosolute expiration to use if object is loaded and cached</param>
        /// <param name="cancelToken">Cancellation Token</param>
        /// <returns>Cached object or result of loader</returns>
        public static Task<T> GetOrSetByTypeAsync<T>(this IAsyncCacheRepository repo, object identifier, Func<Task<T>> loader, Func<T, CacheExpiration> expiration, CancellationToken cancelToken = default(CancellationToken))
        {
            var key = CreateKey<T>(identifier);
            return repo.GetOrSetAsync(key, loader, expiration, cancelToken);
        }

        /// <summary>
        /// Get or set by type.
        /// Cache key is automagically created from object type and identify.
        /// Cache sliding expiration is taken from the enum value or override by application configuration. 
        /// Example: repo.GetOrSetByType{User}(1, LoadUserByIdFromDb(1)); // Get or load and set User 1
        /// </summary>
        /// <typeparam name="T">Type of cached object</typeparam>
        /// <param name="repo">IAsyncCacheRepository</param>
        /// <param name="identifier">Type specific unique identify for object</param>
        /// <param name="loader">Delegate to invoke if cached item is not found</param>
        /// <param name="sliding">Sliding expiration to use if object is loaded and cached</param>
        /// <param name="cancelToken">Cancellation Token</param>
        /// <returns>Cached object or result of loader</returns>
        public static Task<T> GetOrSetByTypeAsync<T>(this IAsyncCacheRepository repo, object identifier, Func<Task<T>> loader, CacheSliding sliding, CancellationToken cancelToken = default(CancellationToken))
        {
            var key = CreateKey<T>(identifier);
            return repo.GetOrSetAsync(key, loader, sliding, cancelToken);
        }

        /// <summary>
        /// Get or set by type.
        /// Cache key is automagically created from object type and identify.
        /// Cache sliding expiration is taken from the enum value or override by application configuration. 
        /// Example: repo.GetOrSetByType{User}(1, LoadUserByIdFromDb(1)); // Get or load and set User 1
        /// </summary>
        /// <typeparam name="T">Type of cached object</typeparam>
        /// <param name="repo">IAsyncCacheRepository</param>
        /// <param name="identifier">Type specific unique identify for object</param>
        /// <param name="loader">Delegate to invoke if cached item is not found</param>
        /// <param name="sliding">Sliding expiration to use if object is loaded and cached</param>
        /// <param name="cancelToken">Cancellation Token</param>
        /// <returns>Cached object or result of loader</returns>
        public static Task<T> GetOrSetByTypeAsync<T>(this IAsyncCacheRepository repo, object identifier, Func<Task<T>> loader, Func<T, CacheSliding> sliding, CancellationToken cancelToken = default(CancellationToken))
        {
            var key = CreateKey<T>(identifier);
            return repo.GetOrSetAsync(key, loader, sliding, cancelToken);
        }

        /// <summary>
        /// Set by type.
        /// Cache key is automagically created from object type and identify.
        /// Example: repo.SetByType{User}(1, user); // Set user by their Id
        /// </summary>
        /// <typeparam name="T">Type of cached object</typeparam>
        /// <param name="repo">IAsyncCacheRepository</param>
        /// <param name="identifier">Type specific unique identify for object</param>
        /// <param name="cancelToken">Cancellation Token</param>
        /// <param name="value">Object to be cached</param>
        /// <returns>Task</returns>
        public static Task SetByTypeAsync<T>(this IAsyncCacheRepository repo, object identifier, T value, CancellationToken cancelToken = default(CancellationToken))
        {
            var key = CreateKey<T>(identifier);
            return repo.SetAsync(key, value, cancelToken);
        }

        /// <summary>
        /// Set by type.
        /// Cache key is automagically created from object type and identify.
        /// Example: repo.SetByType{User}(1, user); // Set user by their Id
        /// </summary>
        /// <typeparam name="T">Type of cached object</typeparam>
        /// <param name="repo">IAsyncCacheRepository</param>
        /// <param name="identifier">Type specific unique identify for object</param>
        /// <param name="expiration">Abosolute expiration to use for cache</param>
        /// <param name="value">Object to be cached</param>
        /// <param name="cancelToken">Cancellation Token</param>
        /// <returns>Task</returns>
        public static Task SetByTypeAsync<T>(this IAsyncCacheRepository repo, object identifier, T value, DateTime expiration, CancellationToken cancelToken = default(CancellationToken))
        {
            var key = CreateKey<T>(identifier);
            return repo.SetAsync(key, value, expiration, cancelToken);
        }

        /// <summary>
        /// Set by type.
        /// Cache key is automagically created from object type and identify.
        /// Example: repo.SetByType{User}(1, user); // Set user by their Id
        /// </summary>
        /// <typeparam name="T">Type of cached object</typeparam>
        /// <param name="repo">IAsyncCacheRepository</param>
        /// <param name="identifier">Type specific unique identify for object</param>
        /// <param name="sliding">Sliding expiration to use for cache</param>
        /// <param name="value">Object to be cached</param>
        /// <param name="cancelToken">Cancellation Token</param>
        /// <returns>Task</returns>
        public static Task SetByTypeAsync<T>(this IAsyncCacheRepository repo, object identifier, T value, TimeSpan sliding, CancellationToken cancelToken = default(CancellationToken))
        {
            var key = CreateKey<T>(identifier);
            return repo.SetAsync(key, value, sliding, cancelToken);
        }

        /// <summary>
        /// Set by type.
        /// Cache key is automagically created from object type and identify.
        /// Cache absolute expiration is taken from the enum value or override by application configuration. 
        /// Example: repo.SetByType{User}(1, user); // Set user by their Id
        /// </summary>
        /// <typeparam name="T">Type of cached object</typeparam>
        /// <param name="repo">IAsyncCacheRepository</param>
        /// <param name="identifier">Type specific unique identify for object</param>
        /// <param name="expiration">Abosolute expiration to use for cache</param>
        /// <param name="value">Object to be cached</param>
        /// <param name="cancelToken">Cancellation Token</param>
        /// <returns>Task</returns>
        public static Task SetByTypeAsync<T>(this IAsyncCacheRepository repo, object identifier, T value, CacheExpiration expiration, CancellationToken cancelToken = default(CancellationToken))
        {
            var key = CreateKey<T>(identifier);
            return repo.SetAsync(key, value, expiration, cancelToken);
        }

        /// <summary>
        /// Set by type.
        /// Cache key is automagically created from object type and identify.
        /// Cache sliding expiration is taken from the enum value or override by application configuration. 
        /// Example: repo.SetByType{User}(1, user); // Set user by their Id
        /// </summary>
        /// <typeparam name="T">Type of cached object</typeparam>
        /// <param name="repo">IAsyncCacheRepository</param>
        /// <param name="identifier">Type specific unique identify for object</param>
        /// <param name="sliding">Sliding expiration to use for cache</param>
        /// <param name="value">Object to be cached</param>
        /// <param name="cancelToken">Cancellation Token</param>
        /// <returns>Task</returns>
        public static Task SetByTypeAsync<T>(this IAsyncCacheRepository repo, object identifier, T value, CacheSliding sliding, CancellationToken cancelToken = default(CancellationToken))
        {
            var key = CreateKey<T>(identifier);
            return repo.SetAsync(key, value, sliding, cancelToken);
        }

        /// <summary>
        /// Get by type.
        /// Cache key is automagically created from object type and identify.
        /// Example: repo.GetByType{User}(1); // Get User 1
        /// </summary>
        /// <typeparam name="T">Type of cached object</typeparam>
        /// <param name="repo">IAsyncCacheRepository</param>
        /// <param name="identifier">Type specific unique identify for object</param>
        /// <returns>Cached object, null if no record found</returns>
        /// <param name="cancelToken">Cancellation Token</param>
        /// <returns>Task</returns>
        public static Task<T> GetByTypeAsync<T>(this IAsyncCacheRepository repo, object identifier, CancellationToken cancelToken = default(CancellationToken))
        {
            var key = CreateKey<T>(identifier);
            return repo.GetAsync<T>(key, cancelToken);
        }

        /// <summary>
        /// Remove by type.
        /// Cache key is automagically created from object type and identify.
        /// Example: repo.RemoveByType{User}(1); // Removes user 1
        /// </summary>
        /// <typeparam name="T">Type of cached object</typeparam>
        /// <param name="repo">IAsyncCacheRepository</param>
        /// <param name="identifier">Type specific unique identify for object</param>
        /// <param name="cancelToken">Cancellation Token</param>
        /// <returns>Task</returns>
        public static Task RemoveByTypeAsync<T>(this IAsyncCacheRepository repo, object identifier, CancellationToken cancelToken = default(CancellationToken))
        {
            var key = CreateKey<T>(identifier);
            return repo.RemoveAsync(key, cancelToken);
        }

        internal static string CreateKey<T>(object identifier)
        {
            var type = typeof(T);

            return identifier == null
                ? type.FullName
                : String.Concat(type.FullName, "_", identifier);
        }
    }
}