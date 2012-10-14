using SystemDot.Messaging.Channels;

namespace SystemDot.Messaging.Storage
{
    public interface IPersistenceFactory
    {
        IPersistence CreatePersistence(PersistenceUseType useType, EndpointAddress address);
    }
}