using System.Diagnostics.Contracts;
using SystemDot.Messaging.Storage;
using SystemDot.Messaging.Storage.InMemory;

namespace SystemDot.Messaging.Channels.Builders
{
    public class PersistenceFactorySelector 
    {
        readonly IPersistenceFactory persistenceFactory;
        
        public PersistenceFactorySelector(IPersistenceFactory persistenceFactory)
        {
            Contract.Requires(persistenceFactory != null);
            
            this.persistenceFactory = persistenceFactory;
        }

        public IPersistenceFactory Select(SendChannelSchema schema)
        {
            Contract.Requires(schema != null);
            
            return (schema.IsDurable) 
                ? this.persistenceFactory
                : new InMemoryPersistenceFactory(new InMemoryDatatore());
        }
    }
}