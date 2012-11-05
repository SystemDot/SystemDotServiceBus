using System.Diagnostics.Contracts;
using SystemDot.Messaging.Channels.Addressing;
using SystemDot.Messaging.Storage.Changes;

namespace SystemDot.Messaging.Storage
{
    public class PersistenceFactory : IPersistenceFactory
    {
        readonly IChangeStore store;

        public PersistenceFactory(IChangeStore store)
        {
            Contract.Requires(store != null);
            this.store = store;
        }

        public IPersistence CreatePersistence(PersistenceUseType useType, EndpointAddress address)
        {
            var persistence = new Persistence(this.store, address, useType);
            persistence.Initialise();

            return persistence;
        }
    }
}