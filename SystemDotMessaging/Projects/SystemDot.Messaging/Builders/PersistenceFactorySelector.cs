using System.Diagnostics.Contracts;
using SystemDot.Messaging.Storage;
using SystemDot.Storage.Changes;

namespace SystemDot.Messaging.Builders
{
    public class PersistenceFactorySelector 
    {
        readonly MessageCacheFactory messageCacheFactory;
        readonly InMemoryChangeStore inMemoryStore;

        public PersistenceFactorySelector(MessageCacheFactory messageCacheFactory, InMemoryChangeStore inMemoryStore)
        {
            Contract.Requires(messageCacheFactory != null);
            Contract.Requires(inMemoryStore != null);
            
            this.messageCacheFactory = messageCacheFactory;
            this.inMemoryStore = inMemoryStore;
        }

        public MessageCacheFactory Select(ChannelSchema schema)
        {
            Contract.Requires(schema != null);
            
            return (schema.IsDurable) 
                ? this.messageCacheFactory
                : new MessageCacheFactory(this.inMemoryStore);
        }
    }
}