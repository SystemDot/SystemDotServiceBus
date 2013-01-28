using System.Collections.Generic;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Storage;

namespace SystemDot.Messaging.Channels.Repeating
{
    public class EscalatingTimeRepeatStrategy : IRepeatStrategy
    {
        public void Repeat(MessageRepeater repeater, MessageCache messageCache, ICurrentDateProvider currentDateProvider)
        {
            IEnumerable<MessagePayload> messages = messageCache.GetMessages();

            messages.ForEach(m =>
            {
                if (m.GetLastTimeSent() <= currentDateProvider.Get().AddSeconds(-GetDelay(m)))
                    repeater.InputMessage(m);
            });
        }

        static int GetDelay(MessagePayload toGetDelayFor)
        {
            return toGetDelayFor.GetAmountSent() < 3 ? toGetDelayFor.GetAmountSent() : 4;
        }
    }
}