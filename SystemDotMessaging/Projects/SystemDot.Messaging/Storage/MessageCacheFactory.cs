using System.Diagnostics.Contracts;
using SystemDot.Core;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Builders;
using SystemDot.Storage.Changes;

namespace SystemDot.Messaging.Storage
{
    class MessageCacheFactory
    {
        readonly ChangeStoreSelector changeStoreSelector;
        readonly ISystemTime systemTime;
        readonly NullChangeStore nullChangeStore;
        readonly ICheckpointStrategy checkPointStrategy;

        public MessageCacheFactory(
            ChangeStoreSelector changeStoreSelector, 
            ISystemTime systemTime,
            NullChangeStore nullChangeStore,
            ICheckpointStrategy checkPointStrategy)
        {
            Contract.Requires(changeStoreSelector != null);
            Contract.Requires(systemTime != null);
            Contract.Requires(nullChangeStore != null);

            this.changeStoreSelector = changeStoreSelector;
            this.systemTime = systemTime;
            this.nullChangeStore = nullChangeStore;
            this.checkPointStrategy = checkPointStrategy;
        }

        public ReceiveMessageCache BuildReceiveCache(PersistenceUseType useType, EndpointAddress address, IDurableOptionSchema durableOptions)
        {
            var cache = CreateReceiveMessageCache(useType, address, durableOptions);
            cache.Initialise();
            return cache;
        }

        ReceiveMessageCache CreateReceiveMessageCache(PersistenceUseType useType, EndpointAddress address, IDurableOptionSchema durableOptions)
        {
            return new ReceiveMessageCache(SelectChangeStore(durableOptions), address, useType, checkPointStrategy);
        }

        public SendMessageCache BuildSendCache(PersistenceUseType useType, EndpointAddress address, IDurableOptionSchema durableOptions)
        {
            var cache = CreateSendMessageCache(useType, address, SelectChangeStore(durableOptions));
            cache.Initialise();
            return cache;
        }

        SendMessageCache CreateSendMessageCache(PersistenceUseType useType, EndpointAddress address, ChangeStore changeStore)
        {
            return new SendMessageCache(systemTime, changeStore, address, useType, checkPointStrategy);
        }

        public SendMessageCache BuildNonDurableSendCache(PersistenceUseType useType, EndpointAddress address)
        {
            var cache = CreateSendMessageCache(useType, address, nullChangeStore);
            cache.Initialise();
            return cache;
        }

        ChangeStore SelectChangeStore(IDurableOptionSchema durableOptions)
        {
            return changeStoreSelector.SelectChangeStore(durableOptions);
        }
    }
}