namespace SystemDot.Messaging.Storage.Changes
{
    public class SetSequenceChange : AddMessageChange
    {
        public int Sequence { get; set; }

        public SetSequenceChange()
        {
        }

        public SetSequenceChange(int sequence)
        {
            Sequence = sequence;
        }
    }
}