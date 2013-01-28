using System;
using System.Collections.Generic;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Storage;

namespace SystemDot.Messaging.Channels.Repeating
{
    public class ConstantTimeRepeatStrategy : IRepeatStrategy
    {
        readonly TimeSpan toRepeatEvery;

        public ConstantTimeRepeatStrategy(TimeSpan toRepeatEvery)
        {
            this.toRepeatEvery = toRepeatEvery;
        }

        public void Repeat(MessageRepeater repeater, MessageCache messageCache, ICurrentDateProvider currentDateProvider)
        {
            IEnumerable<MessagePayload> messages = messageCache.GetMessages();

            messages.ForEach(m =>
            {
                if (m.GetLastTimeSent() <= currentDateProvider.Get().Add(-this.toRepeatEvery))
                    repeater.InputMessage(m);
            });
        }

    }
}