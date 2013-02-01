using System.Collections.Generic;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Storage;

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

        public EscalatingTimeRepeatStrategy(int start, int multiplier, int maximum)
        {
            this.start = start;
            this.multiplier = multiplier;
            this.maximum = maximum;
        }

        public void Repeat(
            MessageRepeater repeater, 
            MessageCache messageCache, 
            ICurrentDateProvider currentDateProvider)
        {
            IEnumerable<MessagePayload> messages = messageCache.GetMessages();

            messages.ForEach(m =>
            {
                if (m.GetLastTimeSent().Ticks <= currentDateProvider.Get().AddSeconds(-GetDelay(m)).Ticks)
                    repeater.InputMessage(m);
            });
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