using SystemDot.Messaging.Channels;

namespace SystemDot.Messaging.Storage.InMemory
{
    public class InMemoryPersistenceFactory : IPersistenceFactory
    {
        public IPersistence CreatePersistence(PersistenceUseType useType, EndpointAddress address)
        {
            return new InMemoryPersistence();
        }
    }
}