using SystemDot.Messaging.Channels;
using SystemDot.Messaging.Channels.Addressing;

namespace SystemDot.Messaging.Storage
{
    public interface IPersistenceFactory
    {
        IPersistence CreatePersistence(PersistenceUseType useType, EndpointAddress address);
    }
}