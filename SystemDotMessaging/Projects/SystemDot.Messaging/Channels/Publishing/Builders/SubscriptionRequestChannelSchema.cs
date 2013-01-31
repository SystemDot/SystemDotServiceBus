using SystemDot.Messaging.Channels.Addressing;
using SystemDot.Messaging.Channels.Builders;
using SystemDot.Messaging.Channels.Repeating;

namespace SystemDot.Messaging.Channels.Publishing.Builders
{
    public class SubscriptionRequestChannelSchema : ChannelSchema
    {
        public EndpointAddress SubscriberAddress { get; set; }

        public EndpointAddress PublisherAddress { get; set; }

        public IRepeatStrategy RepeatStrategy { get; set; }
    }
}