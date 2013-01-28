using SystemDot.Messaging.Channels.Addressing;
using SystemDot.Messaging.Channels.Builders;

namespace SystemDot.Messaging.Channels.PointToPoint.Builders
{
    public class PointToPointSendChannelSchema : SendChannelSchema
    {
        public EndpointAddress RecieverAddress { get; set; }
    }
}