using System;
using System.Collections.Generic;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Sending;
using SystemDot.Messaging.Storage;

namespace SystemDot.Messaging.Repeating
{
    public class ConstantTimeRepeatStrategy : IRepeatStrategy
    {
        readonly TimeSpan toRepeatEvery;
        bool isFirstRepeat;

        public ConstantTimeRepeatStrategy(TimeSpan toRepeatEvery)
        {
            this.toRepeatEvery = toRepeatEvery;
            this.isFirstRepeat = true;
        }

        public void Repeat(MessageRepeater repeater, IMessageCache messageCache, ISystemTime systemTime)
        {
            IEnumerable<MessagePayload> messages = messageCache.GetOrderedMessages();

            messages.ForEach(m =>
            {
                if ((m.IsMarkedAsSent() || this.isFirstRepeat) 
                    && m.GetLastTimeSent() <= systemTime.GetCurrentDate().Add(-this.toRepeatEvery))
                    repeater.InputMessage(m);
            });

            this.isFirstRepeat = false;
        }

    }
}