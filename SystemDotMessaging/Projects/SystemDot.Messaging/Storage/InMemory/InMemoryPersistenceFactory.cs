using System.Diagnostics.Contracts;
using SystemDot.Messaging.Channels.Addressing;

namespace SystemDot.Messaging.Storage.InMemory
{
    public class InMemoryPersistenceFactory : IPersistenceFactory
    {
        readonly IDatastore store;

        public InMemoryPersistenceFactory(IDatastore store)
        {
            Contract.Requires(store != null);
            this.store = store;
        }

        public IPersistence CreatePersistence(PersistenceUseType useType, EndpointAddress address)
        {
            return new InMemoryPersistence(this.store, useType, address);
        }
    }
}