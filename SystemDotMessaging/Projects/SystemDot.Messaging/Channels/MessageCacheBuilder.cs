using System.Diagnostics.Contracts;
using SystemDot.Messaging.Channels.Caching;
using SystemDot.Messaging.Storage;

namespace SystemDot.Messaging.Channels
{
    public class MessageCacheBuilder
    {
        readonly IPersistence persistence;

        public MessageCacheBuilder(IPersistence persistence)
        {
            Contract.Requires(persistence != null);
            this.persistence = persistence;
        }

        public IMessageCache Create(SendChannelSchema schema)
        {
            Contract.Requires(schema != null);
            return new MessageCache(GetPersistence(schema), schema.FromAddress);
        }

        IPersistence GetPersistence(SendChannelSchema schema)
        {
            return schema.IsPersistent ? this.persistence : new InMemoryPersistence();
        }
    }
}