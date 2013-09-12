using System.Collections.Generic;
using System.Data.Common;
using SystemDot.Logging;
using SystemDot.Serialisation;
using SystemDot.Sql.Connections;
using SystemDot.Storage.Changes;

namespace SystemDot.Sql
{
    public abstract class DbChangeStore : IChangeStore
    {
        public static string ConnectionString { get; set; }

        readonly ISerialiser serialiser;
        readonly static object createDbLock = new object();

        protected DbChangeStore(ISerialiser serialiser)
        {
            this.serialiser = serialiser;
        }

        public void StoreChange(string changeRootId, Change change)
        {
            using (var connection = GetConnection())
            {
                CheckPointIfPossible(connection, changeRootId, change);
                StoreChange(connection, changeRootId, change);
            }
        }

        void CheckPointIfPossible(DbConnection connection, string changeRootId, Change change)
        {
            if (change is CheckPointChange)
            {
                DeleteChanges(connection, changeRootId);
            }
        }

        void DeleteChanges(DbConnection connection, string changeRootId)
        {
            connection.Execute(
                "delete from ChangeStore where changeRootId = @changeRootId",
                command => AddParameter(command.Parameters, "@changeRootId", changeRootId));
        }

        void StoreChange(DbConnection connection, string changeRootId, Change change)
        {
            Logger.Debug("Storing change in sql for id {0}", changeRootId);
            
            connection.Execute(
                "insert into ChangeStore(changeRootId, change) values(@changeRootId, @change)",
                command =>
                {
                    AddParameter(command.Parameters, "@changeRootId", changeRootId);
                    AddParameter(command.Parameters, "@change", this.serialiser.Serialise(change));
                });
        }

        public IEnumerable<Change> GetChanges(string changeRootId)
        {
            var messages = new List<Change>();

            using (var connection = GetConnection())
            {
                connection.ExecuteReader(
                    "select change from ChangeStore where changeRootId = '" + changeRootId + "' order by sequence ASC",
                    reader => messages.Add(DeserialiseChange(reader)));
            }

            return messages;
        }

        Change DeserialiseChange(DbDataReader reader)
        {
            return this.serialiser.Deserialise(reader.GetBytes(0)).As<Change>();
        }

        public void Initialise()
        {
            lock (createDbLock)
            {
                using (var connection = GetConnection())
                {
                    connection.Execute(GetInitialisationSql(), command => { });
                }
            }
        }

        protected abstract string GetInitialisationSql();

        public abstract void AddParameter(DbParameterCollection collection, string name, object value);

        public DbConnection GetConnection()
        {
            DbConnection connection = CreateConnection();
            connection.Open();
            return connection;
        }

        protected abstract DbConnection CreateConnection();
    }
}