using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Builders;
using SystemDot.Messaging.Filtering;

namespace SystemDot.Messaging.PointToPoint.Builders
{
    class PointToPointSendChannelSchema : SendChannelSchema
    {
        public EndpointAddress ReceiverAddress { get; set; }
        public IMessageFilterStrategy FilteringStrategy { get; set; }
    }
}