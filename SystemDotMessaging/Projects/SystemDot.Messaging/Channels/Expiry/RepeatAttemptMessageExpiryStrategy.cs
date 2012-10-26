using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Channels.RequestReply.Repeating;

namespace SystemDot.Messaging.Channels.Expiry
{
    public class RepeatAttemptMessageExpiryStrategy : IMessageExpiryStrategy
    {
        readonly int attempts;

        public RepeatAttemptMessageExpiryStrategy(int attempts)
        {
            this.attempts = attempts;
        }

        public bool HasExpired(MessagePayload toCheck)
        {
            return toCheck.GetAmountSent() > this.attempts;
        }
    }
}