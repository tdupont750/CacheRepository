using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Reflection;
using LinqToSql.Futures.Implementation;

namespace LinqToSql.Futures
{
    public static class FutureExtensions
    {
        public static IFutureCollection CreateFutureCollection(this DataContext dataContext)
        {
            return new FutureCollection(dataContext);
        }

        public static Lazy<IList<T>> ToFuture<T>(this IQueryable<T> query)
        {
            var field = query
                .GetType()
                .GetField("context", BindingFlags.NonPublic | BindingFlags.Instance);

            if (field == null)
                return new Lazy<IList<T>>(query.ToList);

            var dataContext = field.GetValue(query) as IFutureDataContext;

            if (dataContext == null)
                throw new InvalidCastException("DataContext does not implement IFutureDataContext");

            return query.ToFuture(dataContext);
        }

        public static Lazy<IList<T>> ToFuture<T>(this IQueryable<T> query, IFutureDataContext dataContext)
        {
            return query.ToFuture(dataContext.FutureCollection);
        }

        public static Lazy<IList<T>> ToFuture<T>(this IQueryable<T> query, IFutureCollection futureCollection)
        {
            return futureCollection
                .Add(query)
                .ToLazy(futureCollection);
        }

        public static Lazy<IList<T>> ToLazy<T>(this IFutureQuery<T> futureQuery, IFutureCollection futureCollection)
        {
            return new Lazy<IList<T>>(() =>
            {
                if (!futureQuery.IsValueLoaded)
                    futureCollection.Execute();

                return futureQuery.Value;
            });
        }
    }
}