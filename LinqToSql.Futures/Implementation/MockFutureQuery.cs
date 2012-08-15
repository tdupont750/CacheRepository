using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LinqToSql.Futures.Implementation
{
    public class MockFutureQuery<T> : IFutureQuery<T>
    {
        public IList<T> Value { get; set; }

        public bool IsValueLoaded
        {
            get { return true; }
        }
    }
}
