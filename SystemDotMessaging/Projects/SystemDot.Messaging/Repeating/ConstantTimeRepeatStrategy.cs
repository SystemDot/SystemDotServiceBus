using System;
using System.Collections.Generic;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Storage;

namespace SystemDot.Messaging.Repeating
{
    public class ConstantTimeRepeatStrategy : IRepeatStrategy
    {
        readonly TimeSpan toRepeatEvery;

        public ConstantTimeRepeatStrategy(TimeSpan toRepeatEvery)
        {
            this.toRepeatEvery = toRepeatEvery;
        }

        public void Repeat(MessageRepeater repeater, IMessageCache messageCache, ISystemTime systemTime)
        {
            IEnumerable<MessagePayload> messages = messageCache.GetMessages();

            messages.ForEach(m =>
            {
                if (m.GetLastTimeSent() <= systemTime.GetCurrentDate().Add(-this.toRepeatEvery))
                    repeater.InputMessage(m);
            });
        }

    }
}