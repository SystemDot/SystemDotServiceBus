using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Storage;

namespace SystemDot.Messaging.Builders
{
    public class ChannelBuilt
    {
        public PersistenceUseType UseType { get; set; }

        public EndpointAddress Address { get; set; }
    }
}