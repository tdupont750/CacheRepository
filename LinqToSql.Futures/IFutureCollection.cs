using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace LinqToSql.Futures
{
    public interface IFutureCollection : IDisposable
    {
        IFutureQuery<T> Add<T>(IQueryable<T> query);

        void Execute(IsolationLevel issolationLevel = IsolationLevel.Unspecified);

        int Count { get; }
    }
}