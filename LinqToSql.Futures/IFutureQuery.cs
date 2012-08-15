using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LinqToSql.Futures
{
    public interface IFutureQuery<T>
    {
        IList<T> Value { get; }

        bool IsValueLoaded { get; }
    }
}
