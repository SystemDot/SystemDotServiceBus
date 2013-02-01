using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Expiry;
using SystemDot.Messaging.Repeating;

namespace SystemDot.Messaging.Builders
{
    public class SendChannelSchema : ChannelSchema
    {
        public EndpointAddress FromAddress { get; set; }

        public IMessageExpiryStrategy ExpiryStrategy { get; set; }

        public IRepeatStrategy RepeatStrategy { get; set; }
    }
}