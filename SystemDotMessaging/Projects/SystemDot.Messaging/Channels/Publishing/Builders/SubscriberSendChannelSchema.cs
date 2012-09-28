using System.Diagnostics.Contracts;

namespace SystemDot.Messaging.Channels.Publishing.Builders
{
    public class SubscriberSendChannelSchema : SendChannelSchema
    {       
        public EndpointAddress SubscriberAddress { get; set; }
    }
}