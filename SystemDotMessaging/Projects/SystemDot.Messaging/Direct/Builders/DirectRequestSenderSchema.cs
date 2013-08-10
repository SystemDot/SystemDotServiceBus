using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Filtering;

namespace SystemDot.Messaging.Direct.Builders
{
    class DirectRequestSenderSchema
    {
        public EndpointAddress ToAddress { get; set; }
        public EndpointAddress FromAddress { get; set; }
        public IMessageFilterStrategy FilterStrategy { get; set; }
    }
}