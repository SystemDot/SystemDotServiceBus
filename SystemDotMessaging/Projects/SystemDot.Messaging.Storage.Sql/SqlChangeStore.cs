using System.Collections.Generic;
using System.Data.SqlClient;
using SystemDot.Messaging.Storage.Changes;
using SystemDot.Messaging.Storage.Sql.Connections;
using SystemDot.Serialisation;

namespace SystemDot.Messaging.Storage.Sql
{
    public class SqlChangeStore : Disposable, IChangeStore
    {
        readonly ISerialiser serialiser;
        readonly static object CreateDbLock = new object();

        public SqlChangeStore(ISerialiser serialiser)
        {
            this.serialiser = serialiser;
        }

        public void StoreChange(string changeRootId, Change change)
        {
            using (var connection = new PooledConnection())
            {
                connection.Execute(
                    "insert into ChangeStore(changeRootId, change) values(@changeRootId, @change)",
                    command =>
                    {
                        command.Parameters.AddWithValue("@changeRootId", changeRootId);
                        command.Parameters.AddWithValue("@change", this.serialiser.Serialise(change));
                    });
            }
        }

        public IEnumerable<Change> GetChanges(string changeRootId)
        {
            var messages = new List<Change>();

            using (var connection = new PooledConnection())
            {
                connection.ExecuteReader(
                    "select change from ChangeStore where changeRootId = '" + changeRootId + "' order by sequence ASC",
                    reader => messages.Add(DeserialiseChange(reader)));
            }

            return messages;
        }

        public void CheckPoint(string changeRootId, Change change)
        {
        }

        Change DeserialiseChange(SqlDataReader reader)
        {
            return this.serialiser.Deserialise(reader.GetBytes(0)).As<Change>();
        }

        public void Initialise(string location)
        {
            lock (CreateDbLock)
            {
                ConnectionPool.Clear();
                ConnectionHelper.ConnectionString = location;

                CreateInitialDatabaseObjects();
            }
        }

        void CreateInitialDatabaseObjects()
        {
            using (var connection = new PooledConnection())
            {
                connection.Execute(
                    @"IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'ChangeStore') AND type in (N'U'))
                        CREATE TABLE ChangeStore(
                            ChangeRootId nvarchar(1000) NOT NULL,
                            Sequence int IDENTITY(1, 1) NOT NULL,
                            Change varbinary(8000) NULL)",
                    command => { });
            }
        }
    }
}
