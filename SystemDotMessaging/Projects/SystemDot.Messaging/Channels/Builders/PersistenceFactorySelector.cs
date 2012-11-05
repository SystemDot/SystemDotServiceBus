using System.Diagnostics.Contracts;
using SystemDot.Messaging.Storage;
using SystemDot.Messaging.Storage.Changes;
using SystemDot.Serialisation;

namespace SystemDot.Messaging.Channels.Builders
{
    public class PersistenceFactorySelector 
    {
        readonly IPersistenceFactory persistenceFactory;
        readonly InMemoryChangeStore inMemoryStore;

        public PersistenceFactorySelector(IPersistenceFactory persistenceFactory, InMemoryChangeStore inMemoryStore)
        {
            Contract.Requires(persistenceFactory != null);
            Contract.Requires(inMemoryStore != null);
            
            this.persistenceFactory = persistenceFactory;
            this.inMemoryStore = inMemoryStore;
        }

        public IPersistenceFactory Select(ChannelSchema schema)
        {
            Contract.Requires(schema != null);
            
            return (schema.IsDurable) 
                ? this.persistenceFactory
                : new PersistenceFactory(this.inMemoryStore);
        }
    }
}