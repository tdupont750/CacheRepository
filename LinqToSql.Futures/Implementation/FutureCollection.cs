using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Linq;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;

namespace LinqToSql.Futures.Implementation
{
    public class FutureCollection : IFutureCollection
    {
        #region Private Classes

        private class BatchedQuery
        {
            public string Query { get; set; }

            public IList<SqlParameter> Parameters { get; set; }
        }

        #endregion

        #region IFutureCollection

        public IFutureQuery<T> Add<T>(IQueryable<T> query)
        {
            // Don't throw exception, must support non DataQuery types when Unit Testing.
            var type = query.GetType();
            // ReSharper disable PossibleNullReferenceException
            if (!type.FullName.StartsWith("System.Data.Linq.DataQuery"))
            // ReSharper restore PossibleNullReferenceException
                return new MockFutureQuery<T> { Value = query.ToList() };

            var dbCommand = _dataContext.GetCommand(query);
            var futureQuery = new FutureQuery<T>
            {
                Command = dbCommand
            };

            _futureQueries.Add(futureQuery);
            return futureQuery;
        }

        public void Execute(IsolationLevel issolationLevel = IsolationLevel.Unspecified)
        {
            var batchQuery = CreateBatchedQuery();

            ExecuteBatchedQuery(batchQuery, issolationLevel);

            _futureQueries.Clear();
        }

        public int Count
        {
            get { return _futureQueries.Count; }
        }

        #endregion

        #region Implementation

        private DataContext _dataContext;

        private IList<FutureQuery> _futureQueries;

        public FutureCollection(DataContext dataContext)
        {
            _dataContext = dataContext;
            _futureQueries = new List<FutureQuery>();
        }

        private BatchedQuery CreateBatchedQuery()
        {
            var batchedQuery = new StringBuilder();
            var batchedParameters = new List<SqlParameter>();

            foreach (var typedCommand in _futureQueries)
            {
                var commandQuery = new StringBuilder(typedCommand.Command.CommandText);
                var commandParameters = new List<SqlParameter>();
                var totalParameterCount = batchedParameters.Count + typedCommand.Command.Parameters.Count;

                for (var i = typedCommand.Command.Parameters.Count - 1; i >= 0; i--)
                {
                    totalParameterCount--;
                    var parameter = CloneSqlParameter((SqlParameter)typedCommand.Command.Parameters[i]);

                    var newName = "@x" + totalParameterCount
                        .ToString()
                        .PadLeft(4, '0');

                    commandQuery.Replace(parameter.ParameterName, newName);
                    parameter.ParameterName = newName;

                    commandParameters.Add(parameter);
                }

                var commandQueryString = commandQuery.ToString();
                batchedQuery.AppendLine(commandQueryString);
                batchedQuery.AppendLine();

                commandParameters.Reverse();
                batchedParameters.AddRange(commandParameters);

                if (batchedParameters.Count > 2100)
                    throw new InvalidOperationException("SQL commands can not have more than 2100 parameters");
            }

            var batchQueryString = batchedQuery.ToString();
            return new BatchedQuery
            {
                Parameters = batchedParameters,
                Query = batchQueryString
            };
        }

        private static SqlParameter CloneSqlParameter(SqlParameter parameter)
        {
            return new SqlParameter
            {
                DbType = parameter.DbType,
                Direction = parameter.Direction,
                IsNullable = parameter.IsNullable,
                LocaleId = parameter.LocaleId,
                Offset = parameter.Offset,
                ParameterName = parameter.ParameterName,
                Precision = parameter.Precision,
                Scale = parameter.Scale,
                Size = parameter.Size,
                SourceColumn = parameter.SourceColumn,
                SourceColumnNullMapping = parameter.SourceColumnNullMapping,
                SourceVersion = parameter.SourceVersion,
                SqlDbType = parameter.SqlDbType,
                SqlValue = parameter.SqlValue,
                TypeName = parameter.TypeName,
                UdtTypeName = parameter.UdtTypeName,
                Value = parameter.Value,
                XmlSchemaCollectionDatabase = parameter.XmlSchemaCollectionDatabase,
                XmlSchemaCollectionName = parameter.XmlSchemaCollectionName,
                XmlSchemaCollectionOwningSchema = parameter.XmlSchemaCollectionOwningSchema
            };
        }

        private void ExecuteBatchedQuery(BatchedQuery batchedQuery, IsolationLevel issolationLevel)
        {
            DbTransaction transaction = null;

            try
            {
                if (_dataContext.Transaction == null)
                {
                    _dataContext.Connection.Open();
                    transaction = _dataContext.Transaction = _dataContext.Connection.BeginTransaction(issolationLevel);
                }

                using (var command = new SqlCommand
                {
                    CommandText = batchedQuery.Query,
                    Connection = (SqlConnection)_dataContext.Connection,
                    Transaction = (SqlTransaction)_dataContext.Transaction
                })
                {
                    foreach (var parameter in batchedQuery.Parameters)
                        command.Parameters.Add(parameter);

                    using (var reader = command.ExecuteReader())
                    using (var multipleResultSets = _dataContext.Translate(reader))
                    {
                        foreach (var futureCommand in _futureQueries)
                        {
                            var genericMethod = GetResultMethod.MakeGenericMethod(futureCommand.Type);
                            var result = genericMethod.Invoke(this, new[] { multipleResultSets }) as IList;
                            futureCommand.SetValue(result);
                        }
                    }
                }
            }
            finally
            {
                if (transaction != null)
                    transaction.Dispose();
            }
        }

        // ReSharper disable UnusedMember.Local
        private static IList<T> GetResult<T>(IMultipleResults multipleResults)
        // ReSharper restore UnusedMember.Local
        {
            return multipleResults
                .GetResult<T>()
                .ToList();
        }
        
        private static readonly MethodInfo GetResultMethod = typeof(FutureCollection).GetMethod("GetResult", BindingFlags.NonPublic | BindingFlags.Static);

        #endregion

        #region IDisposable

        private bool _isDisposed;

        ~FutureCollection()
        {
            Dispose(true);
        }

        public void Dispose()
        {
            Dispose(false);
        }

        private void Dispose(bool isFinalizing)
        {
            if (_isDisposed)
                return;

            if (_futureQueries != null)
            {
                _futureQueries.Clear();
                _futureQueries = null;
            }

            _dataContext = null;

            if (!isFinalizing)
                GC.SuppressFinalize(this);

            _isDisposed = true;
        }

        #endregion
    }
}