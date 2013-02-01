using SystemDot.Messaging.Packaging;

namespace SystemDot.Messaging.Expiry
{
    public class PassthroughMessageExpiryStrategy : IMessageExpiryStrategy
    {
        public bool HasExpired(MessagePayload toCheck)
        {
            return false;
        }
    }
}