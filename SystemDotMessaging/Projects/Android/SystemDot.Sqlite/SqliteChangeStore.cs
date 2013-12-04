using System;
using System.Data.Common;
using SystemDot.Serialisation;
using SystemDot.Sql;
using SystemDot.Storage.Changes;
using SystemDot.Storage.Changes.Upcasting;
using Mono.Data.Sqlite;

namespace SystemDot.Sqlite
{
    public class SqliteChangeStore : DbChangeStore
    {
        readonly static object WriteLock = new object();

        public SqliteChangeStore(ISerialiser serialiser, ChangeUpcasterRunner changeUpcasterRunner)
            : base(serialiser, changeUpcasterRunner)
        {
        }

        protected override void StoreChange(string changeRootId, Change change, Func<Change, byte[]> serialiseAction)
        {
            lock (WriteLock)
            {
                base.StoreChange(changeRootId, change, serialiseAction);
            }
        }

        protected override string GetInitialisationSql() 
        {
            return @"
CREATE TABLE IF NOT EXISTS ChangeStore(
    ChangeRootId nvarchar(1000) NOT NULL,
    Sequence INTEGER PRIMARY KEY,
    Change varbinary(2147483647) NULL)";
        }

        protected override void AddParameter(DbParameterCollection collection, string name, object value)
        {
            collection.Add(new SqliteParameter(name, value));
        }

        protected override DbConnection CreateConnection()
        {
            return new SqliteConnection(ConnectionString);
        }
    }
}