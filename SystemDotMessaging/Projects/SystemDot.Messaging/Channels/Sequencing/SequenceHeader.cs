using SystemDot.Messaging.Channels.Packaging;

namespace SystemDot.Messaging.Channels.Sequencing
{
    public class SequenceHeader : IMessageHeader
    {
        public int Sequence { get; set; }
    }
}