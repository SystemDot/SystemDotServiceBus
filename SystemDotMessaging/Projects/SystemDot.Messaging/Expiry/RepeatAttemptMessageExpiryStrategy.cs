using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Repeating;

namespace SystemDot.Messaging.Expiry
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
            return toCheck.GetAmountSent() == this.attempts;
        }
    }
}