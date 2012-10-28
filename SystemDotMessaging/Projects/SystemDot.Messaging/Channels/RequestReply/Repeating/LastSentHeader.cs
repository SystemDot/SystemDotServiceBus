using System;
using SystemDot.Messaging.Channels.Packaging;

namespace SystemDot.Messaging.Channels.RequestReply.Repeating
{
    public class LastSentHeader : IMessageHeader
    {
        public DateTime Time { get; set; }

        public int Amount { get; set; }

        public override string ToString()
        {
            return string.Concat(this.GetType(), ": ", Time, " ", Amount);
        }
    }
}