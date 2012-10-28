using System.Diagnostics.Contracts;
using SystemDot.Messaging.Storage;
using SystemDot.Messaging.Storage.InMemory;

namespace SystemDot.Messaging.Channels.Builders
{
    public class PersistenceFactorySelector 
    {
        readonly IPersistenceFactory persistenceFactory;
        readonly IDatastore datastore;

        public PersistenceFactorySelector(IPersistenceFactory persistenceFactory, IDatastore datastore)
        {
            Contract.Requires(persistenceFactory != null);
            Contract.Requires(datastore != null);
            
            this.persistenceFactory = persistenceFactory;
            this.datastore = datastore;
        }

        public IPersistenceFactory Select(ChannelSchema schema)
        {
            Contract.Requires(schema != null);
            
            return (schema.IsDurable) 
                ? this.persistenceFactory
                : new InMemoryPersistenceFactory(this.datastore);
        }
    }
}