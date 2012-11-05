using SystemDot.Messaging.Channels.Addressing;
using SystemDot.Messaging.Storage;
using SystemDot.Messaging.Storage.Changes;
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
            accessor.Configure<IChangeStore>(new InMemoryChangeStore(new PlatformAgnosticSerialiser()));

            accessor.Configure<IPersistenceFactory>(
                new PersistenceFactory(accessor.The<IChangeStore>()));

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