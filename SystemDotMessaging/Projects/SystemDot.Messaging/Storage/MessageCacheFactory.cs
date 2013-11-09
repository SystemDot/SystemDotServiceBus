using System.Diagnostics.Contracts;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Builders;
using SystemDot.Storage.Changes;

namespace SystemDot.Messaging.Storage
{
    class MessageCacheFactory
    {
        readonly ChangeStoreSelector changeStoreSelector;
        readonly ISystemTime systemTime;

        public MessageCacheFactory(ChangeStoreSelector changeStoreSelector, ISystemTime systemTime)
        {
            Contract.Requires(changeStoreSelector != null);
            Contract.Requires(systemTime != null);

            this.changeStoreSelector = changeStoreSelector;
            this.systemTime = systemTime;
        }

        public ReceiveMessageCache CreateReceiveCache(PersistenceUseType useType, EndpointAddress address, IDurableOptionSchema durableOptions)
        {
            var cache = new ReceiveMessageCache(changeStoreSelector.SelectChangeStore(durableOptions), address, useType);

            cache.Initialise();

            return cache;
        }

        public SendMessageCache CreateSendCache(PersistenceUseType useType, EndpointAddress address, IDurableOptionSchema durableOptions)
        {
            var cache = new SendMessageCache(systemTime, changeStoreSelector.SelectChangeStore(durableOptions), address, useType);

            cache.Initialise();

            return cache;
        }

        public SendMessageCache CreateNonDurableSendCache(PersistenceUseType useType, EndpointAddress address)
        {
            var cache = new SendMessageCache(systemTime, new NullChangeStore(), address, useType);

            cache.Initialise();

            return cache;
        }
    }
}