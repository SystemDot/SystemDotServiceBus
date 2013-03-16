using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Storage;

namespace SystemDot.Messaging.Builders
{
    public class ChannelBuilt
    {
        public PersistenceUseType UseType { get; set; }

        public EndpointAddress CacheAddress { get; set; }

        public EndpointAddress FromAddress { get; set; }

        public EndpointAddress ToAddress { get; set; }
    }
}