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
        private readonly PersistenceUseType useType;
        readonly EndpointAddress address;

        public SqlitePersistence(ISerialiser serialiser, PersistenceUseType useType, EndpointAddress address)
        {
            this.serialiser = serialiser;
            this.useType = useType;
            this.address = address;
            Initialise();
        }

        public IEnumerable<MessagePayload> GetMessages()
        {
            return GetForChannelAsync().Result.Select(m =>
                new MessagePayload
                {
                    Id = new Guid(m.Id),
                    Headers = this.serialiser.Deserialise(m.Headers).As<List<IMessageHeader>>()
                });
        }

        async Task<List<MessagePayloadStorageItem>> GetForChannelAsync()
        {
            var addressString = address.ToString();
            var useTypeInt = this.useType.As<int>();

            return await GetAsyncConnection()
                .Table<MessagePayloadStorageItem>()
                .Where(m => m.Address == addressString && m.Type == useTypeInt)
                .ToListAsync();
        }

        public void AddMessage(MessagePayload message)
        {
            Logger.Info("Storing message in sqlite storage");

            GetAsyncConnection().RunInTransaction(c =>
            {
                c.Execute(
                    "update MessageSequence set sequencenumber = sequencenumber + 1 where address = ? and type = ?",
                    this.address.ToString(),
                    this.useType);

                c.Execute(
                    "insert into MessagePayloadStorageItem"
                        + "(id, createdon, headers, address, type)"
                        + "values(?, ?, ?, ?, ?)",
                    message.Id.ToString(),
                    message.CreatedOn,
                    this.address.ToString(),
                    this.serialiser.Serialise(message.Headers),
                    this.useType);
            });
        }

        public async void UpdateMessage(MessagePayload message)
        {
            Logger.Info("Storing message in sqlite storage");

            const string statement = "update MessagePayloadStorageItem set headers = ? where id = ?";

            await GetAsyncConnection().ExecuteAsync(
                statement,
                this.serialiser.Serialise(message.Headers),
                message.Id.ToString());
        }

        public async void RemoveMessage(Guid id)
        {
            await GetAsyncConnection().ExecuteAsync(
                "delete from MessagePayloadStorageItem where id = ?", 
                id.ToString());
        }

        public int GetSequence()
        {
            return GetAsyncConnection().ExecuteScalar<int>(
                "select sequencenumber from MessageSequence where address = ? and type = ?", 
                this.address.ToString(),
                this.useType);
        }

        public void SetSequence(int toSet)
        {
            GetAsyncConnection().Execute(
                "update MessageSequence set sequencenumber = ? where address = ? and type = ?",
                toSet,
                this.address.ToString(),
                this.useType);
        }

        async void Initialise()
        {
            await GetAsyncConnection().ExecuteAsync(
                "create table if not exists MessagePayloadStorageItem(\n"
                + "Id varchar(32) not null, \n"
                + "CreatedOn bigint not null, \n"
                + "Headers blob, \n"
                + "Address varchar(1000), \n"
                + "Type int)");

            await GetAsyncConnection().ExecuteAsync(
                "create table if not exists MessageSequence(Address varchar(1000), SequenceNumber int, Type int)");

            if (await GetAsyncConnection().ExecuteScalarAsync<int>(
                "select count(*) from MessageSequence where address = ? and type = ?",
                 address.ToString(),
                 this.useType) > 0) return;

            await GetAsyncConnection().ExecuteAsync(
                "insert into MessageSequence(address, sequencenumber, type) values(?, 1, ?)",
                address.ToString(),
                 this.useType);
        }

        protected static SQLiteAsyncConnection GetAsyncConnection()
        {
            return new SQLiteAsyncConnection("Messaging");
        }
    }
}
