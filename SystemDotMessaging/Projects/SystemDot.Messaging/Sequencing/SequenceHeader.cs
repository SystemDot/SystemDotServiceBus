using SystemDot.Messaging.Packaging;

namespace SystemDot.Messaging.Sequencing
{
    public class SequenceHeader : IMessageHeader
    {
        public int Sequence { get; set; }
        
        public override string ToString()
        {
            return string.Concat("Sequence: ", Sequence.ToString());
        }
    }
  
}