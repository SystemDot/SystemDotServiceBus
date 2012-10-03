using System;
using System.Collections.Generic;
using System.Data.SqlServerCe;
using SystemDot.Messaging.Channels;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Channels.Packaging.Headers;
using SystemDot.Serialisation;

namespace SystemDot.Messaging.Storage.Sql
{
    public class SqlPersistence : IPersistence
    {
        readonly ISerialiser serialiser;

        public SqlPersistence(ISerialiser serialiser)
        {
            this.serialiser = serialiser;
        }

        public IEnumerable<MessagePayload> GetMessages(EndpointAddress address)
        {
            var messages = new List<MessagePayload>();

            using (SqlCeConnection connection = GetConnection())
            {
                connection.ExecuteReader(
                    "select id, createdon, headers from MessagePayloadStorageItem where and address = '" + address + "'",
                    reader => messages.Add(new MessagePayload
                    {
                        Id = reader.GetGuid(0),
                        CreatedOn = reader.GetDateTime(1),
                        Headers = this.serialiser.Deserialise(reader.GetBytes(2)).As<List<IMessageHeader>>()
                    }));
            }

            return messages;
        }

        static SqlCeConnection GetConnection()
        {
            var connection = new SqlCeConnection("Data Source=|DataDirectory|\\Messaging.sdf");
            connection.Open();
            return connection;
        }

        public void AddMessage(MessagePayload message, EndpointAddress address)
        {
            using (SqlCeConnection connection = GetConnection())
            {
                using (SqlCeTransaction transaction = connection.BeginTransaction())
                {
                    connection.Execute(
                        "update MessageSequence set sequencenumber = sequencenumber + 1 where address = @address",
                        command => command.Parameters.AddWithValue("@address", address.ToString()));

                    connection.Execute(
                        "insert into MessagePayloadStorageItem(id, createdon, headers, address) values(@id, @createdon, @headers, @address)",
                        command =>
                        {
                            command.Parameters.AddWithValue("@id", message.Id);
                            command.Parameters.AddWithValue("@createdon", message.CreatedOn);
                            command.Parameters.AddWithValue("@headers", this.serialiser.Serialise(message.Headers));
                            command.Parameters.AddWithValue("@address", message.GetFromAddress().ToString());
                        });

                    transaction.Commit();
                }
            }
        }

        public void UpdateMessage(MessagePayload message)
        {
            using (SqlCeConnection connection = GetConnection())
            {
                connection.Execute(
                    "update MessagePayloadStorageItem set @headers = headers where id = @id",
                    command =>
                    {
                        command.Parameters.AddWithValue("@id", message.Id);
                        command.Parameters.AddWithValue("@headers", this.serialiser.Serialise(message.Headers));
                    });
            }
        }

        public void RemoveMessage(Guid id)
        {
            using (SqlCeConnection connection = GetConnection())
            {
                connection.Execute(
                    "delete from MessagePayloadStorageItem where id = @id",
                    command => command.Parameters.AddWithValue("@id", id));
            }
        }

        public int GetNextSequence(EndpointAddress address)
        {
            using (SqlCeConnection connection = GetConnection())
            {
                return connection.ExecuteScalar<int>(
                    "select sequencenumber from MessageSequence where address = @address",
                    command => command.Parameters.AddWithValue("@address", address.ToString()));
            }
        }

        public void InitialiseChannel(EndpointAddress address)
        {
            using (SqlCeConnection connection = GetConnection())
            {
                if (connection.ExecuteScalar<int>(
                    "select count(*) from MessageSequence where address = @address",
                    command => command.Parameters.AddWithValue("@address", address.ToString())) > 0) return;

                connection.Execute(
                    "insert into MessageSequence(address, sequencenumber) values(@address, 1)",
                    command => command.Parameters.AddWithValue("@address", address.ToString()));
            }
        }
    }
}
