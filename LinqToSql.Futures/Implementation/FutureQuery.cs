using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace LinqToSql.Futures.Implementation
{
    public class FutureQuery<T> : FutureQuery, IFutureQuery<T>
    {
        public IList<T> Value { get; private set; }

        public bool IsValueLoaded { get; private set; }

        public override IDbCommand Command { get; set; }

        public override Type Type
        {
            get { return typeof(T); }
        }

        public override void SetValue(IList results)
        {
            Value = results as IList<T>;
            IsValueLoaded = true;
        }
    }

    public abstract class FutureQuery
    {
        public abstract IDbCommand Command { get; set; }

        public abstract Type Type { get; }

        public abstract void SetValue(IList results);
    }
}
