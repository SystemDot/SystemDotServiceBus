using SystemDot.Messaging.Channels.Builders;

namespace SystemDot.Messaging.Channels.Local.Builders
{
    public class LocalChannelSchema : ChannelSchema
    {
        public IMessageProcessor<object, object> UnitOfWorkRunner { get; set; }

        public bool QueueErrors { get; set; }
    }
}