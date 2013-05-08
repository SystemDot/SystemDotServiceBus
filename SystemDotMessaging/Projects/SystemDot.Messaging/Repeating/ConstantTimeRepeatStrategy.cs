using System;
using System.Collections.Generic;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Storage;

namespace SystemDot.Messaging.Repeating
{
    public class ConstantTimeRepeatStrategy : LoggingRepeatStrategy, IRepeatStrategy
    {
        readonly TimeSpan toRepeatEvery;
        
        public ConstantTimeRepeatStrategy(TimeSpan toRepeatEvery)
        {
            this.toRepeatEvery = toRepeatEvery;
        }

        public void Repeat(MessageRepeater repeater, IMessageCache messageCache, ISystemTime systemTime)
        {
            IEnumerable<MessagePayload> messages = messageCache.GetOrderedMessages();

            messages.ForEach(m =>
            {
                if (m.GetAmountSent() > 0
                    && m.GetLastTimeSent() <= systemTime.GetCurrentDate().Add(-this.toRepeatEvery))
                {
                    LogMessage(m);
                    repeater.InputMessage(m);
                }
            });
        }
    }
}