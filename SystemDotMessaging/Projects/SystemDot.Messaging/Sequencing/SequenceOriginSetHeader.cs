using System;
using SystemDot.Messaging.Packaging;

namespace SystemDot.Messaging.Sequencing
{
    public class SequenceOriginSetHeader : IMessageHeader
    {
        public DateTime SetOn { get; set; }

        public override string ToString()
        {
            return string.Format("SetOn: {0}", this.SetOn);
        }
    }
}