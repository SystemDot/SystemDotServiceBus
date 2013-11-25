using System;
using SystemDot.Storage.Changes;

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
}
