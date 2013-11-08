using System.Diagnostics.Contracts;
using SystemDot.Messaging.Storage;
using SystemDot.Storage.Changes;

namespace SystemDot.Messaging.Builders
{
    class PersistenceFactorySelector 
    {
        readonly MessageCacheFactory messageCacheFactory;
        readonly ISystemTime systemTime;

        public PersistenceFactorySelector(
            MessageCacheFactory messageCacheFactory, 
            ISystemTime systemTime)
        {
            Contract.Requires(messageCacheFactory != null);
            Contract.Requires(systemTime != null);
            
            this.messageCacheFactory = messageCacheFactory;
            this.systemTime = systemTime;
        }

        public MessageCacheFactory Select(IDurableOptionSchema schema)
        {
            Contract.Requires(schema != null);
            
            return (schema.IsDurable) 
                ? messageCacheFactory
                : new MessageCacheFactory(new NullChangeStore(), systemTime);
        }
    }
}