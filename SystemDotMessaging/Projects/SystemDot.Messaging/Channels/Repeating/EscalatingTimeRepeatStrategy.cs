using System.Collections.Generic;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Storage;

namespace SystemDot.Messaging.Channels.Repeating
{
    public class EscalatingTimeRepeatStrategy : IRepeatStrategy
    {
        readonly ICurrentDateProvider currentDateProvider;
        readonly MessageCache messageCache;

        public EscalatingTimeRepeatStrategy(ICurrentDateProvider currentDateProvider, MessageCache messageCache)
        {
            Contract.Requires(currentDateProvider != null);
            Contract.Requires(messageCache != null);
            
            this.currentDateProvider = currentDateProvider;
            this.messageCache = messageCache;
        }

        public void Repeat(MessageRepeater repeater)
        {
            IEnumerable<MessagePayload> messages = this.messageCache.GetMessages();

            messages.ForEach(m =>
            {
                if (m.GetLastTimeSent() <= this.currentDateProvider.Get().AddSeconds(-GetDelay(m)))
                    repeater.InputMessage(m);
            });
        }

        static int GetDelay(MessagePayload toGetDelayFor)
        {
            return toGetDelayFor.GetAmountSent() < 3 ? toGetDelayFor.GetAmountSent() : 4;
        }
    }
}