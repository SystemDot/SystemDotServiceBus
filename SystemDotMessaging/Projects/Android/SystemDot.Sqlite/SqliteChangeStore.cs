using System.Data.Common;
using SystemDot.Serialisation;
using SystemDot.Sql;
using SystemDot.Storage.Changes.Upcasting;
using Mono.Data.Sqlite;

namespace SystemDot.Sqlite
{
    public class SqliteChangeStore : DbChangeStore
    {
        public SqliteChangeStore(ISerialiser serialiser, ChangeUpcasterRunner changeUpcasterRunner)
            : base(serialiser, changeUpcasterRunner)
        {
        }

        protected override string GetInitialisationSql() 
        {
            return @"
CREATE TABLE IF NOT EXISTS ChangeStore(
    ChangeRootId nvarchar(1000) NOT NULL,
    Sequence INTEGER PRIMARY KEY,
    Change varbinary(2147483647) NULL)";
        }

        public override void AddParameter(DbParameterCollection collection, string name, object value)
        {
            collection.Add(new SqliteParameter(name, value));
        }

        protected override DbConnection CreateConnection()
        {
            return new SqliteConnection(ConnectionString);
        }
    }
}