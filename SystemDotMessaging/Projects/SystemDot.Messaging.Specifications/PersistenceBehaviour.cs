using SystemDot.Messaging.Channels;
using SystemDot.Messaging.Channels.Addressing;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Storage;
using SystemDot.Messaging.Storage.InMemory;
using SystemDot.Serialisation;
using Machine.Fakes;

namespace SystemDot.Messaging.Specifications
{
    public class PersistenceBehaviour
    {
        static PersistenceUseType persistenceUseType;
        static EndpointAddress channel;

        OnEstablish context = (accessor) => 
        {
            accessor.Configure<IDatastore>(new InMemoryDatastore(new PlatformAgnosticSerialiser()));

            accessor.Configure<IPersistenceFactory>(
                new InMemoryPersistenceFactory(accessor.The<IDatastore>()));

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
            : this(PersistenceUseType.SubscriberRequestSend, new EndpointAddress("Channel", "Server"))
        {
        }
    }
}