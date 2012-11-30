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
            accessor.Configure(new MessageCacheFactory(accessor.The<IChangeStore>()));
            accessor.Configure(accessor.The<MessageCacheFactory>().CreateCache(persistenceUseType, channel));
        };

        public PersistenceBehaviour(PersistenceUseType persistenceUseType, EndpointAddress channel)
        {
            PersistenceBehaviour.persistenceUseType = persistenceUseType;
            PersistenceBehaviour.channel = channel;
        }

        public PersistenceBehaviour()
            : this(PersistenceUseType.SubscriberRequestSend, new EndpointAddress("GetChannel", "Server"))
        {
        }
    }
}