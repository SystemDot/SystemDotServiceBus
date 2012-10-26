using SystemDot.Messaging.Channels.Addressing;

namespace SystemDot.Messaging.Channels.Builders
{
    public class RecieveChannelSchema
    {
        public EndpointAddress Address { get; set; }
        public EndpointAddress ToAddress { get; set; }
        public bool IsSequenced { get; set; }
    }
}