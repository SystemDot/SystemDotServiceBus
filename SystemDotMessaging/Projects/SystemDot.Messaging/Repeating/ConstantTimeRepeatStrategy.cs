using System;
using System.Collections.Generic;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Storage;

namespace SystemDot.Messaging.Repeating
{
    public class ConstantTimeRepeatStrategy : LoggingRepeatStrategy, IRepeatStrategy
    {
        public TimeSpan RepeatEvery { get; set; }
        
        public void Repeat(MessageRepeater repeater, IMessageCache messageCache, ISystemTime systemTime)
        {
            IEnumerable<MessagePayload> messages = messageCache.GetOrderedMessages();

            messages.ForEach(m =>
            {
                if (m.GetAmountSent() > 0 && m.GetLastTimeSent() <= systemTime.GetCurrentDate().Add(-RepeatEvery))
                {
                    LogMessage(m);
                    repeater.InputMessage(m);
                }
            });
        }
    }
}