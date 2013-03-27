using SystemDot.Messaging.Packaging;

namespace SystemDot.Messaging.Expiry
{
    class PassthroughMessageExpiryStrategy : IMessageExpiryStrategy
    {
        public bool HasExpired(MessagePayload toCheck)
        {
            return false;
        }
    }
}