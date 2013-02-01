using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Builders;
using SystemDot.Messaging.Filtering;

namespace SystemDot.Messaging.RequestReply.Builders
{
    public class RequestSendChannelSchema : SendChannelSchema
    {
        public IMessageFilterStrategy FilteringStrategy { get; set; }

        public EndpointAddress RecieverAddress { get; set; }
    }
}