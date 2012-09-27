using System;
using SystemDot.Messaging.Channels.Packaging;

namespace SystemDot.Messaging.Channels.Repeating
{
    public class LastSentHeader : IMessageHeader
    {
        public DateTime Time { get; set; }

        public int Amount { get; set; }
    }
}