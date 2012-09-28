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

        public async void StoreMessage(MessagePayload message, EndpointAddress address)
        {
            Logger.Info("Storing message in sqlite storage");

            SQLiteAsyncConnection connection = GetConnection();

            string id = message.Id.ToString();

            var item = await GetItemAsync(id);

            if (item == null)
            {
                await connection.InsertAsync(new MessagePayloadStorageItem
                {
                    Id = id,
                    Address = message.GetFromAddress().ToString(),
                    Headers = this.serialiser.Serialise(message.Headers)
                });
            }
            else
            {
                item.Headers = this.serialiser.Serialise(message.Headers);
                await connection.UpdateAsync(item);
            }
        }

        public async void RemoveMessage(Guid id)
        {
            MessagePayloadStorageItem item = await GetItemAsync(id.ToString());
            if(item == null) return;

            await GetConnection().DeleteAsync(item);
        }

        private static async Task<MessagePayloadStorageItem> GetItemAsync(string id)
        {
            return await GetConnection()
                .Table<MessagePayloadStorageItem>()
                .Where(m => m.Id == id)
                .FirstOrDefaultAsync();
        }

        private static SQLiteAsyncConnection GetConnection()
        {
            return new SQLiteAsyncConnection("Messaging");
        }

        public async void Initialise()
        {
            await GetConnection().CreateTableAsync<MessagePayloadStorageItem>();
        }
    }
}
