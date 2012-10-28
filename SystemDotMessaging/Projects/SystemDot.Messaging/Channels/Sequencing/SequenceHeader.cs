using SystemDot.Messaging.Channels.Packaging;

namespace SystemDot.Messaging.Channels.Sequencing
{
    public class SequenceHeader : IMessageHeader
    {
        public int Sequence { get; set; }
        
        public override string ToString()
        {
            return string.Concat(this.GetType() ,": ", Sequence.ToString());
        }
    }
  
}