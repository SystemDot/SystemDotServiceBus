using System.Diagnostics.Contracts;
using SystemDot.Messaging.Storage;
using SystemDot.Storage.Changes;

namespace SystemDot.Messaging.Builders
{
    class PersistenceFactorySelector 
    {
        readonly MessageCacheFactory messageCacheFactory;
        readonly InMemoryChangeStore inMemoryStore;
        readonly ISystemTime systemTime;

        public PersistenceFactorySelector(
            MessageCacheFactory messageCacheFactory, 
            InMemoryChangeStore inMemoryStore, 
            ISystemTime systemTime)
        {
            Contract.Requires(messageCacheFactory != null);
            Contract.Requires(inMemoryStore != null);
            Contract.Requires(systemTime != null);
            
            this.messageCacheFactory = messageCacheFactory;
            this.inMemoryStore = inMemoryStore;
            this.systemTime = systemTime;
        }

        public MessageCacheFactory Select(ChannelSchema schema)
        {
            Contract.Requires(schema != null);
            
            return (schema.IsDurable) 
                ? this.messageCacheFactory
                : new MessageCacheFactory(this.inMemoryStore, this.systemTime);
        }
    }
}