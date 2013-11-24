using System;
using System.Collections.Generic;
using System.Data.Common;
using SystemDot.Logging;
using SystemDot.Serialisation;
using SystemDot.Sql.Connections;
using SystemDot.Storage.Changes;
using SystemDot.Storage.Changes.Upcasting;

namespace SystemDot.Sql
{
    public abstract class DbChangeStore : ChangeStore
    {
        public static string ConnectionString { get; set; }

        readonly static object CreateDbLock = new object();

        protected DbChangeStore(ISerialiser serialiser, ChangeUpcasterRunner changeUpcasterRunner)
            : base(serialiser, changeUpcasterRunner)
        {
        }

        protected override void StoreChange(string changeRootId, Change change, Func<Change, byte[]> serialiseAction)
        {
            using (var connection = GetConnection())
            {
                CheckPointIfPossible(connection, changeRootId, change);
                StoreChange(connection, changeRootId, change, serialiseAction);
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

        void StoreChange(DbConnection connection, string changeRootId, Change change, Func<Change, byte[]> serialiseAction)
        {
            Logger.Debug("Storing change in sql for id {0}", changeRootId);
            
            connection.Execute(
                "insert into ChangeStore(changeRootId, change) values(@changeRootId, @change)",
                command =>
                {
                    AddParameter(command.Parameters, "@changeRootId", changeRootId);
                    AddParameter(command.Parameters, "@change", serialiseAction(change));
                });
        }

        protected override IEnumerable<Change> GetChanges(string changeRootId, Func<byte[], Change> deserialiseAction)
        {
            var messages = new List<Change>();

            using (var connection = GetConnection())
            {
                connection.ExecuteReader(
                    "select change from ChangeStore where changeRootId = '" + changeRootId + "' order by sequence ASC",
                    reader => messages.Add(deserialiseAction(reader.GetBytes(0))));
            }

            return messages;
        }

        public override void Initialise()
        {
            lock (CreateDbLock)
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