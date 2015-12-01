using System;

namespace CacheRepository
{
    /// <summary>
    /// Extensions for ICacheRepository
    /// </summary>
    public static class CacheRepositoryExtensions
    {
        #region GetOrSetByType

        /// <summary>
        /// Get or set by type.
        /// Cache key is automagically created from object type and identify.
        /// Example: repo.GetOrSetByType<User>(1, LoadUserByIdFromDb(1)); // Get or load and set User 1
        /// </summary>
        /// <typeparam name="T">Type of cached object</typeparam>
        /// <param name="repo">ICacheRepository</param>
        /// <param name="identifier">Type specific unique identify for object</param>
        /// <param name="loader">Delegate to invoke if cached item is not found</param>
        /// <returns>Cached object or result of loader</returns>
        public static T GetOrSetByType<T>(this ICacheRepository repo, object identifier, Func<T> loader)
        {
            var key = CreateKey<T>(identifier);
            return repo.GetOrSet(key, loader);
        }

        /// <summary>
        /// Get or set by type.
        /// Cache key is automagically created from object type and identify.
        /// Example: repo.GetOrSetByType<User>(1, LoadUserByIdFromDb(1)); // Get or load and set User 1
        /// </summary>
        /// <typeparam name="T">Type of cached object</typeparam>
        /// <param name="repo">ICacheRepository</param>
        /// <param name="identifier">Type specific unique identify for object</param>
        /// <param name="loader">Delegate to invoke if cached item is not found</param>
        /// <param name="expiration">Abosolute expiration to use if object is loaded and cached</param>
        /// <returns>Cached object or result of loader</returns>
        public static T GetOrSetByType<T>(this ICacheRepository repo, object identifier, Func<T> loader, DateTime expiration)
        {
            var key = CreateKey<T>(identifier);
            return repo.GetOrSet(key, loader, expiration);
        }

        /// <summary>
        /// Get or set by type.
        /// Cache key is automagically created from object type and identify.
        /// Example: repo.GetOrSetByType<User>(1, LoadUserByIdFromDb(1)); // Get or load and set User 1
        /// </summary>
        /// <typeparam name="T">Type of cached object</typeparam>
        /// <param name="repo">ICacheRepository</param>
        /// <param name="identifier">Type specific unique identify for object</param>
        /// <param name="loader">Delegate to invoke if cached item is not found</param>
        /// <param name="expiration">Abosolute expiration to use if object is loaded and cached</param>
        /// <returns>Cached object or result of loader</returns>
        public static T GetOrSetByType<T>(this ICacheRepository repo, object identifier, Func<T> loader, Func<T, DateTime> expiration)
        {
            var key = CreateKey<T>(identifier);
            return repo.GetOrSet(key, loader, expiration);
        }

        /// <summary>
        /// Get or set by type.
        /// Cache key is automagically created from object type and identify.
        /// Example: repo.GetOrSetByType<User>(1, LoadUserByIdFromDb(1)); // Get or load and set User 1
        /// </summary>
        /// <typeparam name="T">Type of cached object</typeparam>
        /// <param name="repo">ICacheRepository</param>
        /// <param name="identifier">Type specific unique identify for object</param>
        /// <param name="loader">Delegate to invoke if cached item is not found</param>
        /// <param name="sliding">Sliding expiration to use if object is loaded and cached</param>
        /// <returns>Cached object or result of loader</returns>
        public static T GetOrSetByType<T>(this ICacheRepository repo, object identifier, Func<T> loader, TimeSpan sliding)
        {
            var key = CreateKey<T>(identifier);
            return repo.GetOrSet(key, loader, sliding);
        }

        /// <summary>
        /// Get or set by type.
        /// Cache key is automagically created from object type and identify.
        /// Example: repo.GetOrSetByType<User>(1, LoadUserByIdFromDb(1)); // Get or load and set User 1
        /// </summary>
        /// <typeparam name="T">Type of cached object</typeparam>
        /// <param name="repo">ICacheRepository</param>
        /// <param name="identifier">Type specific unique identify for object</param>
        /// <param name="loader">Delegate to invoke if cached item is not found</param>
        /// <param name="sliding">Sliding expiration to use if object is loaded and cached</param>
        /// <returns>Cached object or result of loader</returns>
        public static T GetOrSetByType<T>(this ICacheRepository repo, object identifier, Func<T> loader, Func<T, TimeSpan> sliding)
        {
            var key = CreateKey<T>(identifier);
            return repo.GetOrSet(key, loader, sliding);
        }

        /// <summary>
        /// Get or set by type.
        /// Cache key is automagically created from object type and identify.
        /// Cache absolute expiration is taken from the enum value or override by application configuration. 
        /// Example: repo.GetOrSetByType<User>(1, LoadUserByIdFromDb(1)); // Get or load and set User 1
        /// </summary>
        /// <typeparam name="T">Type of cached object</typeparam>
        /// <param name="repo">ICacheRepository</param>
        /// <param name="identifier">Type specific unique identify for object</param>
        /// <param name="loader">Delegate to invoke if cached item is not found</param>
        /// <param name="expiration">Abosolute expiration to use if object is loaded and cached</param>
        /// <returns>Cached object or result of loader</returns>
        public static T GetOrSetByType<T>(this ICacheRepository repo, object identifier, Func<T> loader, CacheExpiration expiration)
        {
            var key = CreateKey<T>(identifier);
            return repo.GetOrSet(key, loader, expiration);
        }

        /// <summary>
        /// Get or set by type.
        /// Cache key is automagically created from object type and identify.
        /// Cache absolute expiration is taken from the enum value or override by application configuration. 
        /// Example: repo.GetOrSetByType<User>(1, LoadUserByIdFromDb(1)); // Get or load and set User 1
        /// </summary>
        /// <typeparam name="T">Type of cached object</typeparam>
        /// <param name="repo">ICacheRepository</param>
        /// <param name="identifier">Type specific unique identify for object</param>
        /// <param name="loader">Delegate to invoke if cached item is not found</param>
        /// <param name="expiration">Abosolute expiration to use if object is loaded and cached</param>
        /// <returns>Cached object or result of loader</returns>
        public static T GetOrSetByType<T>(this ICacheRepository repo, object identifier, Func<T> loader, Func<T, CacheExpiration> expiration)
        {
            var key = CreateKey<T>(identifier);
            return repo.GetOrSet(key, loader, expiration);
        }

        /// <summary>
        /// Get or set by type.
        /// Cache key is automagically created from object type and identify.
        /// Cache sliding expiration is taken from the enum value or override by application configuration. 
        /// Example: repo.GetOrSetByType<User>(1, LoadUserByIdFromDb(1)); // Get or load and set User 1
        /// </summary>
        /// <typeparam name="T">Type of cached object</typeparam>
        /// <param name="repo">ICacheRepository</param>
        /// <param name="identifier">Type specific unique identify for object</param>
        /// <param name="loader">Delegate to invoke if cached item is not found</param>
        /// <param name="sliding">Sliding expiration to use if object is loaded and cached</param>
        /// <returns>Cached object or result of loader</returns>
        public static T GetOrSetByType<T>(this ICacheRepository repo, object identifier, Func<T> loader, CacheSliding sliding)
        {
            var key = CreateKey<T>(identifier);
            return repo.GetOrSet(key, loader, sliding);
        }

        /// <summary>
        /// Get or set by type.
        /// Cache key is automagically created from object type and identify.
        /// Cache sliding expiration is taken from the enum value or override by application configuration. 
        /// Example: repo.GetOrSetByType<User>(1, LoadUserByIdFromDb(1)); // Get or load and set User 1
        /// </summary>
        /// <typeparam name="T">Type of cached object</typeparam>
        /// <param name="repo">ICacheRepository</param>
        /// <param name="identifier">Type specific unique identify for object</param>
        /// <param name="loader">Delegate to invoke if cached item is not found</param>
        /// <param name="sliding">Sliding expiration to use if object is loaded and cached</param>
        /// <returns>Cached object or result of loader</returns>
        public static T GetOrSetByType<T>(this ICacheRepository repo, object identifier, Func<T> loader, Func<T, CacheSliding> sliding)
        {
            var key = CreateKey<T>(identifier);
            return repo.GetOrSet(key, loader, sliding);
        }

        #endregion

        #region SetByType

        /// <summary>
        /// Set by type.
        /// Cache key is automagically created from object type and identify.
        /// Example: repo.SetByType<User>(1, user); // Set user by their Id
        /// </summary>
        /// <typeparam name="T">Type of cached object</typeparam>
        /// <param name="repo">ICacheRepository</param>
        /// <param name="identifier">Type specific unique identify for object</param>
        /// <param name="value">Object to be cached</param>
        public static void SetByType<T>(this ICacheRepository repo, object identifier, T value)
        {
            var key = CreateKey<T>(identifier);
            repo.Set(key, value);
        }

        /// <summary>
        /// Set by type.
        /// Cache key is automagically created from object type and identify.
        /// Example: repo.SetByType<User>(1, user); // Set user by their Id
        /// </summary>
        /// <typeparam name="T">Type of cached object</typeparam>
        /// <param name="repo">ICacheRepository</param>
        /// <param name="identifier">Type specific unique identify for object</param>
        /// <param name="expiration">Abosolute expiration to use for cache</param>
        /// <param name="value">Object to be cached</param>
        public static void SetByType<T>(this ICacheRepository repo, object identifier, T value, DateTime expiration)
        {
            var key = CreateKey<T>(identifier);
            repo.Set(key, value, expiration);
        }
        
        /// <summary>
        /// Set by type.
        /// Cache key is automagically created from object type and identify.
        /// Example: repo.SetByType<User>(1, user); // Set user by their Id
        /// </summary>
        /// <typeparam name="T">Type of cached object</typeparam>
        /// <param name="repo">ICacheRepository</param>
        /// <param name="identifier">Type specific unique identify for object</param>
        /// <param name="sliding">Sliding expiration to use for cache</param>
        /// <param name="value">Object to be cached</param>
        public static void SetByType<T>(this ICacheRepository repo, object identifier, T value, TimeSpan sliding)
        {
            var key = CreateKey<T>(identifier);
            repo.Set(key, value, sliding);
        }

        /// <summary>
        /// Set by type.
        /// Cache key is automagically created from object type and identify.
        /// Cache absolute expiration is taken from the enum value or override by application configuration. 
        /// Example: repo.SetByType<User>(1, user); // Set user by their Id
        /// </summary>
        /// <typeparam name="T">Type of cached object</typeparam>
        /// <param name="repo">ICacheRepository</param>
        /// <param name="identifier">Type specific unique identify for object</param>
        /// <param name="expiration">Abosolute expiration to use for cache</param>
        /// <param name="value">Object to be cached</param>
        public static void SetByType<T>(this ICacheRepository repo, object identifier, T value, CacheExpiration expiration)
        {
            var key = CreateKey<T>(identifier);
            repo.Set(key, value, expiration);
        }
        
        /// <summary>
        /// Set by type.
        /// Cache key is automagically created from object type and identify.
        /// Cache sliding expiration is taken from the enum value or override by application configuration. 
        /// Example: repo.SetByType<User>(1, user); // Set user by their Id
        /// </summary>
        /// <typeparam name="T">Type of cached object</typeparam>
        /// <param name="repo">ICacheRepository</param>
        /// <param name="identifier">Type specific unique identify for object</param>
        /// <param name="sliding">Sliding expiration to use for cache</param>
        /// <param name="value">Object to be cached</param>
        public static void SetByType<T>(this ICacheRepository repo, object identifier, T value, CacheSliding sliding)
        {
            var key = CreateKey<T>(identifier);
            repo.Set(key, value, sliding);
        }

        #endregion

        #region Misc

        /// <summary>
        /// Get by type.
        /// Cache key is automagically created from object type and identify.
        /// Example: repo.GetByType<User>(1); // Get User 1
        /// </summary>
        /// <typeparam name="T">Type of cached object</typeparam>
        /// <param name="repo">ICacheRepository</param>
        /// <param name="identifier">Type specific unique identify for object</param>
        /// <returns>Cached object, null if no record found</returns>
        public static T GetByType<T>(this ICacheRepository repo, object identifier)
        {
            var key = CreateKey<T>(identifier);
            return repo.Get<T>(key);
        }

        /// <summary>
        /// Remove by type.
        /// Cache key is automagically created from object type and identify.
        /// Example: repo.RemoveByType<User>(1); // Removes user 1
        /// </summary>
        /// <typeparam name="T">Type of cached object</typeparam>
        /// <param name="repo">ICacheRepository</param>
        /// <param name="identifier">Type specific unique identify for object</param>
        public static void RemoveByType<T>(this ICacheRepository repo, object identifier)
        {
            var key = CreateKey<T>(identifier);
            repo.Remove(key);
        }

        private static string CreateKey<T>(object identifier)
        {
            var type = typeof(T);

            return identifier == null
                ? type.FullName
                : String.Concat(type.FullName, "_", identifier);
        }

        #endregion
    }
}