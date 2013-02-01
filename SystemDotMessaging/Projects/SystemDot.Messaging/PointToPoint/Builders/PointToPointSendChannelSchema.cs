using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Builders;

namespace SystemDot.Messaging.PointToPoint.Builders
{
    public class PointToPointSendChannelSchema : SendChannelSchema
    {
        public EndpointAddress RecieverAddress { get; set; }
    }
}