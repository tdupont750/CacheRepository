﻿using System;

namespace CacheRepository
{
    /// <summary>
    /// Generic Caching Mechanism
    /// </summary>
    [Obsolete]
    public interface ICacheRepository
    {
        /// <summary>
        /// Get by key
        /// </summary>
        /// <typeparam name="T">Type of the cached object</typeparam>
        /// <param name="key">Cache key</param>
        /// <returns>Cached object, default if no record found</returns>
        T Get<T>(string key);

        /// <summary>
        /// Get or set by key.
        /// If key is not found, loader is invoked and the result is cached under the specified key.
        /// </summary>
        /// <typeparam name="T">Type of the cached object</typeparam>
        /// <param name="key">Cache key</param>
        /// <param name="loader">Delegate to invoke if cached item is not found</param>
        /// <returns>Cached object or result of loader</returns>
        T GetOrSet<T>(string key, Func<T> loader);

        /// <summary>
        /// Get or set by key.
        /// If key is not found, loader is invoked and the result is cached under the specified key.
        /// </summary>
        /// <typeparam name="T">Type of the cached object</typeparam>
        /// <param name="key">Cache key</param>
        /// <param name="loader">Delegate to invoke if cached item is not found</param>
        /// <param name="expiration">Abosolute expiration to use if object is loaded and cached</param>
        /// <returns>Cached object or result of loader</returns>
        T GetOrSet<T>(string key, Func<T> loader, DateTime expiration);

        /// <summary>
        /// Get or set by key.
        /// If key is not found, loader is invoked and the result is cached under the specified key.
        /// </summary>
        /// <typeparam name="T">Type of the cached object</typeparam>
        /// <param name="key">Cache key</param>
        /// <param name="loader">Delegate to invoke if cached item is not found</param>
        /// <param name="expiration">Abosolute expiration to use if object is loaded and cached</param>
        /// <returns>Cached object or result of loader</returns>
        T GetOrSet<T>(string key, Func<T> loader, Func<T, DateTime> expiration);

        /// <summary>
        /// Get or set by key.
        /// If key is not found, loader is invoked and the result is cached under the specified key.
        /// </summary>
        /// <typeparam name="T">Type of the cached object</typeparam>
        /// <param name="key">Cache key</param>
        /// <param name="loader">Delegate to invoke if cached item is not found</param>
        /// <param name="sliding">Sliding expiration to use if object is loaded and cached</param>
        /// <returns>Cached object or result of loader</returns>
        T GetOrSet<T>(string key, Func<T> loader, TimeSpan sliding);

        /// <summary>
        /// Get or set by key.
        /// If key is not found, loader is invoked and the result is cached under the specified key.
        /// </summary>
        /// <typeparam name="T">Type of the cached object</typeparam>
        /// <param name="key">Cache key</param>
        /// <param name="loader">Delegate to invoke if cached item is not found</param>
        /// <param name="sliding">Sliding expiration to use if object is loaded and cached</param>
        /// <returns>Cached object or result of loader</returns>
        T GetOrSet<T>(string key, Func<T> loader, Func<T, TimeSpan> sliding);

        /// <summary>
        /// Get or set by key.
        /// If key is not found, loader is invoked and the result is cached under the specified key.
        /// Cache absolute expiration is taken from the enum value or override by application configuration. 
        /// </summary>
        /// <typeparam name="T">Type of the cached object</typeparam>
        /// <param name="key">Cache key</param>
        /// <param name="loader">Delegate to invoke if cached item is not found</param>
        /// <param name="expiration">Abosolute expiration to use if object is loaded and cached</param>
        /// <returns>Cached object or result of loader</returns>
        T GetOrSet<T>(string key, Func<T> loader, CacheExpiration expiration);

        /// <summary>
        /// Get or set by key.
        /// If key is not found, loader is invoked and the result is cached under the specified key.
        /// Cache absolute expiration is taken from the enum value or override by application configuration. 
        /// </summary>
        /// <typeparam name="T">Type of the cached object</typeparam>
        /// <param name="key">Cache key</param>
        /// <param name="loader">Delegate to invoke if cached item is not found</param>
        /// <param name="expiration">Abosolute expiration to use if object is loaded and cached</param>
        /// <returns>Cached object or result of loader</returns>
        T GetOrSet<T>(string key, Func<T> loader, Func<T, CacheExpiration> expiration);

        /// <summary>
        /// Get or set by key.
        /// If key is not found, loader is invoked and the result is cached under the specified key.
        /// Cache sliding expiration is taken from the enum value or override by application configuration. 
        /// </summary>
        /// <typeparam name="T">Type of the cached object</typeparam>
        /// <param name="key">Cache key</param>
        /// <param name="loader">Delegate to invoke if cached item is not found</param>
        /// <param name="sliding">Sliding expiration to use if object is loaded and cached</param>
        /// <returns>Cached object or result of loader</returns>
        T GetOrSet<T>(string key, Func<T> loader, CacheSliding sliding);

        /// <summary>
        /// Get or set by key.
        /// If key is not found, loader is invoked and the result is cached under the specified key.
        /// Cache sliding expiration is taken from the enum value or override by application configuration. 
        /// </summary>
        /// <typeparam name="T">Type of the cached object</typeparam>
        /// <param name="key">Cache key</param>
        /// <param name="loader">Delegate to invoke if cached item is not found</param>
        /// <param name="sliding">Sliding expiration to use if object is loaded and cached</param>
        /// <returns>Cached object or result of loader</returns>
        T GetOrSet<T>(string key, Func<T> loader, Func<T, CacheSliding> sliding);

        /// <summary>
        /// Set by key
        /// </summary>
        /// <param name="key">Cache key</param>
        /// <param name="value">Value to be cached</param>
        void Set<T>(string key, T value);

        /// <summary>
        /// Set by key
        /// </summary>
        /// <param name="key">Cache key</param>
        /// <param name="value">Value to be cached</param>
        /// <param name="expiration">Abosolute expiration to use for cache</param>
        void Set<T>(string key, T value, DateTime expiration);

        /// <summary>
        /// Set by key
        /// </summary>
        /// <param name="key">Cache key</param>
        /// <param name="value">Value to be cached</param>
        /// <param name="sliding">Sliding expiration to use for cache</param>
        void Set<T>(string key, T value, TimeSpan sliding);

        /// <summary>
        /// Set by key
        /// Cache absolute expiration is taken from the enum value or override by application configuration. 
        /// </summary>
        /// <param name="key">Cache key</param>
        /// <param name="value">Value to be cached</param>
        /// <param name="expiration">Abosolute expiration to use for cache</param>
        void Set<T>(string key, T value, CacheExpiration expiration);

        /// <summary>
        /// Set by key
        /// Cache sliding expiration is taken from the enum value or override by application configuration. 
        /// </summary>
        /// <param name="key">Cache key</param>
        /// <param name="value">Value to be cached</param>
        /// <param name="sliding">Sliding expiration to use for cache</param>
        void Set<T>(string key, T value, CacheSliding sliding);

        /// <summary>
        /// Remove by key
        /// </summary>
        /// <param name="key">Cache key</param>
        void Remove(string key);

        /// <summary>
        /// Clear all cache
        /// </summary>
        void ClearAll();
    }
}