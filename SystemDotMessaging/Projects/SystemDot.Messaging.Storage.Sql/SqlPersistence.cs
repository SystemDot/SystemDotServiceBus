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
                    "select id, createdon, headers from MessagePayloadStorageItem where old = 0 and address = '" + address + "'", 
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

        public void StoreMessage(MessagePayload message, EndpointAddress address)
        {
            using (SqlCeConnection connection = GetConnection())
            {
                connection.Execute(
                    "update MessagePayloadStorageItem set old = 1 where id = @id", 
                    command => command.Parameters.AddWithValue("@id", message.Id));

                connection.Execute(
                    "insert into MessagePayloadStorageItem(id, createdon, headers, address, old) values(@id, @createdon, @headers, @address, 0)",  
                    command =>
                    {
                        command.Parameters.AddWithValue("@id", message.Id);
                        command.Parameters.AddWithValue("@createdon", message.CreatedOn);
                        command.Parameters.AddWithValue("@headers", this.serialiser.Serialise(message.Headers));
                        command.Parameters.AddWithValue("@address", message.GetFromAddress().ToString());
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
    }
}
