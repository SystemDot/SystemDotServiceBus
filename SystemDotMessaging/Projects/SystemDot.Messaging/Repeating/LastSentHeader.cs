using System;
using SystemDot.Messaging.Packaging;

namespace SystemDot.Messaging.Repeating
{
    public class LastSentHeader : IMessageHeader
    {
        public DateTime Time { get; set; }

        public int Amount { get; set; }

        public override string ToString()
        {
            return string.Concat(this.GetType(), ": ", this.Time, " ", this.Amount);
        }
    }
}