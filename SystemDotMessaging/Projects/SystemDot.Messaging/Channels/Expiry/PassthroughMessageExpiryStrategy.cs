using SystemDot.Messaging.Channels.Packaging;

namespace SystemDot.Messaging.Channels.Expiry
{
    public class PassthroughMessageExpiryStrategy : IMessageExpiryStrategy
    {
        public bool HasExpired(MessagePayload toCheck)
        {
            return false;
        }
    }
}