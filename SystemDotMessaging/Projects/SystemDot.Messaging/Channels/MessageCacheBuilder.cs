using System.Diagnostics.Contracts;
using SystemDot.Messaging.Channels.Caching;
using SystemDot.Messaging.Storage;
using SystemDot.Messaging.Storage.InMemory;

namespace SystemDot.Messaging.Channels
{
    public class MessageCacheBuilder
    {
        public IMessageCache Create(SendChannelSchema schema, IPersistence persistence)
        {
            Contract.Requires(schema != null);
            Contract.Requires(persistence != null);

            return new MessageCache(schema.IsDurable ? persistence : new InMemoryPersistence());
        }
    }
}