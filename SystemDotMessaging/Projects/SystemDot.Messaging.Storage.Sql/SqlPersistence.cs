using System;
using System.Collections.Generic;
using System.Data.SqlServerCe;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.Channels.Addressing;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Storage.Sql.Connections;
using SystemDot.Serialisation;

namespace SystemDot.Messaging.Storage.Sql
{
    public class SqlPersistence : IPersistence
    {
        readonly ISerialiser serialiser;

        public EndpointAddress Address { get; private set; }
        public PersistenceUseType UseType { get; private set; }

        public SqlPersistence(
            ISerialiser serialiser, 
            PersistenceUseType useType, 
            EndpointAddress address)
        {
            Contract.Requires(serialiser != null);
            Contract.Requires(address != null);
            Contract.Requires(address != EndpointAddress.Empty);

            this.serialiser = serialiser;
            UseType = useType;
            Address = address;
            Initialise();
        }

        public IEnumerable<MessagePayload> GetMessages()
        {
            var messages = new List<MessagePayload>();

            using (var connection = new PooledConnection())
            {
                connection.ExecuteReader(
                    "select id, createdon, headers from MessagePayloadStorageItem where address = '" + Address + "'",
                    reader => messages.Add(
                        new MessagePayload
                        {
                            Id = reader.GetGuid(0),
                            CreatedOn = reader.GetDateTime(1),
                            Headers = this.serialiser.Deserialise(reader.GetBytes(2)).As<List<IMessageHeader>>()
                        }));
            }

            return messages;
        }

        public void AddMessageAndIncrementSequence(MessagePayload message)
        {
            using (var connection = new PooledConnection())
            {
                using (SqlCeTransaction transaction = connection.Connection.BeginTransaction())
                {
                    IncrementSequence(connection);
                    AddMessage(message, connection);
                    transaction.Commit();
                }
            }
        }

        void IncrementSequence(PooledConnection connection)
        {
            connection.Execute(
                "update MessageSequence set sequencenumber = sequencenumber + 1 where address = @address and type = @type",
                command =>
                {
                    command.Parameters.AddWithValue("@address", Address.ToString());
                    command.Parameters.AddWithValue("@type", UseType);
                });
        }

        public void AddMessage(MessagePayload message)
        {
            using (var connection = new PooledConnection())
            {
                AddMessage(message, connection);
            }
        }

        void AddMessage(MessagePayload message, PooledConnection connection)
        {
            connection.Execute(
                "insert into MessagePayloadStorageItem(id, createdon, headers, address, type) values(@id, @createdon, @headers, @address, @type)",
                command =>
                {
                    command.Parameters.AddWithValue("@id", message.Id);
                    command.Parameters.AddWithValue("@createdon", message.CreatedOn);
                    command.Parameters.AddWithValue("@headers", this.serialiser.Serialise(message.Headers));
                    command.Parameters.AddWithValue("@address", Address.ToString());
                    command.Parameters.AddWithValue("@type", UseType);
                });
        }

        public void UpdateMessage(MessagePayload message)
        {
            using (var connection = new PooledConnection())
            {
                connection.Execute(
                    "update MessagePayloadStorageItem set headers = @headers where id = @id and address = @address and type = @type",
                    command =>
                    {
                        command.Parameters.AddWithValue("@id", message.Id);
                        command.Parameters.AddWithValue("@headers", this.serialiser.Serialise(message.Headers));
                        command.Parameters.AddWithValue("@address", Address.ToString());
                        command.Parameters.AddWithValue("@type", UseType);
                    });
            }
        }

        public int GetSequence()
        {
            using (var connection = new PooledConnection())
            {
                return connection.ExecuteScalar<int>(
                    "select sequencenumber from MessageSequence where address = @address and type = @type",
                    command =>
                    {
                        command.Parameters.AddWithValue("@address", Address.ToString());
                        command.Parameters.AddWithValue("@type", UseType);
                    })  ;
            }
        }
        
        public void SetSequence(int toSet)
        {
            using (var connection = new PooledConnection())
            {
                connection.Execute(
                    "update MessageSequence set sequencenumber = @sequence where address = @address and type = @type",
                    command =>
                    {
                        command.Parameters.AddWithValue("@sequence", toSet);
                        command.Parameters.AddWithValue("@address", Address.ToString());
                        command.Parameters.AddWithValue("@type", UseType);
                    });
            }
        }

        public void Delete(Guid id)
        {
            using (var connection = new PooledConnection())
            {
                connection.Execute(
                    "delete from MessagePayloadStorageItem where id = @id and address = @address and type = @type",
                    command =>
                    {
                        command.Parameters.AddWithValue("@id", id);
                        command.Parameters.AddWithValue("@address", Address.ToString());
                        command.Parameters.AddWithValue("@type", UseType);
                    });
            }
        }

        public void Initialise()
        {
            using (var connection = new PooledConnection())
            {
                if (connection.ExecuteScalar<int>(
                    "select count(*) from MessageSequence where address = @address and type = @type",
                    command =>
                    {
                        command.Parameters.AddWithValue("@address", Address.ToString());
                        command.Parameters.AddWithValue("@type", UseType);
                    }) > 0) return;

                connection.Execute(
                    "insert into MessageSequence(address, sequencenumber, type) values(@address, 1, @type)",
                    command =>
                    {
                        command.Parameters.AddWithValue("@address", Address.ToString());
                        command.Parameters.AddWithValue("@type", UseType);
                    });
            }
        }
    }
}
