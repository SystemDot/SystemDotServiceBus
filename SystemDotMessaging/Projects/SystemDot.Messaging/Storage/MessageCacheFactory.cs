using System.Diagnostics.Contracts;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Storage.Changes;
using SystemDot.Storage.Changes;

namespace SystemDot.Messaging.Storage
{
    public class MessageCacheFactory
    {
        readonly IChangeStore store;

        public MessageCacheFactory(IChangeStore store)
        {
            Contract.Requires(store != null);
            this.store = store;
        }

        public MessageCache CreateCache(PersistenceUseType useType, EndpointAddress address)
        {
            var cache = new MessageCache(this.store, address, useType);

            cache.Initialise();

            return cache;
        }
    }
}