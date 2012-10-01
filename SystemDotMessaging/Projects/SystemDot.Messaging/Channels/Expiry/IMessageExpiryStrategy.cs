using SystemDot.Messaging.Channels.Packaging;

namespace SystemDot.Messaging.Channels.Expiry
{
    public interface IMessageExpiryStrategy
    {
        bool HasExpired(MessagePayload toCheck);
    }
}