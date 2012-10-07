using SystemDot.Messaging.Channels.Expiry;

namespace SystemDot.Messaging.Channels
{
    public class SendChannelSchema
    {
        public EndpointAddress FromAddress { get; set; }

        public bool IsDurable { get; set; }

        public IMessageExpiryStrategy ExpiryStrategy { get; set; }
    }
}