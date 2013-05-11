using System.Collections.Generic;
using System.Data.SqlClient;
using SystemDot.Logging;
using SystemDot.Serialisation;
using SystemDot.Sql.Connections;
using SystemDot.Storage.Changes;

namespace SystemDot.Sql
{
    public class SqlChangeStore : Disposable, IChangeStore
    {
        readonly ISerialiser serialiser;
        readonly static object createDbLock = new object();

        public SqlChangeStore(ISerialiser serialiser)
        {
            this.serialiser = serialiser;
        }

        public void StoreChange(string changeRootId, Change change)
        {
            using (var connection = ConnectionHelper.GetConnection())
            {
                CheckPointIfPossible(connection, changeRootId, change);
                StoreChange(connection, changeRootId, change);
            }
        }

        void CheckPointIfPossible(SqlConnection connection, string changeRootId, Change change)
        {
            if (change is CheckPointChange)
            {
                DeleteChanges(connection, changeRootId);
            }
        }

        void DeleteChanges(SqlConnection connection, string changeRootId)
        {
            connection.Execute(
                "delete from ChangeStore where changeRootId = @changeRootId",
                command => command.Parameters.AddWithValue("@changeRootId", changeRootId));
        }

        void StoreChange(SqlConnection connection, string changeRootId, Change change)
        {
            Logger.Debug("Storing change in sql");
            
            connection.Execute(
                "insert into ChangeStore(changeRootId, change) values(@changeRootId, @change)",
                command =>
                {
                    command.Parameters.AddWithValue("@changeRootId", changeRootId);
                    command.Parameters.AddWithValue("@change", this.serialiser.Serialise(change));
                });
        }

        public IEnumerable<Change> GetChanges(string changeRootId)
        {
            var messages = new List<Change>();

            using (var connection = ConnectionHelper.GetConnection())
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

        public void Initialise(string location)
        {
            lock (createDbLock)
            {
                ConnectionHelper.ConnectionString = location;
                CreateInitialDatabaseObjects();
            }
        }

        void CreateInitialDatabaseObjects()
        {
            using (var connection = ConnectionHelper.GetConnection())
            {
                connection.Execute(
                    @"IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'ChangeStore') AND type in (N'U'))
                        CREATE TABLE ChangeStore(
                            ChangeRootId nvarchar(1000) NOT NULL,
                            Sequence int IDENTITY(1, 1) NOT NULL,
                            Change varbinary(max) NULL)",
                    command => { });
            }
        }
    }
}
