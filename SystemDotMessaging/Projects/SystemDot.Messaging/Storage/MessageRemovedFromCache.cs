using System;
using SystemDot.Messaging.Addressing;

namespace SystemDot.Messaging.Storage
{
    public class MessageRemovedFromCache
    {
        public Guid MessageId { get; set; }

        public EndpointAddress Address { get; set; }

        public PersistenceUseType UseType { get; set; }
    }
}