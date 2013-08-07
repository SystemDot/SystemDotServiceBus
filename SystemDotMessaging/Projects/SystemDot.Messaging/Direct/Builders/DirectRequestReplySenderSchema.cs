using SystemDot.Messaging.Addressing;

namespace SystemDot.Messaging.Direct.Builders
{
    class DirectRequestReplySenderSchema
    {
        public EndpointAddress ToAddress { get; set; }
        public EndpointAddress FromAddress { get; set; }
    }
}