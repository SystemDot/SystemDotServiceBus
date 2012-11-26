using SystemDot.Messaging.Channels.Addressing;

namespace SystemDot.Messaging.Channels.Builders
{
    public class RecieveChannelSchema : ChannelSchema
    {
        public EndpointAddress Address { get; set; }
        public EndpointAddress ToAddress { get; set; }
        public IMessageProcessor<object, object> UnitOfWorkRunner { get; set; }
    }
}