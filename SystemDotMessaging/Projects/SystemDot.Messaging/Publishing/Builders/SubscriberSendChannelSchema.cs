using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Builders;

namespace SystemDot.Messaging.Publishing.Builders
{
    class SubscriberSendChannelSchema : SendChannelSchema
    {       
        public EndpointAddress SubscriberAddress { get; set; }
    }
}