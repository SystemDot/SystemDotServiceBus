using System;
using System.Collections.Generic;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Storage;

namespace SystemDot.Messaging.Repeating
{
    public class ConstantTimeRepeatStrategy : LoggingRepeatStrategy, IRepeatStrategy
    {
        public static IRepeatStrategy EveryTenSeconds()
        {
            return new ConstantTimeRepeatStrategy { RepeatEvery = TimeSpan.FromSeconds(10) };
        }

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