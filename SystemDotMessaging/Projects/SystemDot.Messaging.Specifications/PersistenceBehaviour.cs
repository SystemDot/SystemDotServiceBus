using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Storage;
using SystemDot.Serialisation;
using SystemDot.Storage.Changes;
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
            accessor.Configure(new MessageCacheFactory(accessor.The<IChangeStore>(), accessor.The<ISystemTime>()));
            accessor.Configure(accessor.The<MessageCacheFactory>().CreateSendCache(persistenceUseType, channel));
            accessor.Configure(accessor.The<MessageCacheFactory>().CreateReceiveCache(persistenceUseType, channel));
        };

        public PersistenceBehaviour(PersistenceUseType persistenceUseType, EndpointAddress channel)
        {
            PersistenceBehaviour.persistenceUseType = persistenceUseType;
            PersistenceBehaviour.channel = channel;
        }

        public PersistenceBehaviour()
            : this(PersistenceUseType.SubscriberRequestSend, TestEndpointAddressBuilder.Build("GetChannel", "Server"))
        {
        }
    }
}