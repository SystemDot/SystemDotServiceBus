using SystemDot.Messaging.Expiry;

namespace SystemDot.Messaging.Configuration.Expiry
{
    public interface IExpireMessagesConfigurer
    {
        void SetMessageExpiryStrategy(IMessageExpiryStrategy strategy);
    }
}