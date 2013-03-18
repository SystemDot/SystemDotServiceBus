using SystemDot.Messaging.Addressing;

namespace SystemDot.Messaging.PointToPoint.Builders
{
    public class PointToPointSendChannelBuilt
    {
        public EndpointAddress SenderAddress { get; set; }
        public EndpointAddress ReceiverAddress { get; set; }
    }
}