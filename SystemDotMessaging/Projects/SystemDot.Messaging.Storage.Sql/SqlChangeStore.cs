using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlServerCe;
using SystemDot.Messaging.Storage.Changes;
using SystemDot.Messaging.Storage.Sql.Connections;
using SystemDot.Serialisation;

namespace SystemDot.Messaging.Storage.Sql
{
    public class SqlChangeStore : IChangeStore
    {
        readonly ISerialiser serialiser;
        readonly static object CreateDbLock = new object();

        public SqlChangeStore(ISerialiser serialiser)
        {
            this.serialiser = serialiser;
        }

        public Guid StoreChange(string changeRootId, Change change)
        {
            var id = Guid.NewGuid();

            using (var connection = new PooledConnection())
            {
                connection.Execute(
                    "insert into ChangeStore(id, changeRootId, change) values(@id, @changeRootId, @change)",
                    command =>
                    {
                        command.Parameters.AddWithValue("@id", id);
                        command.Parameters.AddWithValue("@changeRootId", changeRootId);
                        command.Parameters.AddWithValue("@change", this.serialiser.Serialise(change));
                    });
            }

            return id;
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

        Change DeserialiseChange(SqlDataReader reader)
        {
            return this.serialiser.Deserialise(reader.GetBytes(0)).As<Change>();
        }

        public Change GetChange(Guid id)
        {
            Change change = null;
            
            using (var connection = new PooledConnection())
            {
                connection.ExecuteReader(
                    "select change from ChangeStore where id = '" + id + "'",
                    reader => change = DeserialiseChange(reader));
            }

            return change;
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
                    @"CREATE TABLE ChangeStore(
                        Id uniqueidentifier NOT NULL CONSTRAINT ChangeStore_PK PRIMARY KEY,
                        ChangeRootId nvarchar(1000) NOT NULL,
                        Change varbinary(8000) NULL,
                        Sequence int IDENTITY(1, 1) NOT NULL)",
                    command => { });
            }
        }
    }
}
