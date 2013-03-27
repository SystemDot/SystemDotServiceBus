using System.Diagnostics.Contracts;
using SystemDot.Messaging.Addressing;
using SystemDot.Storage.Changes;

namespace SystemDot.Messaging.Storage
{
    class MessageCacheFactory
    {
        readonly IChangeStore store;
        readonly ISystemTime systemTime;

        public MessageCacheFactory(IChangeStore store, ISystemTime systemTime)
        {
            Contract.Requires(store != null);
            Contract.Requires(systemTime != null);

            this.store = store;
            this.systemTime = systemTime;
        }

        public ReceiveMessageCache CreateReceiveCache(PersistenceUseType useType, EndpointAddress address)
        {
            var cache = new ReceiveMessageCache(this.store, address, useType);

            cache.Initialise();

            return cache;
        }

        public SendMessageCache CreateSendCache(PersistenceUseType useType, EndpointAddress address)
        {
            var cache = new SendMessageCache(this.systemTime, this.store, address, useType);

            cache.Initialise();

            return cache;
        }
    }
}