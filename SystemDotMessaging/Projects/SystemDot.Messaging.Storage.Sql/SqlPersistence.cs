using System;
using System.Collections.Generic;
using System.Data.SqlServerCe;
using SystemDot.Messaging.Channels;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Serialisation;

namespace SystemDot.Messaging.Storage.Sql
{
    public class SqlPersistence : IPersistence
    {
        readonly ISerialiser serialiser;
        readonly PersistenceUseType useType;
        readonly EndpointAddress address;

        public SqlPersistence(ISerialiser serialiser, PersistenceUseType useType, EndpointAddress address)
        {
            this.serialiser = serialiser;
            this.useType = useType;
            this.address = address;
            Initialise();
        }

        public IEnumerable<MessagePayload> GetMessages()
        {
            var messages = new List<MessagePayload>();

            using (SqlCeConnection connection = GetConnection())
            {
                connection.ExecuteReader(
                    "select id, createdon, headers from MessagePayloadStorageItem where address = '" + this.address + "'",
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

        public void AddMessage(MessagePayload message)
        {
            using (SqlCeConnection connection = GetConnection())
            {
                using (SqlCeTransaction transaction = connection.BeginTransaction())
                {
                    connection.Execute(
                        "update MessageSequence set sequencenumber = sequencenumber + 1 where address = @address and type = @type",
                        command =>
                            {
                                command.Parameters.AddWithValue("@address", this.address.ToString());
                                command.Parameters.AddWithValue("@type", this.useType);
                            });

                    connection.Execute(
                        "insert into MessagePayloadStorageItem(id, createdon, headers, address, type) values(@id, @createdon, @headers, @address, @type)",
                        command =>
                        {
                            command.Parameters.AddWithValue("@id", message.Id);
                            command.Parameters.AddWithValue("@createdon", message.CreatedOn);
                            command.Parameters.AddWithValue("@headers", this.serialiser.Serialise(message.Headers));
                            command.Parameters.AddWithValue("@address", this.address.ToString());
                            command.Parameters.AddWithValue("@type", this.useType);
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
                    "update MessagePayloadStorageItem set headers = @headers where id = @id",
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

        public int GetSequence()
        {
            using (SqlCeConnection connection = GetConnection())
            {
                return connection.ExecuteScalar<int>(
                    "select sequencenumber from MessageSequence where address = @address and type = @type",
                    command =>
                        {
                            command.Parameters.AddWithValue("@address", this.address.ToString());
                            command.Parameters.AddWithValue("@type", this.useType);
                        });
            }
        }
        
        public void SetSequence(int toSet)
        {
            using (SqlCeConnection connection = GetConnection())
            {
                connection.Execute(
                    "update MessageSequence set sequencenumber = @sequence where address = @address and type = @type",
                    command =>
                    {
                        command.Parameters.AddWithValue("@sequence", toSet);
                        command.Parameters.AddWithValue("@address", address.ToString());
                        command.Parameters.AddWithValue("@type", this.useType);
                    });
            }
        }

        public void Initialise()
        {
            using (SqlCeConnection connection = GetConnection())
            {
                if (connection.ExecuteScalar<int>(
                    "select count(*) from MessageSequence where address = @address and type = @type",
                    command =>
                        {
                            command.Parameters.AddWithValue("@address", this.address.ToString());
                            command.Parameters.AddWithValue("@type", this.useType);
                        }) > 0) return;

                connection.Execute(
                    "insert into MessageSequence(address, sequencenumber, type) values(@address, 1, @type)",
                    command =>
                        {
                            command.Parameters.AddWithValue("@address", this.address.ToString());
                            command.Parameters.AddWithValue("@type", this.useType);
                        });
            }
        }

        SqlCeConnection GetConnection()
        {
            var connection = new SqlCeConnection("Data Source=|DataDirectory|\\Messaging.sdf");
            connection.Open();
            return connection;
        }
    }
}
