using System.Collections.Generic;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Storage;

namespace SystemDot.Messaging.Channels.Repeating
{
    public class EscalatingTimeRepeatStrategy : IRepeatStrategy
    {
        const int Start = 1;
        const int Multiplier = 2;
        const int Maximum = 4;
        
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

        static int GetDelay(MessagePayload toGetDelayFor)
        {
             return GetUnlimitedDelay(toGetDelayFor) < Maximum ? GetUnlimitedDelay(toGetDelayFor) : Maximum;
        }

        static int GetUnlimitedDelay(MessagePayload toGetDelayFor)
        {
            if (toGetDelayFor.GetAmountSent() == 1)
                return Start;

            return Start * ((toGetDelayFor.GetAmountSent() - 1) * Multiplier);
        }
    }
}