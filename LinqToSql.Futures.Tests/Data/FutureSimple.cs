using LinqToSql.Futures.Implementation;

namespace LinqToSql.Futures.Tests.Data
{
    partial class FutureSimpleDataContext : IFutureDataContext
    {
        protected override void Dispose(bool disposing)
        {
            if (_futureCollection != null)
            {
                _futureCollection.Dispose();
                _futureCollection = null;
            }

            base.Dispose(disposing);
        }

        private IFutureCollection _futureCollection;
        public IFutureCollection FutureCollection
        {
            get
            {
                if (_futureCollection == null)
                    _futureCollection = this.CreateFutureCollection();

                return _futureCollection;
            }
        }
    }
}
