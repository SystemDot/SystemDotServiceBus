using SystemDot.Messaging.Packaging;

namespace SystemDot.Messaging.Sequencing
{
    public static class MessageFirstSequenceExtensions
    {
        public static int GetFirstSequence(this MessagePayload payload)
        {
            return payload.GetHeader<FirstSequenceHeader>().Sequence;
        }

        public static void SetFirstSequence(this MessagePayload payload, int sequence)
        {
            payload.RemoveHeader(typeof(FirstSequenceHeader));
            payload.AddHeader(new FirstSequenceHeader { Sequence = sequence });
        }
    }
}