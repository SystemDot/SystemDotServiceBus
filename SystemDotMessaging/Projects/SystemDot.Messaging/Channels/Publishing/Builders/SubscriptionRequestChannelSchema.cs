using SystemDot.Messaging.Channels.Addressing;
using SystemDot.Messaging.Channels.Builders;

namespace SystemDot.Messaging.Channels.Publishing.Builders
{
    public class SubscriptionRequestChannelSchema : ChannelSchema
    {
        public EndpointAddress SubscriberAddress { get; set; }

        public EndpointAddress PublisherAddress { get; set; }
    }
}