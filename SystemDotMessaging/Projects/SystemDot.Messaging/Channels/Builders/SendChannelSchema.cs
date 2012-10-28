using SystemDot.Messaging.Channels.Addressing;
using SystemDot.Messaging.Channels.Expiry;

namespace SystemDot.Messaging.Channels.Builders
{
    public class SendChannelSchema : ChannelSchema
    {
        public EndpointAddress FromAddress { get; set; }

        public IMessageExpiryStrategy ExpiryStrategy { get; set; }
    }
}