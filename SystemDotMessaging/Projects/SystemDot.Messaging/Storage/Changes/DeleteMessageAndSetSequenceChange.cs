using System;

namespace SystemDot.Messaging.Storage.Changes
{
    public class DeleteMessageAndSetSequenceChange : Change
    {
        public Guid Id { get; set; }
        public int Sequence { get; set; }

        public DeleteMessageAndSetSequenceChange()
        {
        }

        public DeleteMessageAndSetSequenceChange(Guid id, int sequence)
        {
            Id = id;
            Sequence = sequence;
        }
    }

    public class MessageCheckpointChange : CheckPointChange
    {
        public int Sequence { get; set; }
    }
}
