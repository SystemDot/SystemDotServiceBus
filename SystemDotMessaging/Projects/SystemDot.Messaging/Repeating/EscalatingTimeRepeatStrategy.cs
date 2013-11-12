using System.Collections.Generic;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Storage;

namespace SystemDot.Messaging.Repeating
{
    public class EscalatingTimeRepeatStrategy : LoggingRepeatStrategy, IRepeatStrategy
    {
        public int ToStartAt { get; set; }
        public int Multiplier { get; set; }
        public int Peak { get; set; }
        
        public void Repeat(MessageRepeater repeater, IMessageCache messageCache, ISystemTime systemTime)
        {
            IEnumerable<MessagePayload> messages = messageCache.GetOrderedMessages();

            messages.ForEach(m =>
            {
                if (m.GetAmountSent() > 0 && m.GetLastTimeSent().Ticks <= systemTime.GetCurrentDate().AddSeconds(-GetDelay(m)).Ticks)
                {
                    LogMessage(m);
                    repeater.InputMessage(m);
                }
            });
        }

        int GetDelay(MessagePayload toGetDelayFor)
        {
            int unlimitedDelay = GetUnlimitedDelay(toGetDelayFor);

            return unlimitedDelay < Peak 
                ? unlimitedDelay 
                : Peak;
        }

        int GetUnlimitedDelay(MessagePayload toGetDelayFor)
        {
            return toGetDelayFor.GetAmountSent() == 1 
                ? ToStartAt 
                : ToStartAt * GetMultiplier(toGetDelayFor);
        }

        int GetMultiplier(MessagePayload toGetDelayFor)
        {
            return (toGetDelayFor.GetAmountSent() - 1) * Multiplier;
        }
    }
}