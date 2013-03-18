using SystemDot.Messaging.Addressing;

namespace SystemDot.Messaging.Publishing.Builders
{
    public class SubscriberSendChannelBuilt
    {
        public EndpointAddress CacheAddress { get; set; }

        public EndpointAddress SubscriberAddress { get; set; }

        public EndpointAddress PublisherAddress { get; set; }
    }
}