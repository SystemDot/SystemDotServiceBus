using System.Collections.Generic;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Storage;
using SystemDot.Messaging.Sending;

namespace SystemDot.Messaging.Repeating
{
    public class EscalatingTimeRepeatStrategy : IRepeatStrategy
    {
        public static EscalatingTimeRepeatStrategy Default
        {
            get
            {
                return new EscalatingTimeRepeatStrategy(4, 2, 16);
            }
        }

        readonly int start;
        readonly int multiplier;
        readonly int maximum;
        bool isFirstRepeat;

        EscalatingTimeRepeatStrategy(int start, int multiplier, int maximum)
        {
            this.start = start;
            this.multiplier = multiplier;
            this.maximum = maximum;
            this.isFirstRepeat = true;
        }

        public void Repeat(
            MessageRepeater repeater, 
            IMessageCache messageCache, 
            ISystemTime systemTime)
        {
            IEnumerable<MessagePayload> messages = messageCache.GetOrderedMessages();

            messages.ForEach(m =>
            {
                if ((m.IsMarkedAsSent() || this.isFirstRepeat)  
                    && m.GetLastTimeSent().Ticks <= systemTime.GetCurrentDate().AddSeconds(-GetDelay(m)).Ticks)
                    repeater.InputMessage(m);
            });

            this.isFirstRepeat = false;
        }

        int GetDelay(MessagePayload toGetDelayFor)
        {
            int unlimitedDelay = GetUnlimitedDelay(toGetDelayFor);

            return unlimitedDelay < this.maximum 
                ? unlimitedDelay 
                : this.maximum;
        }

        int GetUnlimitedDelay(MessagePayload toGetDelayFor)
        {
            return toGetDelayFor.GetAmountSent() == 1 
                ? this.start 
                : this.start * GetMultiplier(toGetDelayFor);
        }

        int GetMultiplier(MessagePayload toGetDelayFor)
        {
            return (toGetDelayFor.GetAmountSent() - 1) * this.multiplier;
        }
    }
}