using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Filtering;

namespace SystemDot.Messaging.Direct.Builders
{
    class RequestReceiverSchema
    {
        public EndpointAddress Address { get; set; }
        public IMessageFilterStrategy FilterStrategy { get; set; }
        public bool BlockMessages { get; set; }
    }
}