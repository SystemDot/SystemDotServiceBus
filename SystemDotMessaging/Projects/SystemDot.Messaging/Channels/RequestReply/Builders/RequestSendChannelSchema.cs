using SystemDot.Messaging.Channels.Addressing;
using SystemDot.Messaging.Channels.Builders;
using SystemDot.Messaging.Channels.Expiry;
using SystemDot.Messaging.Channels.Filtering;

namespace SystemDot.Messaging.Channels.RequestReply.Builders
{
    public class RequestSendChannelSchema : SendChannelSchema
    {
        public IMessageFilterStrategy FilteringStrategy { get; set; }

        public EndpointAddress RecieverAddress { get; set; }
    }
}