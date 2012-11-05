using System;
using System.Collections.Generic;
using System.Data.SqlServerCe;
using SystemDot.Messaging.Storage.Changes;
using SystemDot.Messaging.Storage.Sql.Connections;
using SystemDot.Serialisation;

namespace SystemDot.Messaging.Storage.Sql
{
    public class SqlChangeStore : IChangeStore
    {
        readonly ISerialiser serialiser;

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

        Change DeserialiseChange(SqlCeDataReader reader)
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
    }
}
