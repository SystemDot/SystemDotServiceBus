using SystemDot.Messaging.Packaging;
using SystemDot.Storage.Changes;

namespace SystemDot.Messaging.Storage.Changes
{
    public class AddMessageAndIncrementSequenceChange : Change
    {
        public MessagePayload Message { get; set; }
        public int Sequence { get; set; }

        public AddMessageAndIncrementSequenceChange()
        {
        }

        public AddMessageAndIncrementSequenceChange(MessagePayload message, int sequence)
        {
            Message = message;
            Sequence = sequence;
        }
    }
}