using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Builders;

namespace SystemDot.Messaging.PointToPoint.Builders
{
    class PointToPointSendChannelSchema : SendChannelSchema
    {
        public EndpointAddress ReceiverAddress { get; set; }
    }
}