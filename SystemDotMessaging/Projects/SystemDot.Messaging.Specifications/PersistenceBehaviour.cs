using SystemDot.Messaging.Channels;
using SystemDot.Messaging.Storage;
using SystemDot.Messaging.Storage.InMemory;
using Machine.Fakes;

namespace SystemDot.Messaging.Specifications
{
    public class PersistenceBehaviour
    {
        static PersistenceUseType persistenceUseType;
        static EndpointAddress channel;

        OnEstablish context = (accessor) => 
        {
            accessor.Configure(new InMemoryDatatore());

            accessor.Configure<IPersistenceFactory>(
                new InMemoryPersistenceFactory(accessor.The<InMemoryDatatore>()));

            accessor.Configure<IPersistence>(
                accessor.The<IPersistenceFactory>()
                    .CreatePersistence(persistenceUseType, channel));
        };

        public PersistenceBehaviour(PersistenceUseType persistenceUseType, EndpointAddress channel)
        {
            PersistenceBehaviour.persistenceUseType = persistenceUseType;
            PersistenceBehaviour.channel = channel;
        }

        public PersistenceBehaviour()
            : this(PersistenceUseType.Other, new EndpointAddress("Channel", "Server"))
        {
        }
    }
}