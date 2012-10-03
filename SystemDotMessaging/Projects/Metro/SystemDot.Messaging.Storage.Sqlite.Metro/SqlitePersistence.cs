using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SystemDot.Logging;
using SystemDot.Messaging.Channels;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Channels.Packaging.Headers;
using SystemDot.Serialisation;
using SQLite;

namespace SystemDot.Messaging.Storage.Sqlite.Metro
{
    public class SqlitePersistence : IPersistence
    {
        readonly ISerialiser serialiser;

        public SqlitePersistence(ISerialiser serialiser)
        {
            this.serialiser = serialiser;
        }

        public IEnumerable<MessagePayload> GetMessages(EndpointAddress address)
        {
            return GetForChannelAsync(address).Result.Select(m => 
                new MessagePayload
                {
                    Id = new Guid(m.Id),
                    Headers = this.serialiser.Deserialise(m.Headers).As<List<IMessageHeader>>()
                });
        }

        async Task<List<MessagePayloadStorageItem>> GetForChannelAsync(EndpointAddress address)
        {
            var addressString = address.ToString();

            return await GetConnection()
                .Table<MessagePayloadStorageItem>()
                .Where(m => m.Address == addressString)
                .ToListAsync();
        }

        public async void AddMessage(MessagePayload message, EndpointAddress address)
        {
            Logger.Info("Storing message in sqlite storage");

            await GetConnection().ExecuteAsync(
                "update MessageSequence set sequencenumber = sequencenumber + 1 where address = ?",
                message.Id.ToString());

            const string statement = "insert into MessagePayloadStorageItem" 
                + "(id, createdon, headers, address)" 
                + "values(?, ?, ?, ?)";

            await GetConnection().ExecuteAsync(
                statement,
                message.Id.ToString(), 
                message.CreatedOn, 
                message.GetFromAddress().ToString(), 
                this.serialiser.Serialise(message.Headers));
        }

        public async void UpdateMessage(MessagePayload message)
        {
            Logger.Info("Storing message in sqlite storage");

            const string statement = "update MessagePayloadStorageItem set headers = ? where id = ?";

            await GetConnection().ExecuteAsync(
                statement,
                this.serialiser.Serialise(message.Headers),
                message.Id.ToString());
        }

        public async void RemoveMessage(Guid id)
        {
            await GetConnection().ExecuteAsync("delete from MessagePayloadStorageItem where id = ?", id.ToString());
        }

        public int GetNextSequence(EndpointAddress address)
        {
            return GetNextSequenceAsync(address).Result;
        }

        public async void InitialiseChannel(EndpointAddress address)
        {
            await GetConnection().ExecuteAsync(
                "insert into MessageSequence(address, sequencenumber) values(?, 1)",
                address.ToString());
        }

        static async Task<int> GetNextSequenceAsync(EndpointAddress address)
        {
            return await GetConnection().ExecuteScalarAsync<int>(
                "select sequencenumber from MessageSequence where address = ?", 
                address.ToString());
        }

        private static SQLiteAsyncConnection GetConnection()
        {
            return new SQLiteAsyncConnection("Messaging");
        }

        public async void Initialise()
        {
            await GetConnection().ExecuteAsync(
                "create table if not exists MessagePayloadStorageItem(\n"
                + "Id varchar(140) primary key not null ,\n"
                + "CreatedOn bigint not null ,\n"
                + "Headers blob ,\n"
                + "Address varchar(1000))");

            await GetConnection().ExecuteAsync(
                "create table if not exists MessageSequence(Address varchar(1000), SequenceNumber int)");
        }
    }
}
