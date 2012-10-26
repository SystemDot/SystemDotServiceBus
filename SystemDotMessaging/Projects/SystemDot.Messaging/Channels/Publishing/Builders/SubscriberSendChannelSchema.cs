using System.Diagnostics.Contracts;
using SystemDot.Messaging.Channels.Addressing;
using SystemDot.Messaging.Channels.Builders;

namespace SystemDot.Messaging.Channels.Publishing.Builders
{
    public class SubscriberSendChannelSchema : SendChannelSchema
    {       
        public EndpointAddress SubscriberAddress { get; set; }
    }
}