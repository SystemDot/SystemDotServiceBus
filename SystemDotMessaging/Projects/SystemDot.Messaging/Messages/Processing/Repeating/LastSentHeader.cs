using System;
using SystemDot.Messaging.Messages.Packaging;

namespace SystemDot.Messaging.Messages.Processing.Repeating
{
    public class LastSentHeader : IMessageHeader
    {
        public DateTime Time { get; set; }

        public int Amount { get; set; }
    }
}