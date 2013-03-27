using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Packaging;

namespace SystemDot.Messaging.Storage
{
    public class MessageAddedToCache
    {
        public EndpointAddress CacheAddress { get; set; }
        
        public PersistenceUseType UseType { get; set; }

        public MessagePayload Message { get; set; }
    }
}